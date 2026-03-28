package com.n7.quizbattle.events.presentation.activities;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.util.TypedValue;
import android.view.Gravity;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.google.android.material.button.MaterialButton;
import com.n7.quizbattle.R;
import com.n7.quizbattle.events.domain.model.LimitedEventModel;
import com.n7.quizbattle.events.domain.model.Reward;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.model.ThresholdModel;
import com.n7.quizbattle.events.presentation.viewmodel.LimitedEventViewModel;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.concurrent.TimeUnit;

public class LimitedEventDetailActivity extends AppCompatActivity {

    private LimitedEventModel eventModel;
    private LimitedEventViewModel viewModel;
    private TextView tvTimerDetail, tvQuestionCount, tvCheckpoint;
    private MaterialButton btnStartChallenge;
    private ImageButton btnBack;
    private LinearLayout containerRewards;
    private SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
    private int currentThresholdId = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_limited_event_detail);

        eventModel = (LimitedEventModel) getIntent().getSerializableExtra("EVENT_DATA");
        viewModel = new ViewModelProvider(this).get(LimitedEventViewModel.class);

        initViews();
        observeViewModel();
        initListeners();

        viewModel.loadRewardMapping();
    }

    private void initViews() {
        tvTimerDetail = findViewById(R.id.tv_timer_detail);
        tvQuestionCount = findViewById(R.id.tv_question_count);
        tvCheckpoint = findViewById(R.id.tv_checkpoint);
        containerRewards = findViewById(R.id.container_rewards);
        btnStartChallenge = findViewById(R.id.btn_start_challenge);
        btnBack = findViewById(R.id.btn_back);
    }

    private void initListeners() {
        btnStartChallenge.setOnClickListener(v -> {
            if (eventModel != null) {
                Intent intent = new Intent(this, LimitedEventStageActivity.class);
                intent.putExtra("EVENT_DATA", eventModel);
                intent.putExtra("CURRENT_STAGE", currentThresholdId);
                startActivity(intent);
            }
        });

        btnBack.setOnClickListener(v -> {
            // Quay lại trang HomeEventActivity
            finish();
        });
    }

    private void observeViewModel() {
        viewModel.rewardMapping.observe(this, mappings -> {
            displayData(mappings);
        });

        viewModel.errorMessage.observe(this, error -> {
            if (error != null) {
                Toast.makeText(this, error, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void displayData(List<RewardMappingModel> mappings) {
        if (eventModel == null) return;

        tvTimerDetail.setText(calculateRemainingTime(eventModel.getStartTime(), eventModel.getEndTime()));

        if (eventModel.getThresholds() != null && !eventModel.getThresholds().isEmpty()) {
            ThresholdModel threshold = eventModel.getThresholds().get(0);
            currentThresholdId = threshold.getThresholdId();

            tvCheckpoint.setText("Chặng số " + currentThresholdId);

            int qCount = threshold.getChallengeQuestionIds() != null ? threshold.getChallengeQuestionIds().size() : 0;
            tvQuestionCount.setText(qCount + " Câu hỏi");

            containerRewards.removeAllViews();
            if (threshold.getRewards() != null) {
                for (Reward reward : threshold.getRewards()) {
                    addRewardView(reward, mappings);
                }
            }
        }
    }

    private void addRewardView(Reward reward, List<RewardMappingModel> mappings) {
        RewardMappingModel mapping = findMapping(reward.getEventRewardId(), mappings);
        if (mapping == null) return;

        LinearLayout row = new LinearLayout(this);
        row.setOrientation(LinearLayout.HORIZONTAL);
        row.setGravity(Gravity.CENTER_VERTICAL);
        row.setPadding(0, 16, 0, 16);

        ImageView icon = new ImageView(this);
        LinearLayout.LayoutParams iconParams = new LinearLayout.LayoutParams(
                (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 48, getResources().getDisplayMetrics()),
                (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 48, getResources().getDisplayMetrics())
        );
        icon.setLayoutParams(iconParams);

        String name = mapping.getName().toLowerCase();
        if (name.contains("vàng")) {
            icon.setImageResource(R.drawable.gold_prize);
        } else if (name.contains("bảo vệ")) {
            icon.setImageResource(R.drawable.defend_rank_prize);
        } else if (name.contains("kinh nghiệm") || name.contains("exp")) {
            icon.setImageResource(R.drawable.exprience_prize);
        } else {
            icon.setImageResource(R.drawable.ic_bell);
        }

        TextView text = new TextView(this);
        String displayText = reward.getValue() + " " + mapping.getName();
        text.setText(displayText);
        text.setTextSize(20);
        text.setTextColor(Color.parseColor("#FF7043"));
        text.setPadding(24, 0, 0, 0);
        text.setTypeface(null, android.graphics.Typeface.BOLD);

        row.addView(icon);
        row.addView(text);
        containerRewards.addView(row);
    }

    private RewardMappingModel findMapping(int rewardId, List<RewardMappingModel> mappings) {
        if (mappings == null) return null;
        for (RewardMappingModel m : mappings) {
            if (m.getId() == rewardId) return m;
        }
        return null;
    }

    private String calculateRemainingTime(String startTimeStr, String endTimeStr) {
        try {
            String cleanStartTime = startTimeStr != null ? startTimeStr.replace("T", " ").substring(0, 19) : null;
            String cleanEndTime = endTimeStr != null ? endTimeStr.replace("T", " ").substring(0, 19) : null;

            Date endDate = (cleanEndTime == null || cleanEndTime.isEmpty()) ? new Date() : sdf.parse(cleanEndTime);
            Date startDate = (cleanStartTime == null || cleanStartTime.isEmpty()) ? new Date() : sdf.parse(cleanStartTime);

            long diffInMs = endDate.getTime() - startDate.getTime();
            if (diffInMs < 0) return "Đã kết thúc";

            long hours = TimeUnit.MILLISECONDS.toHours(diffInMs);
            long minutes = TimeUnit.MILLISECONDS.toMinutes(diffInMs) % 60;
            long seconds = TimeUnit.MILLISECONDS.toSeconds(diffInMs) % 60;

            return String.format(Locale.getDefault(), "%02dh : %02dp : %02ds", hours, minutes, seconds);
        } catch (Exception e) {
            return "18h : 26p : 19s";
        }
    }
}
