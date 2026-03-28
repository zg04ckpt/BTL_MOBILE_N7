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

import java.lang.reflect.Method;
import java.util.Locale;

public class HomeActivity extends AppCompatActivity {
    public static final String EXTRA_HOME_USER = "extra_home_user";

    private HomeRankRepository repository;
    private SessionManager sessionManager;
    private HomeProfileModel currentUser;

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

        currentUser = readUserFromIntent();
        if (currentUser == null) {
            // Test-branch fallback only: ensure activity still has a valid user context.
            currentUser = createDefaultUserForTest();
        }

        sessionManager.saveUserId(currentUser.getId());
        bindHomeProfile(currentUser);
        refreshHomeDataFromApi();
    }

    private HomeProfileModel createDefaultUserForTest() {
        return new HomeProfileModel(
                2,
                "Admin2",
                "",
                1,
                0,
                0,
                0,
                0,
                0f,
                0
        );
    }

    private HomeProfileModel readUserFromIntent() {
        Intent intent = getIntent();
        if (intent == null) {
            return null;
        }

        Object rawUser = intent.getSerializableExtra(EXTRA_HOME_USER);
        if (rawUser instanceof HomeProfileModel) {
            return (HomeProfileModel) rawUser;
        }

        if (rawUser != null) {
            return mapFromGenericUser(rawUser);
        }

        return null;
    }

    private HomeProfileModel mapFromGenericUser(Object rawUser) {
        try {
            int id = invokeInt(rawUser, "getId", -1);
            if (id <= 0) {
                return null;
            }

            String name = invokeString(rawUser, "getName", "guest");
            String avatarUrl = invokeString(rawUser, "getAvatarUrl", "");
            int level = invokeInt(rawUser, "getLevel", 1);
            int exp = invokeInt(rawUser, "getExp", 0);
            int rank = invokeInt(rawUser, "getRank", 0);
            int rankScore = invokeInt(rawUser, "getRankScore", 0);
            int numberOfMatchs = invokeInt(rawUser, "getNumberOfMatchs", 0);
            float winningRate = invokeFloat(rawUser, "getWinningRate", 0f);
            int winningStreak = invokeInt(rawUser, "getWinningStreak", 0);

            return new HomeProfileModel(
                    id,
                    name,
                    avatarUrl,
                    level,
                    exp,
                    rank,
                    rankScore,
                    numberOfMatchs,
                    winningRate,
                    winningStreak
            );
        } catch (Exception ignored) {
            return null;
        }
    }

    private int invokeInt(Object target, String methodName, int defaultValue) {
        try {
            Method method = target.getClass().getMethod(methodName);
            Object value = method.invoke(target);
            if (value instanceof Number) {
                return ((Number) value).intValue();
            }
        } catch (Exception ignored) {
        }
        return defaultValue;
    }

    private float invokeFloat(Object target, String methodName, float defaultValue) {
        try {
            Method method = target.getClass().getMethod(methodName);
            Object value = method.invoke(target);
            if (value instanceof Number) {
                return ((Number) value).floatValue();
            }
        } catch (Exception ignored) {
        }
        return defaultValue;
    }

    private String invokeString(Object target, String methodName, String defaultValue) {
        try {
            Method method = target.getClass().getMethod(methodName);
            Object value = method.invoke(target);
            if (value instanceof String) {
                return (String) value;
            }
        } catch (Exception ignored) {
        }
        return defaultValue;
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
                startActivityWithCurrentUser(RankingActivity.class);
                return true;
            }
            if (itemId == R.id.nav_battle) {
                startActivityWithCurrentUser(MatchConfigActivity.class);
                return true;
            }
            if (itemId == R.id.nav_profile) {
                startActivityWithCurrentUser(ProfileActivity.class);
                return true;
            }
            return false;
        });
    }

    private void startActivityWithCurrentUser(Class<?> targetActivity) {
        Intent intent = new Intent(this, targetActivity);
        if (currentUser != null) {
            intent.putExtra(EXTRA_HOME_USER, currentUser);
        }
        startActivity(intent);
    }

    private void refreshHomeDataFromApi() {
        String token = sessionManager.getAccessToken();
        if (token != null && !token.isEmpty()) {
            loadHomeDataByProfileApi(token);
            return;
        }

        loadHomeStatsByRankApi();
    }

    private void loadHomeDataByProfileApi(String token) {
        repository.getHomeProfile(token, new RepositoryCallback<>() {
            @Override
            public void onSuccess(HomeProfileModel data) {
                if (data == null) {
                    return;
                }
                currentUser = data;
                bindHomeProfile(data);
                sessionManager.saveUserId(data.getId());
            }

            @Override
            public void onError(String message) {
                Toast.makeText(HomeActivity.this, message, Toast.LENGTH_SHORT).show();
                loadHomeStatsByRankApi();
            }
        });
    }

    private void loadHomeStatsByRankApi() {
        if (currentUser == null || currentUser.getId() <= 0) {
            applyGuestData();
            return;
        }

        repository.getRankDetail(currentUser.getId(), new RepositoryCallback<>() {
            @Override
            public void onSuccess(com.n7.quizbattle.home_rank.models.RankDetailModel data) {
                if (data == null) {
                    return;
                }
                bindHomeStatsFromRankDetail(data);
            }

            @Override
            public void onError(String message) {
                Toast.makeText(HomeActivity.this, message, Toast.LENGTH_SHORT).show();
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

        bindHomeStats(profile);
    }

    private void bindHomeStats(HomeProfileModel profile) {
        int rankScore = Math.max(0, profile.getRankScore());
        int rank = profile.getRank();
        int numberOfMatches = Math.max(0, profile.getNumberOfMatchs());

        float winRate = profile.getWinningRate();
        // Some services return ratio (0..1), others return percent (0..100).
        if (winRate > 0f && winRate <= 1f) {
            winRate = winRate * 100f;
        }
        winRate = Math.max(0f, Math.min(100f, winRate));

        String rankText = rank > 0 ? ("#" + rank) : "#-";
        String winRateText = String.format(Locale.US, "%.1f%%", winRate);

        setStatItem(findViewById(R.id.include_item_score), R.drawable.ic_home_rank_point, "Diem xep hang", String.valueOf(rankScore));
        setStatItem(findViewById(R.id.include_item_rank), R.drawable.ic_home_rank, "Thu hang", rankText);
        setStatItem(findViewById(R.id.include_item_matches), R.drawable.ic_home_number_match, "So tran", String.valueOf(numberOfMatches));
        setStatItem(findViewById(R.id.include_item_win_rate), R.drawable.ic_home_win_rate, "Ty le thang", winRateText);
    }

    private void bindHomeStatsFromRankDetail(com.n7.quizbattle.home_rank.models.RankDetailModel detail) {
        int rankScore = Math.max(0, detail.getRankScore());
        int rank = detail.getRank();
        int numberOfMatches = Math.max(0, detail.getNumberOfMatchs());

        float winRate = detail.getWinningRate();
        if (winRate > 0f && winRate <= 1f) {
            winRate = winRate * 100f;
        }
        winRate = Math.max(0f, Math.min(100f, winRate));

        String rankText = rank > 0 ? ("#" + rank) : "#-";
        String winRateText = String.format(Locale.US, "%.1f%%", winRate);

        setStatItem(findViewById(R.id.include_item_score), R.drawable.ic_home_rank_point, "Diem xep hang", String.valueOf(rankScore));
        setStatItem(findViewById(R.id.include_item_rank), R.drawable.ic_home_rank, "Thu hang", rankText);
        setStatItem(findViewById(R.id.include_item_matches), R.drawable.ic_home_number_match, "So tran", String.valueOf(numberOfMatches));
        setStatItem(findViewById(R.id.include_item_win_rate), R.drawable.ic_home_win_rate, "Ty le thang", winRateText);
    }

    private void onNotificationClick() {
        Toast.makeText(this, "Thong bao chua duoc mo", Toast.LENGTH_SHORT).show();
    }

    private void onStartGameClick() {
        startActivityWithCurrentUser(MatchConfigActivity.class);
    }

    private void onEventBannerClick() {
        startActivityWithCurrentUser(EventActivity.class);
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