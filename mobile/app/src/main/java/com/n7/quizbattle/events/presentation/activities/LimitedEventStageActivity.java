package com.n7.quizbattle.events.presentation.activities;

import android.graphics.Color;
import android.graphics.Typeface;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.Gravity;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.n7.quizbattle.R;
import com.n7.quizbattle.events.domain.model.LimitedEventModel;
import com.n7.quizbattle.events.domain.model.Reward;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.model.ThresholdModel;
import com.n7.quizbattle.events.presentation.viewmodel.LimitedEventViewModel;

import java.util.List;

public class LimitedEventStageActivity extends AppCompatActivity {

    private LimitedEventModel eventModel;
    private LimitedEventViewModel viewModel;
    private TextView tvStageNumber;
    private LinearLayout containerPrevRewards, containerCurrentRewards;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_limited_event_stage);

        eventModel = (LimitedEventModel) getIntent().getSerializableExtra("EVENT_DATA");
        viewModel = new ViewModelProvider(this).get(LimitedEventViewModel.class);

        initViews();
        observeViewModel();
        
        viewModel.loadRewardMapping();
    }

    private void initViews() {
        tvStageNumber = findViewById(R.id.tv_stage_number);
        containerPrevRewards = findViewById(R.id.container_prev_rewards);
        containerCurrentRewards = findViewById(R.id.container_current_rewards);
        
        findViewById(R.id.btn_end).setOnClickListener(v -> finish());
        findViewById(R.id.btn_continue).setOnClickListener(v -> {
            // Logic to move to next stage or start quiz
            Toast.makeText(this, "Tiếp tục chặng đua!", Toast.LENGTH_SHORT).show();
        });
    }

    private void observeViewModel() {
        viewModel.rewardMapping.observe(this, mappings -> {
            displayData(mappings);
        });
    }

    private void displayData(List<RewardMappingModel> mappings) {
        if (eventModel == null || eventModel.getThresholds() == null || eventModel.getThresholds().size() < 2) {
            // Demo logic: If only 1 threshold, assume we are at stage 2 for display
            tvStageNumber.setText("Chặng số 2");
            return;
        }

        // Theo yêu cầu: chặng sẽ là do dữ liệu từ chặng hiện tại + 1
        // Giả sử  đang hiển thị chặng 2 (index 1)
        ThresholdModel currentThreshold = eventModel.getThresholds().get(1);
        ThresholdModel prevThreshold = eventModel.getThresholds().get(0);

        tvStageNumber.setText("Chặng số " + currentThreshold.getThresholdId());

        // Phần thưởng chặng trước (Icon + Text) - Trước nút END
        displayRewards(prevThreshold.getRewards(), containerPrevRewards, mappings, true);

        // Phần thưởng chặng hiện tại (Text only) - Trước nút CONTINUE
        displayRewards(currentThreshold.getRewards(), containerCurrentRewards, mappings, false);
    }

    private void displayRewards(List<Reward> rewards, LinearLayout container, List<RewardMappingModel> mappings, boolean showIcon) {
        container.removeAllViews();
        if (rewards == null) return;

        for (Reward reward : rewards) {
            RewardMappingModel mapping = findMapping(reward.getEventRewardId(), mappings);
            if (mapping == null) continue;

            if (showIcon) {
                LinearLayout row = new LinearLayout(this);
                row.setOrientation(LinearLayout.HORIZONTAL);
                row.setGravity(Gravity.CENTER);
                row.setPadding(0, 8, 0, 8);

                ImageView icon = new ImageView(this);
                icon.setLayoutParams(new LinearLayout.LayoutParams(dpToPx(48), dpToPx(48)));
                icon.setImageResource(getIconRes(mapping.getName()));

                TextView text = new TextView(this);
                text.setText(reward.getValue() + " " + mapping.getName());
                text.setTextSize(20);
                text.setTextColor(Color.parseColor("#FF7043"));
                text.setPadding(16, 0, 0, 0);
                text.setTypeface(null, Typeface.BOLD);

                row.addView(icon);
                row.addView(text);
                container.addView(row);
            } else {
                TextView text = new TextView(this);
                text.setText(reward.getValue() + " " + mapping.getName());
                text.setTextSize(20);
                text.setTextColor(Color.parseColor("#FF7043"));
                text.setGravity(Gravity.CENTER);
                text.setPadding(0, 16, 0, 16);
                text.setTypeface(null, Typeface.BOLD);
                container.addView(text);
            }
        }
    }

    private int getIconRes(String name) {
        name = name.toLowerCase();
        if (name.contains("vàng")) return R.drawable.gold_prize;
        if (name.contains("bảo vệ")) return R.drawable.defend_rank_prize;
        if (name.contains("exp") || name.contains("kinh nghiệm")) return R.drawable.exprience_prize;
        return R.drawable.ic_bell;
    }

    private RewardMappingModel findMapping(int id, List<RewardMappingModel> mappings) {
        if (mappings == null) return null;
        for (RewardMappingModel m : mappings) if (m.getId() == id) return m;
        return null;
    }

    private int dpToPx(int dp) {
        return (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, dp, getResources().getDisplayMetrics());
    }
}
