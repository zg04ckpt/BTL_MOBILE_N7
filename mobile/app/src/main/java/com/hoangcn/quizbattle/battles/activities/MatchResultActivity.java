package com.hoangcn.quizbattle.battles.activities;

import android.widget.Button;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.home_rank.activities.HomeActivity;
import com.hoangcn.quizbattle.battles.models.MatchResultUserItem;
import com.hoangcn.quizbattle.battles.models.MatchReviewResponse;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class MatchResultActivity extends AppCompatActivity {
    private BattleService battleService;
    private final List<View> rankRows = new ArrayList<>();
    private final List<TextView> rankNames = new ArrayList<>();
    private final List<TextView> rankScores = new ArrayList<>();
    private final List<TextView> rankXps = new ArrayList<>();
    private final List<TextView> rankRanks = new ArrayList<>();
    private final List<ImageView> rankAvatars = new ArrayList<>();
    private TextView tvTitle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_match_result);

        bindViews();
        Button btnHome = findViewById(R.id.btn_home);
        btnHome.setOnClickListener(v -> {
            var intent = new android.content.Intent(this, HomeActivity.class);
            intent.setFlags(android.content.Intent.FLAG_ACTIVITY_CLEAR_TOP | android.content.Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        battleService = new BattleService(this);
        int matchId = getIntent().getIntExtra("matchId", 0);
        if (matchId > 0) {
            loadMatchReview(matchId);
        }
    }

    private void loadMatchReview(int matchId) {
        battleService.getMatchReview(matchId, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchReviewResponse> data) {
                if (data == null || data.getData() == null) return;
                runOnUiThread(() -> renderReview(data.getData()));
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    tvTitle.setText("Không thể tải kết quả: " + message);
                });
            }
        });
    }

    private void bindViews() {
        tvTitle = findViewById(R.id.tv_title);

        rankRows.add(findViewById(R.id.row_rank1));
        rankRows.add(findViewById(R.id.row_rank2));
        rankRows.add(findViewById(R.id.row_rank3));
        rankRows.add(findViewById(R.id.row_rank4));
        rankRows.add(findViewById(R.id.row_rank5));

        rankNames.add(findViewById(R.id.tv_name_rank1));
        rankNames.add(findViewById(R.id.tv_name_rank2));
        rankNames.add(findViewById(R.id.tv_name_rank3));
        rankNames.add(findViewById(R.id.tv_name_rank4));
        rankNames.add(findViewById(R.id.tv_name_rank5));

        rankScores.add(findViewById(R.id.tv_score_rank1));
        rankScores.add(findViewById(R.id.tv_score_rank2));
        rankScores.add(findViewById(R.id.tv_score_rank3));
        rankScores.add(findViewById(R.id.tv_score_rank4));
        rankScores.add(findViewById(R.id.tv_score_rank5));

        rankXps.add(findViewById(R.id.tv_xp_rank1));
        rankXps.add(findViewById(R.id.tv_xp_rank2));
        rankXps.add(findViewById(R.id.tv_xp_rank3));
        rankXps.add(findViewById(R.id.tv_xp_rank4));
        rankXps.add(findViewById(R.id.tv_xp_rank5));

        rankRanks.add(findViewById(R.id.tv_diamond_rank1));
        rankRanks.add(findViewById(R.id.tv_diamond_rank2));
        rankRanks.add(findViewById(R.id.tv_diamond_rank3));
        rankRanks.add(findViewById(R.id.tv_diamond_rank4));
        rankRanks.add(findViewById(R.id.tv_diamond_rank5));

        rankAvatars.add(findViewById(R.id.iv_avatar_rank1));
        rankAvatars.add(findViewById(R.id.iv_avatar_rank2));
        rankAvatars.add(findViewById(R.id.iv_avatar_rank3));
        rankAvatars.add(findViewById(R.id.iv_avatar_rank4));
        rankAvatars.add(findViewById(R.id.iv_avatar_rank5));
    }

    private void renderReview(MatchReviewResponse response) {
        var users = response.getUsers() == null ? new ArrayList<MatchResultUserItem>() : new ArrayList<>(response.getUsers());
        users.sort(Comparator.comparingInt(MatchResultUserItem::getScore).reversed().thenComparing(MatchResultUserItem::getUserId));

        for (int i = 0; i < rankRows.size(); i++) {
            if (i < users.size()) {
                var user = users.get(i);
                rankRows.get(i).setVisibility(View.VISIBLE);
                rankNames.get(i).setText((i + 1) + ". " + safe(user.getName()));
                rankScores.get(i).setText(String.valueOf(user.getScore()));
                rankXps.get(i).setText((user.getExpGained() >= 0 ? "+" : "") + user.getExpGained() + "XP");
                rankRanks.get(i).setText((user.getRankScoreGained() >= 0 ? "+" : "") + user.getRankScoreGained() + " rank");
                bindAvatar(rankAvatars.get(i), user.getAvatarUrl());
            } else {
                rankRows.get(i).setVisibility(View.GONE);
            }
        }

        int totalQuestions = response.getQuestionReviews() == null ? 0 : response.getQuestionReviews().size();
        tvTitle.setText("Kết quả xếp hạng - " + totalQuestions + " câu");
    }

    private String safe(String value) {
        return value == null || value.isEmpty() ? "Người chơi" : value;
    }

    private void bindAvatar(ImageView imageView, String avatarUrl) {
        if (avatarUrl == null || avatarUrl.isEmpty()) {
            imageView.setImageResource(R.drawable.avatar_current_user);
            return;
        }

        var fullUrl = avatarUrl.startsWith("/")
            ? "https://quizbattle.hoangcn.com" + avatarUrl
            : avatarUrl;

        Glide.with(this)
            .load(fullUrl)
            .centerCrop()
            .placeholder(R.drawable.avatar_current_user)
            .error(R.drawable.avatar_current_user)
            .into(imageView);
    }
}