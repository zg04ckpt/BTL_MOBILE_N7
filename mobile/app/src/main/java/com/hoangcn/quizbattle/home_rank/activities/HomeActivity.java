package com.hoangcn.quizbattle.home_rank.activities;

import android.content.Intent;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.viewpager2.widget.ViewPager2;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.activities.EventActivity;
import com.hoangcn.quizbattle.home_rank.adapters.HomeEventAdapter;
import com.hoangcn.quizbattle.home_rank.api.HomeRankService;
import com.hoangcn.quizbattle.home_rank.models.UserProfile;
import com.hoangcn.quizbattle.home_rank.models.OngoingEvent;
import com.hoangcn.quizbattle.lobby.activities.MatchConfigActivity;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;
import com.hoangcn.quizbattle.users.activities.LoginActivity;
import com.hoangcn.quizbattle.users.activities.ProfileActivity;

import java.util.List;
import java.util.concurrent.Executors;

public class HomeActivity extends AppCompatActivity {
    public static final String EXTRA_HOME_USER = "extra_home_user";

    private HomeRankService api;

    private ImageView ivAvatar;
    private TextView tvUsername;
    private TextView tvLevel;
    private TextView tvPbStart;
    private TextView tvPbEnd;
    private TextView tvPercentExp;
    private ProgressBar pbExp;
    private ViewPager2 vpEvents;
    private MediaPlayer mediaPlayer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (!hasValidSession()) {
            redirectToLogin();
            return;
        }

        setContentView(R.layout.activity_home);

        api = new HomeRankService(this);

        initViews();
        setListeners();
        loadData();
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (!hasValidSession()) {
            redirectToLogin();
            return;
        }
        loadData();
        if (mediaPlayer == null) {
            mediaPlayer = MediaPlayer.create(this, R.raw.home);
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

    private void loadData() {
        getUserStats();
        getListEvents();
    }

    private void getListEvents() {
        Executors.newSingleThreadExecutor().execute(() -> {
            api.getOngoingEvents(new ApiCallback<List<OngoingEvent>>() {
                @Override
                public void onSuccess(ApiResponse<List<OngoingEvent>> data) {
                    runOnUiThread(() -> renderListEvents(data.getData()));
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> {
                        Toast.makeText(
                                HomeActivity.this,
                                "Tải dữ liệu sự kiện thất bại: " + message,
                                Toast.LENGTH_SHORT).show();
                    });
                }
            });
        });
    }

    private void renderListEvents(List<OngoingEvent> data) {
        var adapter = new HomeEventAdapter(data);
        adapter.setOnItemClickedListener(event -> {
            startActivity(new Intent(this, EventActivity.class));
        });
        vpEvents.setAdapter(adapter);
    }

    private void getUserStats() {
        Executors.newSingleThreadExecutor().execute(() -> {
            api.getProfile(new ApiCallback<UserProfile>() {
                @Override
                public void onSuccess(ApiResponse<UserProfile> data) {
                    SharedPreferenceUtil.getInstance(HomeActivity.this)
                            .putInt("userId", data.getData().getId());
                    runOnUiThread(() -> {
                        renderUserStats(data.getData());
                    });
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> {
                        Toast.makeText(
                                HomeActivity.this,
                                "Tải dữ liệu user thất bại: " + message,
                                Toast.LENGTH_SHORT).show();
                    });
                }
            });
        });
    }

    private void renderUserStats(UserProfile data) {
        tvUsername.setText(data.getName());
        tvLevel.setText("Lv. " + data.getLevel());
        tvPbStart.setText("Lv. " + data.getLevel());
        tvPbEnd.setText("Lv. " + (data.getLevel() + 1));
        Glide.with(this).load(api.getFullImageUrl(data.getAvatarUrl()))
                .centerCrop().circleCrop().into(ivAvatar);

        int progress = (int) (data.getExp() * 100.0 / data.getExpToUpLevel());
        tvPercentExp.setText(String.format("%d%%", progress));
        pbExp.setProgress(progress);

        setStatItem(
                findViewById(R.id.include_item_score),
                R.drawable.ic_home_rank_point,
                "Điểm xếp hạng",
                String.valueOf(data.getRankScore()));

        setStatItem(
                findViewById(R.id.include_item_rank),
                R.drawable.ic_home_rank,
                "Thứ hạng",
                "#" + data.getRank());

        setStatItem(
                findViewById(R.id.include_item_matches),
                R.drawable.ic_home_number_match,
                "Số trận",
                String.valueOf(data.getNumberOfMatchs()));

        setStatItem(
                findViewById(R.id.include_item_win_rate),
                R.drawable.ic_home_win_rate,
                "Tỷ lệ thắng",
                String.format("%.2f%%", data.getWinningRate()));
    }

    private void initViews() {
        ivAvatar = findViewById(R.id.iv_avatar);
        tvUsername = findViewById(R.id.tv_username);
        tvLevel = findViewById(R.id.tv_level);
        tvPbStart = findViewById(R.id.tv_pb_start);
        tvPbEnd = findViewById(R.id.tv_pb_end);
        tvPercentExp = findViewById(R.id.tv_percent_exp);
        pbExp = findViewById(R.id.pb_exp);
        setupBottomNavigation();
        vpEvents = findViewById(R.id.vpEvents);
    }

    private void setListeners() {
//        findViewById(R.id.iv_bell).setOnClickListener(v -> onNotificationClick());
        findViewById(R.id.card_start_game).setOnClickListener(v ->
                startActivity(new Intent(this, MatchConfigActivity.class)));
//        findViewById(R.id.tv_start_game).setOnClickListener(v -> onStartGameClick());
//        findViewById(R.id.iv_event_image).setOnClickListener(v -> onEventBannerClick());
//        findViewById(R.id.card_event_banner).setOnClickListener(v -> onEventBannerClick());
        findViewById(R.id.tvViewAllEvents).setOnClickListener(l -> {
            startActivity(new Intent(this, EventActivity.class));
        });
    }

    private void setupBottomNavigation() {
        BottomNavigationView bottomNav = findViewById(R.id.bottom_navigation);
        bottomNav.setSelectedItemId(R.id.nav_home);
        bottomNav.setOnItemSelectedListener(item -> {
            int itemId = item.getItemId();
            if (itemId == R.id.nav_home) {
                return true;
            }
            if (itemId == R.id.nav_rank) {
                startActivity(new Intent(this, RankingActivity.class));
                return true;
            }
            if (itemId == R.id.nav_battle) {
                startActivity(new Intent(this, MatchConfigActivity.class));
                return true;
            }
            if (itemId == R.id.nav_profile) {
                startActivity(new Intent(this, ProfileActivity.class));
                return true;
            }
            return false;
        });
    }


    private void applyGuestData() {
        tvUsername.setText("guest");
        tvLevel.setText("Lv. 1");
        tvPbStart.setText("Lv. 1");
        tvPbEnd.setText("Lv. 2");
        tvPercentExp.setText("0%");
        pbExp.setProgress(0);

        setStatItem(findViewById(R.id.include_item_score), R.drawable.ic_home_rank_point, "Diem xep hang", "0");
        setStatItem(findViewById(R.id.include_item_rank), R.drawable.ic_home_rank, "Thu hang", "#- ");
        setStatItem(findViewById(R.id.include_item_matches), R.drawable.ic_home_number_match, "So tran", "0");
        setStatItem(findViewById(R.id.include_item_win_rate), R.drawable.ic_home_win_rate, "Ty le thang", "0%");
    }

    private void onNotificationClick() {
        Toast.makeText(this, "Thong bao chua duoc mo", Toast.LENGTH_SHORT).show();
    }

    public void setStatItem(View includeView, int iconRes, String label, String value) {
        ImageView icon = includeView.findViewById(R.id.iv_stat_icon);
        TextView tvLabel = includeView.findViewById(R.id.tv_stat_label);
        TextView tvValue = includeView.findViewById(R.id.tv_stat_value);

        icon.setImageResource(iconRes);
        tvLabel.setText(label);
        tvValue.setText(value);
    }

    private boolean hasValidSession() {
        var prefs = SharedPreferenceUtil.getInstance(this);
        int userId = prefs.getInt("userId", -1);
        String accessToken = prefs.getString("accessToken", null);
        return userId > 0 && accessToken != null && !accessToken.isEmpty();
    }

    private void redirectToLogin() {
        Intent intent = new Intent(this, LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
    }
}