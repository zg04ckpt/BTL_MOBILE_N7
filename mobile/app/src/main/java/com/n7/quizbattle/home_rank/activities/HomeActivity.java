package com.n7.quizbattle.home_rank.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.n7.quizbattle.R;
import com.n7.quizbattle.events.activities.EventActivity;
import com.n7.quizbattle.home_rank.models.HomeProfileModel;
import com.n7.quizbattle.home_rank.repositories.HomeRankRepository;
import com.n7.quizbattle.home_rank.repositories.RepositoryCallback;
import com.n7.quizbattle.home_rank.repositories.SessionManager;
import com.n7.quizbattle.lobby.activities.MatchConfigActivity;
import com.n7.quizbattle.users.activities.ProfileActivity;
import com.google.android.material.bottomnavigation.BottomNavigationView;

public class HomeActivity extends AppCompatActivity {
    private HomeRankRepository repository;
    private SessionManager sessionManager;

    private TextView tvUsername;
    private TextView tvLevel;
    private TextView tvPbStart;
    private TextView tvPbEnd;
    private TextView tvPercentExp;
    private ProgressBar pbExp;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_home);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        repository = new HomeRankRepository();
        sessionManager = new SessionManager(this);

        initViews();
        setupClickHandlers();
        setupBottomNavigation();
        loadHomeData();
    }

    private void initViews() {
        tvUsername = findViewById(R.id.tv_username);
        tvLevel = findViewById(R.id.tv_level);
        tvPbStart = findViewById(R.id.tv_pb_start);
        tvPbEnd = findViewById(R.id.tv_pb_end);
        tvPercentExp = findViewById(R.id.tv_percent_exp);
        pbExp = findViewById(R.id.pb_exp);
    }

    private void setupClickHandlers() {
        findViewById(R.id.iv_bell).setOnClickListener(v -> onNotificationClick());
        findViewById(R.id.card_start_game).setOnClickListener(v -> onStartGameClick());
        findViewById(R.id.tv_start_game).setOnClickListener(v -> onStartGameClick());
        findViewById(R.id.iv_event_image).setOnClickListener(v -> onEventBannerClick());
        findViewById(R.id.card_event_banner).setOnClickListener(v -> onEventBannerClick());
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

    private void loadHomeData() {
        String token = sessionManager.getAccessToken();
        if (token == null || token.isEmpty()) {
            applyGuestData();
            return;
        }

        repository.getHomeProfile(token, new RepositoryCallback<>() {
            @Override
            public void onSuccess(HomeProfileModel data) {
                bindHomeProfile(data);
                sessionManager.saveUserId(data.getId());
            }

            @Override
            public void onError(String message) {
                Toast.makeText(HomeActivity.this, message, Toast.LENGTH_SHORT).show();
                applyGuestData();
            }
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

    private void bindHomeProfile(HomeProfileModel profile) {
        tvUsername.setText(profile.getName());
        tvLevel.setText("Lv. " + profile.getLevel());
        tvPbStart.setText("Lv. " + profile.getLevel());
        tvPbEnd.setText("Lv. " + (profile.getLevel() + 1));

        int progress = Math.max(0, Math.min(100, profile.getExp() % 100));
        pbExp.setProgress(progress);
        tvPercentExp.setText(progress + "%");

        setStatItem(findViewById(R.id.include_item_score), R.drawable.ic_home_rank_point, "Diem xep hang", String.valueOf(profile.getRankScore()));
        setStatItem(findViewById(R.id.include_item_rank), R.drawable.ic_home_rank, "Thu hang", "#" + profile.getRank());
        setStatItem(findViewById(R.id.include_item_matches), R.drawable.ic_home_number_match, "So tran", String.valueOf(profile.getNumberOfMatchs()));
        setStatItem(findViewById(R.id.include_item_win_rate), R.drawable.ic_home_win_rate, "Ty le thang", String.format("%.1f%%", profile.getWinningRate()));
    }

    private void onNotificationClick() {
        Toast.makeText(this, "Thong bao chua duoc mo", Toast.LENGTH_SHORT).show();
    }

    private void onStartGameClick() {
        startActivity(new Intent(this, MatchConfigActivity.class));
    }

    private void onEventBannerClick() {
        startActivity(new Intent(this, EventActivity.class));
    }

    public void setStatItem(View includeView, int iconRes, String label, String value) {
        ImageView icon = includeView.findViewById(R.id.iv_stat_icon);
        TextView tvLabel = includeView.findViewById(R.id.tv_stat_label);
        TextView tvValue = includeView.findViewById(R.id.tv_stat_value);

        icon.setImageResource(iconRes);
        tvLabel.setText(label);
        tvValue.setText(value);
    }
}