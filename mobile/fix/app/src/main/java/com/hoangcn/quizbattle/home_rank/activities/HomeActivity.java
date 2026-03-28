package com.hoangcn.quizbattle.home_rank.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.hoangcn.quizbattle.R;

public class HomeActivity extends AppCompatActivity {

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

        setupStats();
    }

    private void setupStats() {
        setStatItem(findViewById(R.id.include_item_score), R.drawable.ic_home_rank_point, "Điểm xếp hạng", "1560");
        setStatItem(findViewById(R.id.include_item_rank), R.drawable.ic_home_rank, "Thứ hạng", "#25");
        setStatItem(findViewById(R.id.include_item_matches), R.drawable.ic_home_number_match, "Số trận", "48");
        setStatItem(findViewById(R.id.include_item_win_rate), R.drawable.ic_home_win_rate, "Tỷ lệ thắng", "50.5%");
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