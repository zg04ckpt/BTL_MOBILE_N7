package com.n7.quizbattle.users.activities;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.n7.quizbattle.R;
import com.n7.quizbattle.battles.adapters.MatchHistoryAdapter;
import com.n7.quizbattle.battles.models.MatchHistory;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.users.api.UsersApi;
import com.n7.quizbattle.users.auth.UsersErrorMapper;
import com.n7.quizbattle.users.auth.UsersTokenManager;
import com.n7.quizbattle.users.models.UserModel;

import java.io.InputStream;
import java.lang.ref.WeakReference;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

public class ProfileActivity extends AppCompatActivity {

    private ImageView ivAvatar;
    private TextView tvUsername;
    private TextView tvLevelLabel;
    private TextView tvExp;
    private Button btnLogout;

    private UserModel currentUser;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_profile);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        setupViews();

        // Show neutral placeholders until API returns
        setupStats("-", "-", "-", "-");

        setupMatchHistory();

        findViewById(R.id.btn_edit_profile).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                openEditProfileDialog();
            }
        });

        btnLogout.setOnClickListener(v -> {
            new UsersTokenManager(ProfileActivity.this).clear();
            Toast.makeText(ProfileActivity.this, "Đã đăng xuất", Toast.LENGTH_SHORT).show();
            // Optionally go back to login
            Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
            finish();
        });

        // Load profile data
        loadProfile();
    }

    private void setupViews() {
        ivAvatar = findViewById(R.id.iv_avatar);
        tvUsername = findViewById(R.id.tv_username);
        tvLevelLabel = findViewById(R.id.tv_level_label);
        tvExp = findViewById(R.id.tv_exp);
        btnLogout = findViewById(R.id.btn_logout);
    }

    private void openEditProfileDialog() {
        EditProfileDialogFragment dialog = new EditProfileDialogFragment();
        dialog.setInitialUser(currentUser);
        dialog.setUpdateListener(updatedUser -> {
            if (updatedUser != null) {
                currentUser = updatedUser;
                bindProfile(updatedUser);
            }
            // Fetch again to ensure we have the latest data (including avatar URL hosted path)
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

    private void setupMatchHistory() {
        RecyclerView rvHistory = findViewById(R.id.rv_history);

        List<MatchHistory> data = new ArrayList<>();
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn B", "2 phút trước", "25", true));
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn C", "Hôm qua", "15", false));
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn D", "2 phút trước", "25", true));
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn E", "2 phút trước", "25", true));
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn F", "2 phút trước", "25", true));
        data.add(new MatchHistory(R.drawable.opponent_avatar, "Nguyễn Văn G", "2 phút trước", "15", false));

        MatchHistoryAdapter adapter = new MatchHistoryAdapter(data);
        rvHistory.setLayoutManager(new LinearLayoutManager(this));
        rvHistory.setAdapter(adapter);
    }

    private void loadProfile() {
        UsersApi api = new UsersApi(this);
        api.getProfile(new ApiCallback<UserModel>() {
            @Override
            public void onSuccess(ApiResponse<UserModel> data) {
                if (data != null && data.getData() != null) {
                    UserModel u = data.getData();
                    currentUser = u;
                    runOnUiThread(() -> bindProfile(u));
                }
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> Toast.makeText(ProfileActivity.this, UsersErrorMapper.map(message), Toast.LENGTH_LONG).show());
            }
        });
    }

    private void bindProfile(UserModel u) {
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

        String avatarUrl = resolveAvatarUrl(u.getAvatar());
        if (avatarUrl != null) {
            new ImageLoadTask(avatarUrl, ivAvatar).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
        } else {
            ivAvatar.setImageResource(R.drawable.opponent_avatar);
        }
    }

    private String resolveAvatarUrl(String raw) {
        if (raw == null || raw.trim().isEmpty()) return null;
        if (raw.startsWith("http")) return raw;
        // Backend returns relative path like /uploads/xxx
        return "https://quizbattle.hoangcn.com" + raw;
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

    // Very small image loader fallback (not production-grade). Replace with Glide/Picasso if available.
    private static class ImageLoadTask extends AsyncTask<Void, Void, Bitmap> {
        private final String url;
        private final WeakReference<ImageView> ivRef;

        ImageLoadTask(String url, ImageView iv) {
            this.url = url;
            this.ivRef = new WeakReference<>(iv);
        }

        @Override
        protected Bitmap doInBackground(Void... voids) {
            try {
                URL u = new URL(url);
                HttpURLConnection conn = (HttpURLConnection) u.openConnection();
                conn.setConnectTimeout(5000);
                conn.setReadTimeout(5000);
                conn.setInstanceFollowRedirects(true);
                InputStream is = conn.getInputStream();
                Bitmap b = BitmapFactory.decodeStream(is);
                is.close();
                return b;
            } catch (Exception e) {
                return null;
            }
        }

        @Override
        protected void onPostExecute(Bitmap bitmap) {
            ImageView iv = ivRef.get();
            if (bitmap != null && iv != null) {
                iv.setImageBitmap(bitmap);
            }
        }
    }
}
