package com.hoangcn.quizbattle.home_rank.activities;

import static android.view.View.GONE;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.home_rank.adapters.RankingAdapter;
import com.hoangcn.quizbattle.home_rank.api.HomeRankService;
import com.hoangcn.quizbattle.home_rank.models.UserRankListItem;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;

import java.util.ArrayList;
import java.util.List;

public class RankingActivity extends AppCompatActivity {
    private HomeRankService service;
    private String rankingTimeType = "Total";

    private TextView tabYearly;
    private TextView tabMonthly;
    private TextView tabTotal;

    private ImageView ivTop1Avatar;
    private ImageView ivTop2Avatar;
    private ImageView ivTop3Avatar;
    private TextView tvTop1Name;
    private TextView tvTop2Name;
    private TextView tvTop3Name;
    private TextView tvTop1Score;
    private TextView tvTop2Score;
    private TextView tvTop3Score;

    private ImageView ivCurrentUserAvatar;
    private TextView tvCurrentUserRank;
    private TextView tvCurrentUserName;
    private TextView tvCurrentUserScore;

    private RankingAdapter rankingAdapter;
    private RecyclerView rvRankingList;
    // toàn bộ dữ liệu từ API — dùng để tìm current user bất kể thứ hạng
    private List<UserRankListItem> allRanks = new ArrayList<>();
    // top 8 — dùng cho podium (top 3) và RecyclerView (rank 4-8)
    private List<UserRankListItem> ranks = new ArrayList<>();
    // rank 4-8 — dùng cho RecyclerView (top 3 đã hiển thị ở podium)
    private List<UserRankListItem> rankListItems = new ArrayList<>();

    private Button btnBackToGame;
    private MediaPlayer mediaPlayer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ranking);

        service = new HomeRankService(this);

        initViews();
        setListeners();
        initData();
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (mediaPlayer == null) {
            mediaPlayer = MediaPlayer.create(this, R.raw.rank);
            mediaPlayer.setLooping(true);
        }
        mediaPlayer.start();
    }

    @Override
    protected void onPause() {
        super.onPause();
        if (mediaPlayer != null && mediaPlayer.isPlaying()) {
            mediaPlayer.pause();
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mediaPlayer != null) {
            mediaPlayer.stop();
            mediaPlayer.release();
            mediaPlayer = null;
        }
    }

    private void initData() {
        loadRankBoard();
    }

    private void loadRankBoard() {
        service.getRankBoard(rankingTimeType, new ApiCallback<List<UserRankListItem>>() {
            @Override
            public void onSuccess(ApiResponse<List<UserRankListItem>> data) {
                List<UserRankListItem> all = data.getData();
                all.forEach(r -> r.setAvatarUrl(service.getFullImageUrl(r.getAvatarUrl())));
                runOnUiThread(() -> {
                    // Lưu toàn bộ để tìm current user bất kể thứ hạng
                    allRanks.clear();
                    allRanks.addAll(all);

                    // Giữ tối đa top 8 cho podium + list
                    ranks.clear();
                    ranks.addAll(all.subList(0, Math.min(8, all.size())));

                    // RecyclerView chỉ hiển thị rank 4-8
                    rankListItems.clear();
                    if (ranks.size() > 3) {
                        rankListItems.addAll(ranks.subList(3, ranks.size()));
                    }

                    rankingAdapter.notifyDataSetChanged();
                    setCurrentUserPlaceholder();
                    renderTop3();
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() ->
                    Toast.makeText(
                        RankingActivity.this,
                        "Lấy dữ liệu xếp hạng thất bại: " + message,
                        Toast.LENGTH_SHORT).show()
                );
            }
        });
    }

    private void initViews() {
        tabYearly = findViewById(R.id.tabYearly);
        tabMonthly = findViewById(R.id.tabMonthly);
        tabTotal = findViewById(R.id.tabTotal);

        ivTop1Avatar = findViewById(R.id.ivTop1Avatar);
        ivTop2Avatar = findViewById(R.id.ivTop2Avatar);
        ivTop3Avatar = findViewById(R.id.ivTop3Avatar);
        tvTop1Name = findViewById(R.id.ivTop1Name);
        tvTop2Name = findViewById(R.id.ivTop2Name);
        tvTop3Name = findViewById(R.id.ivTop3Name);
        tvTop1Score = findViewById(R.id.ivTop1Score);
        tvTop2Score = findViewById(R.id.ivTop2Score);
        tvTop3Score = findViewById(R.id.ivTop3Score);

        ivCurrentUserAvatar = findViewById(R.id.ivCurrentUserAvatar);
        tvCurrentUserRank = findViewById(R.id.tvCurrentUserRank);
        tvCurrentUserName = findViewById(R.id.tvCurrentUserName);
        tvCurrentUserScore = findViewById(R.id.tvCurrentUserScore);

        rvRankingList = findViewById(R.id.rvRankingList);
        rvRankingList.setLayoutManager(new LinearLayoutManager(this));
        rankingAdapter = new RankingAdapter(rankListItems);
        rvRankingList.setAdapter(rankingAdapter);

        btnBackToGame = findViewById(R.id.btnBackToGame);

        renderRankingType();
    }

    private void setListeners() {
        tabTotal.setOnClickListener(l -> {
            if (rankingTimeType.equals("Total")) return;
            rankingTimeType = "Total";
            renderRankingType();
            loadRankBoard();
        });

        tabMonthly.setOnClickListener(l -> {
            if (rankingTimeType.equals("Monthly")) return;
            rankingTimeType = "Monthly";
            renderRankingType();
            loadRankBoard();
        });

        tabYearly.setOnClickListener(l -> {
            if (rankingTimeType.equals("Yearly")) return;
            rankingTimeType = "Yearly";
            renderRankingType();
            loadRankBoard();
        });

        btnBackToGame.setOnClickListener(l -> finish());

        rankingAdapter.setOnItemClickListener(l ->
            Toast.makeText(this, "Chức năng đang phát triển", Toast.LENGTH_SHORT).show()
        );
    }

    private void renderTop3() {
        if (ranks.size() >= 1) {
            var top1Info = ranks.get(0);
            Glide.with(this).load(top1Info.getAvatarUrl())
                    .centerCrop().circleCrop().into(ivTop1Avatar);
            tvTop1Name.setText(top1Info.getDisplayName());
            tvTop1Score.setText(top1Info.getRankScore() + " point");
        } else {
            ivTop1Avatar.setVisibility(GONE);
            tvTop1Name.setText("-");
            tvTop1Score.setText("-");
        }

        if (ranks.size() >= 2) {
            var top2Info = ranks.get(1);
            Glide.with(this).load(top2Info.getAvatarUrl())
                    .centerCrop().circleCrop().into(ivTop2Avatar);
            tvTop2Name.setText(top2Info.getDisplayName());
            tvTop2Score.setText(top2Info.getRankScore() + " point");
        } else {
            ivTop2Avatar.setVisibility(GONE);
            tvTop2Name.setText("-");
            tvTop2Score.setText("-");
        }

        if (ranks.size() >= 3) {
            var top3Info = ranks.get(2);
            Glide.with(this).load(top3Info.getAvatarUrl())
                    .centerCrop().circleCrop().into(ivTop3Avatar);
            tvTop3Name.setText(top3Info.getDisplayName());
            tvTop3Score.setText(top3Info.getRankScore() + " point");
        } else {
            ivTop3Avatar.setVisibility(GONE);
            tvTop3Name.setText("-");
            tvTop3Score.setText("-");
        }
    }

    private void renderRankingType() {
        if (rankingTimeType.equals("Monthly")) {
            setTabSelected(tabMonthly, true);
            setTabSelected(tabYearly, false);
            setTabSelected(tabTotal, false);
        } else if (rankingTimeType.equals("Yearly")) {
            setTabSelected(tabMonthly, false);
            setTabSelected(tabYearly, true);
            setTabSelected(tabTotal, false);
        } else {
            setTabSelected(tabMonthly, false);
            setTabSelected(tabYearly, false);
            setTabSelected(tabTotal, true);
        }
    }

    private void setTabSelected(TextView tab, boolean selected) {
        tab.setBackgroundResource(selected ? R.drawable.bg_tab_selected : android.R.color.transparent);
        tab.setTextColor(ContextCompat.getColor(this, selected ? R.color.color2 : R.color.color11));
    }

    private void setCurrentUserPlaceholder() {
        int userId = SharedPreferenceUtil.getInstance(this).getInt("userId", -1);
        if (userId != -1) {
            var info = allRanks.stream().filter(r -> r.getUserId() == userId).findFirst();
            if (info.isPresent()) {
                Glide.with(this).load(info.get().getAvatarUrl())
                        .centerCrop().circleCrop().into(ivCurrentUserAvatar);
                tvCurrentUserRank.setText(String.valueOf(info.get().getRank()));
                tvCurrentUserName.setText(info.get().getDisplayName());
                tvCurrentUserScore.setText(info.get().getRankScore() + " point");
                return;
            }
        }

        ivCurrentUserAvatar.setVisibility(GONE);
        tvCurrentUserRank.setText("-");
        tvCurrentUserName.setText("Đăng nhập để xem xếp hạng của bạn");
        tvCurrentUserScore.setText("0 point");
    }
}
