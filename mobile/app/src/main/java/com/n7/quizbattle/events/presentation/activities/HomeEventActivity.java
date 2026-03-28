package com.n7.quizbattle.events.presentation.activities;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.google.android.material.button.MaterialButton;
import com.n7.quizbattle.R;
import com.n7.quizbattle.events.presentation.state.UiState;
import com.n7.quizbattle.events.presentation.viewmodel.HomeEventModel;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.concurrent.TimeUnit;

public class HomeEventActivity extends AppCompatActivity {

    private HomeEventModel viewModel;
    private TextView tvTimerLimited, tvTimerDaily, tvTimerSeasonal;
    private MaterialButton btnJoinLimited, btnJoinDaily, btnJoinSeasonal;
    private SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_event);

        initViews();
        viewModel = new ViewModelProvider(this).get(HomeEventModel.class);

        updateButtonsState(false);
        observeViewModel();
        initListeners();
        
        viewModel.loadLuckySpinEvent();
    }

    private void initViews() {
        tvTimerLimited = findViewById(R.id.tv_timer_limited);
        tvTimerDaily = findViewById(R.id.tv_timer_daily);
        tvTimerSeasonal = findViewById(R.id.tv_timer_seasonal);

        btnJoinLimited = findViewById(R.id.btn_join_limited);
        btnJoinDaily = findViewById(R.id.btn_join_daily);
        btnJoinSeasonal = findViewById(R.id.btn_join_seasonal);
    }

    private void initListeners() {
        btnJoinLimited.setOnClickListener(v -> {
            if (viewModel.getLimitedEvent() != null) {
                Intent intent = new Intent(this, LimitedEventDetailActivity.class);
                intent.putExtra("EVENT_DATA", viewModel.getLimitedEvent());
                startActivity(intent);
            }
        });

        btnJoinSeasonal.setOnClickListener(v -> {
            if (viewModel.getSeasonalEvent() != null) {
                Intent intent = new Intent(this, SeasonalEventDetailActivity.class);
                intent.putExtra("SEASONAL_EVENT_DATA", viewModel.getSeasonalEvent());
                startActivity(intent);
            }
        });
        
        btnJoinDaily.setOnClickListener(v -> {
            // Chuyển sang WheelActivity hoặc DailyEventActivity tùy yêu cầu
            Intent intent = new Intent(this, WheelActivity.class);
            startActivity(intent);
        });
    }

    private void observeViewModel() {
        viewModel.limitedEventData.observe(this, event -> {
            if (event != null) {
                tvTimerLimited.setText(calculateRemainingTime(event.getStartTime(), event.getEndTime()));
            }
        });

        viewModel.dailyEventData.observe(this, event -> {
            if (event != null) {
                tvTimerDaily.setText(calculateRemainingTime(event.getStartTime(), event.getEndTime()));
            }
        });

        viewModel.seasonalEventData.observe(this, event -> {
            if (event != null) {
                tvTimerSeasonal.setText(calculateRemainingTime(event.getStartTime(), event.getEndTime()));
            }
        });

        viewModel.isDataReady.observe(this, state -> {
            if (state.status == UiState.Status.SUCCESS) {
                updateButtonsState(true);
            } else if (state.status == UiState.Status.ERROR) {
                updateButtonsState(false);
                Toast.makeText(this, state.message, Toast.LENGTH_SHORT).show();
            } else {
                updateButtonsState(false);
            }
        });
    }

    private void updateButtonsState(boolean isEnabled) {
        float alpha = isEnabled ? 1.0f : 0.5f;
        btnJoinLimited.setEnabled(isEnabled);
        btnJoinLimited.setAlpha(alpha);
        btnJoinDaily.setEnabled(isEnabled);
        btnJoinDaily.setAlpha(alpha);
        btnJoinSeasonal.setEnabled(isEnabled);
        btnJoinSeasonal.setAlpha(alpha);
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
            return "00h : 00p : 00s";
        }
    }
}
