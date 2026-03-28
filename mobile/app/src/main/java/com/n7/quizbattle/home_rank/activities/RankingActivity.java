package com.n7.quizbattle.home_rank.activities;

import android.content.Intent;
import android.os.Bundle;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.n7.quizbattle.R;
import com.n7.quizbattle.home_rank.adapters.RankingAdapter;
import com.n7.quizbattle.home_rank.models.HomeProfileModel;
import com.n7.quizbattle.home_rank.models.RankBoardItemModel;
import com.n7.quizbattle.home_rank.models.RankDetailModel;
import com.n7.quizbattle.home_rank.models.RankTypeModel;
import com.n7.quizbattle.home_rank.repositories.HomeRankRepository;
import com.n7.quizbattle.home_rank.repositories.RepositoryCallback;
import com.n7.quizbattle.home_rank.repositories.SessionManager;

import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;

public class RankingActivity extends AppCompatActivity {
    private HomeRankRepository repository;
    private SessionManager sessionManager;
    private HomeProfileModel currentUser;

    private TextView tabWeekly;
    private TextView tabMonthly;
    private TextView tabTotal;
    private TextView tabFriend;
    private TextView tabGlobal;

    private TextView tvRank1Name;
    private TextView tvRank1Score;
    private TextView tvRank2Name;
    private TextView tvRank2Score;
    private TextView tvRank3Name;
    private TextView tvRank3Score;

    private TextView tvCurrentUserRank;
    private TextView tvCurrentUserName;
    private TextView tvCurrentUserScore;

    private RankingAdapter adapter;
    private String selectedApiType = "Monthly";
    private String weeklyApiType = "Monthly";
    private String monthlyApiType = "Yearly";
    private String totalApiType = "Total";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_ranking);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        repository = new HomeRankRepository();
        sessionManager = new SessionManager(this);
        currentUser = readUserFromIntent();
        if (currentUser != null && currentUser.getId() > 0) {
            sessionManager.saveUserId(currentUser.getId());
        }

        initViews();
        setupRecyclerView();
        setupControlHandlers();
        loadRankingTypesAndBoard();
        loadCurrentUserRank();
    }

    private HomeProfileModel readUserFromIntent() {
        Intent intent = getIntent();
        if (intent == null) {
            return null;
        }

        Object rawUser = intent.getSerializableExtra(HomeActivity.EXTRA_HOME_USER);
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
        tabWeekly = findViewById(R.id.tabWeekly);
        tabMonthly = findViewById(R.id.tabMonthly);
        tabTotal = findViewById(R.id.tabTotal);
        tabFriend = findViewById(R.id.tabFriend);
        tabGlobal = findViewById(R.id.tabGlobal);

        tvRank1Name = findViewById(R.id.tvRank1Name);
        tvRank1Score = findViewById(R.id.tvRank1Score);
        tvRank2Name = findViewById(R.id.tvRank2Name);
        tvRank2Score = findViewById(R.id.tvRank2Score);
        tvRank3Name = findViewById(R.id.tvRank3Name);
        tvRank3Score = findViewById(R.id.tvRank3Score);

        tvCurrentUserRank = findViewById(R.id.tvCurrentUserRank);
        tvCurrentUserName = findViewById(R.id.tvCurrentUserName);
        tvCurrentUserScore = findViewById(R.id.tvCurrentUserScore);
    }

    private void setupRecyclerView() {
        RecyclerView recyclerView = findViewById(R.id.rvRankingList);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));

        adapter = new RankingAdapter(this::onRankItemClick);
        recyclerView.setAdapter(adapter);
    }

    private void setupControlHandlers() {
        tabWeekly.setOnClickListener(v -> onRankingTypeChanged(weeklyApiType, tabWeekly));
        tabMonthly.setOnClickListener(v -> onRankingTypeChanged(monthlyApiType, tabMonthly));
        tabTotal.setOnClickListener(v -> onRankingTypeChanged(totalApiType, tabTotal));

        tabFriend.setOnClickListener(v -> onRankingScopeChanged("Friend"));
        tabGlobal.setOnClickListener(v -> onRankingScopeChanged("Global"));

        findViewById(R.id.btnBackToGame).setOnClickListener(v -> onBackToGameClick());

        // Current backend chi ho tro global ranking, nen boi den Global mac dinh.
        onRankingScopeChanged("Global");
    }

    private void loadRankingTypesAndBoard() {
        repository.getRankTypes(new RepositoryCallback<>() {
            @Override
            public void onSuccess(List<RankTypeModel> data) {
                applyApiTypeMapping(data);
                onRankingTypeChanged(weeklyApiType, tabWeekly);
            }

            @Override
            public void onError(String message) {
                // Fallback to default mapping if rank type API fails.
                onRankingTypeChanged(weeklyApiType, tabWeekly);
            }
        });
    }

    private void applyApiTypeMapping(List<RankTypeModel> apiTypes) {
        List<String> values = new ArrayList<>();
        if (apiTypes != null) {
            for (RankTypeModel item : apiTypes) {
                if (item != null && item.getValue() != null) {
                    values.add(item.getValue());
                }
            }
        }

        if (!values.isEmpty()) {
            weeklyApiType = pickType(values, "Monthly", values.get(0));
            monthlyApiType = pickType(values, "Yearly", weeklyApiType);
            totalApiType = pickType(values, "Total", monthlyApiType);
        }
    }

    private String pickType(List<String> types, String preferred, String fallback) {
        for (String type : types) {
            if (preferred.equalsIgnoreCase(type)) {
                return type;
            }
        }
        return fallback;
    }

    private void onRankingTypeChanged(String apiType, TextView selectedTab) {
        selectedApiType = apiType;

        setTabSelected(tabWeekly, false);
        setTabSelected(tabMonthly, false);
        setTabSelected(tabTotal, false);
        setTabSelected(selectedTab, true);

        loadRankingBoard();
    }

    private void setTabSelected(TextView tab, boolean selected) {
        tab.setBackgroundResource(selected ? R.drawable.bg_tab_selected : android.R.color.transparent);
        tab.setTextColor(ContextCompat.getColor(this, selected ? R.color.color2 : R.color.color11));
    }

    private void onRankingScopeChanged(String scope) {
        boolean isGlobal = "Global".equals(scope);
        tabGlobal.setBackgroundResource(isGlobal ? R.drawable.bg_tab_selected : android.R.color.transparent);
        tabFriend.setBackgroundResource(!isGlobal ? R.drawable.bg_tab_selected : android.R.color.transparent);
        tabGlobal.setTextColor(ContextCompat.getColor(this, isGlobal ? R.color.color2 : R.color.color11));
        tabFriend.setTextColor(ContextCompat.getColor(this, !isGlobal ? R.color.color2 : R.color.color11));

        if (!isGlobal) {
            Toast.makeText(this, "Bang xep hang ban be chua duoc backend ho tro", Toast.LENGTH_SHORT).show();
            // Keep UX stable on unsupported scope.
            tabGlobal.performClick();
        }
    }

    private void loadRankingBoard() {
        repository.getRankBoard(selectedApiType, new RepositoryCallback<>() {
            @Override
            public void onSuccess(List<RankBoardItemModel> data) {
                List<RankBoardItemModel> list = data == null ? new ArrayList<>() : data;
                adapter.submitList(list);
                bindTopThree(list);
            }

            @Override
            public void onError(String message) {
                Toast.makeText(RankingActivity.this, message, Toast.LENGTH_SHORT).show();
                adapter.submitList(new ArrayList<>());
                bindTopThree(new ArrayList<>());
            }
        });
    }

    private void bindTopThree(List<RankBoardItemModel> list) {
        bindTopRow(tvRank1Name, tvRank1Score, list, 0);
        bindTopRow(tvRank2Name, tvRank2Score, list, 1);
        bindTopRow(tvRank3Name, tvRank3Score, list, 2);
    }

    private void bindTopRow(TextView nameView, TextView scoreView, List<RankBoardItemModel> list, int index) {
        if (index < list.size()) {
            RankBoardItemModel item = list.get(index);
            nameView.setText(item.getDisplayName());
            scoreView.setText(item.getRankScore() + " point");
        } else {
            nameView.setText("-");
            scoreView.setText("0 point");
        }
    }

    private void loadCurrentUserRank() {
        if (currentUser != null && currentUser.getId() > 0) {
            requestCurrentUserRank(currentUser.getId());
            return;
        }

        int userId = sessionManager.getUserId();
        if (userId > 0) {
            requestCurrentUserRank(userId);
            return;
        }

        String token = sessionManager.getAccessToken();
        if (token == null || token.isEmpty()) {
            setCurrentUserPlaceholder();
            return;
        }

        repository.getHomeProfile(token, new RepositoryCallback<>() {
            @Override
            public void onSuccess(HomeProfileModel data) {
                sessionManager.saveUserId(data.getId());
                requestCurrentUserRank(data.getId());
            }

            @Override
            public void onError(String message) {
                setCurrentUserPlaceholder();
            }
        });
    }

    private void requestCurrentUserRank(int userId) {
        repository.getRankDetail(userId, new RepositoryCallback<>() {
            @Override
            public void onSuccess(RankDetailModel data) {
                tvCurrentUserRank.setText(String.valueOf(data.getRank()));
                tvCurrentUserName.setText(data.getDisplayName());
                tvCurrentUserScore.setText(data.getRankScore() + " point");
            }

            @Override
            public void onError(String message) {
                setCurrentUserPlaceholder();
            }
        });
    }

    private void setCurrentUserPlaceholder() {
        tvCurrentUserRank.setText("-");
        tvCurrentUserName.setText("Dang nhap de xem hang cua ban");
        tvCurrentUserScore.setText("0 point");
    }

    private void onRankItemClick(RankBoardItemModel item) {
        Toast.makeText(this, "UserId: " + item.getUserId(), Toast.LENGTH_SHORT).show();
    }

    private void onBackToGameClick() {
        Intent intent = new Intent(this, HomeActivity.class);
        if (currentUser != null) {
            intent.putExtra(HomeActivity.EXTRA_HOME_USER, currentUser);
        }
        startActivity(intent);
        finish();
    }
}