package com.n7.quizbattle.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.n7.quizbattle.R;
import com.n7.quizbattle.adapters.MatchHistoryAdapter;
import com.n7.quizbattle.models.MatchHistory;

import java.util.ArrayList;
import java.util.List;

public class ProfileActivity extends AppCompatActivity {

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

        setupStats();

        setupMatchHistory();

        Button btnEditProfile = findViewById(R.id.btn_edit_profile);

        btnEditProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                openEditProfileDialog();
            }
        });
    }

    private void openEditProfileDialog() {
        EditProfileDialogFragment dialog = new EditProfileDialogFragment();

        dialog.show(getSupportFragmentManager(), "EditProfile");
    }

    private void setupStats() {
        setStatItem(findViewById(R.id.stat_rank), R.drawable.ic_profile_rank_point, "1580", "Điểm xếp hạng");
        setStatItem(findViewById(R.id.stat_total_matches), R.drawable.ic_profile_clock, "20", "Tổng số trận");
        setStatItem(findViewById(R.id.stat_win_rate), R.drawable.ic_profile_percent, "70.2", "Tỉ lệ thắng");
        setStatItem(findViewById(R.id.stat_streak), R.drawable.ic_profile_fire, "3", "Chuỗi thắng");
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
}