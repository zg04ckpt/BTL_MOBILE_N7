package com.hoangcn.quizbattle.events.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.adapters.QuizMilestoneEventThresholdAdapter;
import com.hoangcn.quizbattle.events.api.EventService;
import com.hoangcn.quizbattle.events.models.ClaimRewardRequest;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.quizmilestone.ThresholdProgressModel;
import com.hoangcn.quizbattle.events.utils.ConvertUtil;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.concurrent.Executors;

public class QuizMilestoneChallengeActivity extends AppCompatActivity {
    public static final String KEY_EVENT = "quiz_milestone_event_data";

    private HashMap<Integer, RewardModel> rewards  = new HashMap<>();
    private EventService eventService;
    private List<ThresholdProgressModel> progresses = new ArrayList<>();
    private EventModel event;
    private QuizMilestoneEventThresholdAdapter adapter;

    TextView tvProgress;
    Button btnBack;
    RecyclerView rvThresholds;
    View vProgressBarBg;
    View vProgressBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_quiz_milestone_challenge);

        eventService = new EventService(this);

        initViews();
        setListeners();
        initData();
    }

    private void initViews() {
        tvProgress = findViewById(R.id.tvProgress);
        rvThresholds = findViewById(R.id.rvThresholds);
        btnBack = findViewById(R.id.btnBack);
        vProgressBarBg = findViewById(R.id.vProgressBarBg);
        vProgressBar = findViewById(R.id.vProgressBar);
    }

    private void setListeners() {
        btnBack.setOnClickListener(v -> finish());
    }

    private void initData() {
        event = (EventModel) getIntent().getSerializableExtra(KEY_EVENT);
        loadProgress();
    }

    private void loadProgress() {
        Executors.newSingleThreadExecutor().execute(() -> {
            eventService.getProgress(event.getId(), new ApiCallback<>() {
                @Override
                public void onSuccess(ApiResponse<EventProgressModel> data) {
                    progresses = data.getData().getThresholdProgresses();
                    runOnUiThread(() -> renderProgressInfo());
                    getRewardInfos();
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> {
                        Toast.makeText(
                                QuizMilestoneChallengeActivity.this,
                                "Tải dữ liệu tiến độ thất bại: " + message,
                                Toast.LENGTH_SHORT).show();
                    });
                }
            });
        });
    }

    private void renderProgressInfo() {
        int completed = (int) progresses.stream().filter(ThresholdProgressModel::isCompleted).count();
        tvProgress.setText("Tiến độ " + completed + "/" + event.getThresholds().size());

        // Cập nhật chiều cao của ProgressBarBg dựa trên số ngưỡng
        vProgressBarBg.getLayoutParams().height = ConvertUtil.dpToPx(this, progresses.size() * 100);

        // Cập nhật chiều cao của ProgressBar dựa trên số ngưỡng đã hoàn thành
        if (completed == progresses.size()) {
            vProgressBar.getLayoutParams().height = ConvertUtil.dpToPx(this, progresses.size() * 100);
        } else {
            vProgressBar.getLayoutParams().height = ConvertUtil.dpToPx(this, (completed + 1) * 100 - 50);
        }
    }

    private void renderListThresholds() {
        adapter = new QuizMilestoneEventThresholdAdapter(
                event.getThresholds(), progresses, rewards);
        adapter.setOnItemClickedListener(progress -> {
            if (progress.isCompleted() && !progress.isRewardClaimed()) {
                claimReward(progress.getThresholdId());
            } else {
                Toast.makeText(
                        QuizMilestoneChallengeActivity.this,
                        "Chức năng chưa phát triển",
                        Toast.LENGTH_SHORT
                ).show();
            }
        });
        rvThresholds.setAdapter(adapter);
    }

    private void claimReward(int thresholdId) {
        var request = new ClaimRewardRequest(
                event.getId(),
                thresholdId,
                null
        );
        eventService.claimReward(request, new ApiCallback<>(){
            @Override
            public void onSuccess(ApiResponse<EventProgressModel> data) {
                progresses.clear();
                progresses.addAll(data.getData().getThresholdProgresses());
                runOnUiThread(() -> {
                    Toast.makeText(
                            QuizMilestoneChallengeActivity.this,
                            "Nhận phần thưởng thành công",
                            Toast.LENGTH_SHORT
                    ).show();
                    adapter.notifyDataSetChanged();
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    Toast.makeText(
                            QuizMilestoneChallengeActivity.this,
                            "Nhận phần thưởng thất bại: " + message,
                            Toast.LENGTH_SHORT
                    ).show();
                });
            }
        });
    }

    private void getRewardInfos() {
        eventService.getRewardMapping(new ApiCallback<List<RewardModel>>() {
            @Override
            public void onSuccess(ApiResponse<List<RewardModel>> data) {
                data.getData().forEach(e -> {
                    rewards.put(e.getId(), e);
                });
                renderListThresholds();
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    Toast.makeText(
                            QuizMilestoneChallengeActivity.this,
                            "Tải dữ liệu phần thưởng thất bại: " + message,
                            Toast.LENGTH_SHORT
                    );
                });
            }
        });
    }
}