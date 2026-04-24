package com.hoangcn.quizbattle.users.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.adapters.MatchHistoryAdapter;
import com.hoangcn.quizbattle.battles.models.MatchHistory;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;
import com.hoangcn.quizbattle.users.api.UserService;
import com.hoangcn.quizbattle.users.models.UserProfile;

import java.util.ArrayList;
import java.util.List;

public class ProfileActivity extends AppCompatActivity {

    private ImageView ivBack;
    private ImageView ivAvatar;
    private TextView tvUsername;
    private TextView tvLevelLabel;
    private TextView tvExp;
    private Button btnLogout;
    private Button btnEditProfile;

    private UserProfile currentUser;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        initViews();
        setListeners();
        initData();
    }

    private void initViews() {
        ivBack = findViewById(R.id.iv_back);
        ivAvatar = findViewById(R.id.iv_avatar);
        tvUsername = findViewById(R.id.tv_username);
        tvLevelLabel = findViewById(R.id.tv_level_label);
        tvExp = findViewById(R.id.tv_exp);
        btnLogout = findViewById(R.id.btn_logout);
        btnEditProfile = findViewById(R.id.btn_edit_profile);

        setupStats("-", "-", "-", "-");
    }

    private void setListeners() {
        ivBack.setOnClickListener(v -> navigateHome());
        btnEditProfile.setOnClickListener(l -> openEditProfileDialog());
        btnLogout.setOnClickListener(l -> logout());
    }

    private void navigateHome() {
        finish();
    }

    private void logout() {
        var api = new UserService(this);
        api.logout(new ApiCallback<Void>() {
            @Override
            public void onSuccess(ApiResponse<Void> data) {
                Toast.makeText(ProfileActivity.this, "Đăng xuất thành công", Toast.LENGTH_SHORT).show();
                SharedPreferenceUtil.getInstance(ProfileActivity.this).remove("userId");
                SharedPreferenceUtil.getInstance(ProfileActivity.this).remove("accessToken");
                Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
                startActivity(intent);
                finish();
            }

            @Override
            public void onError(String message) {
                Toast.makeText(ProfileActivity.this, "Đăng xuất thất bại: " + message, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void initData() {
        loadProfile();
    }

    private void openEditProfileDialog() {
        EditProfileDialogFragment dialog = new EditProfileDialogFragment();
        dialog.setInitialUser(currentUser);
        dialog.setUpdateListener(updatedUser -> {
            if (updatedUser != null) {
                currentUser = updatedUser;
                bindProfile(updatedUser);
            }
            loadProfile();
        });

        dialog.show(getSupportFragmentManager(), "EditProfile");
    }

    private void setupStats(String rank, String matches, String winRate, String streak) {
        setStatItem(findViewById(R.id.stat_rank), R.drawable.ic_profile_rank_point, rank, "Điểm xếp hạng");
        setStatItem(findViewById(R.id.stat_total_matches), R.drawable.ic_profile_clock, matches, "Tổng số trận");
        setStatItem(findViewById(R.id.stat_win_rate), R.drawable.ic_profile_percent, winRate, "Tỉ lệ thắng");
        setStatItem(findViewById(R.id.stat_streak), R.drawable.ic_profile_fire, streak, "Chuỗi thắng");
    }

    private void setStatItem(View includeView, int iconRes, String value, String label) {
        ImageView ivIcon = includeView.findViewById(R.id.iv_stat_icon);
        TextView tvValue = includeView.findViewById(R.id.tv_stat_value);
        TextView tvLabel = includeView.findViewById(R.id.tv_stat_label);

        ivIcon.setImageResource(iconRes);
        tvValue.setText(value);
        tvLabel.setText(label);
    }

    private void loadProfile() {
        var api = new UserService(this);
        api.getProfile(new ApiCallback<UserProfile>() {
            @Override
            public void onSuccess(ApiResponse<UserProfile> data) {
                if (data != null && data.getData() != null) {
                    UserProfile u = data.getData();
                    currentUser = u;
                    currentUser.setAvatar(api.getFullImageUrl(currentUser.getAvatar()));
                    runOnUiThread(() -> bindProfile(u));
                }
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    Toast.makeText(ProfileActivity.this, "Lấy thông tin thất bại: " + message, Toast.LENGTH_LONG).show();
                });
            }
        });
    }

    private void bindProfile(UserProfile u) {
        currentUser = u;
        // Choose best available name: displayName/name (already mapped) -> email -> placeholder
        String name = firstNonEmpty(u.getDisplayName(), u.getEmail(), "Người chơi");
        tvUsername.setText(name);

        Integer lvl = u.getLevel();
        tvLevelLabel.setText("Lv: " + (lvl == null ? 0 : lvl));

        Integer expVal = u.getExp();
        tvExp.setText((expVal == null ? 0 : expVal) + " EXP");

        String rank = u.getRankScore() != null ? String.valueOf(u.getRankScore()) : "0";
        String matches = u.getNumberOfMatches() != null ? String.valueOf(u.getNumberOfMatches()) : "0";
        String winRate = u.getWinningRate() != null ? String.format("%.1f", u.getWinningRate()) : "0";
        String streak = u.getWinningStreak() != null ? String.valueOf(u.getWinningStreak()) : "0";
        setupStats(rank, matches, winRate, streak);

        Glide.with(this).load(u.getAvatar())
                .centerCrop()
                .circleCrop()
                .into(ivAvatar);
    }

    private String firstNonEmpty(String... values) {
        if (values == null) return "";
        for (String v : values) {
            if (v != null && !v.trim().isEmpty()) {
                return v.trim();
            }
        }
        return "";
    }
}
