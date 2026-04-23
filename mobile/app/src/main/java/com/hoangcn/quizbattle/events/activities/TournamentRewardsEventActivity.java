package com.hoangcn.quizbattle.events.activities;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.adapters.TournamentTaskAdapter;
import com.hoangcn.quizbattle.events.api.EventService;
import com.hoangcn.quizbattle.events.models.ClaimRewardRequest;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskProgressModel;
import com.hoangcn.quizbattle.events.utils.RewardUtil;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Executors;

public class TournamentRewardsEventActivity extends AppCompatActivity {
    public static final String KEY_EVENT = "tournament_rewards_event_data";

    private EventService eventService;
    private EventModel event;
    private List<TaskProgressModel> progresses = new ArrayList<>();
    private Map<Integer, RewardModel> rewards = new HashMap<>();
    private TournamentTaskAdapter adapter;
    private MediaPlayer rewardMusic;

    private RecyclerView rvTasks;
    private Button btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_tournament_rewards_event);

        eventService = new EventService(this);
        initViews();
        setListeners();
        initData();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (rewardMusic != null) {
            rewardMusic.release();
            rewardMusic = null;
        }
    }

    private void initViews() {
        rvTasks = findViewById(R.id.rvTasks);
        btnBack = findViewById(R.id.btnBack);
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
            eventService.getProgress(event.getId(), new ApiCallback<EventProgressModel>() {
                @Override
                public void onSuccess(ApiResponse<EventProgressModel> data) {
                    progresses = data.getData().getTaskProgresses();
                    getRewardInfos();
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> Toast.makeText(TournamentRewardsEventActivity.this, "Tải tiến độ thất bại", Toast.LENGTH_SHORT).show());
                }
            });
        });
    }

    private void getRewardInfos() {
        eventService.getRewardMapping(new ApiCallback<List<RewardModel>>() {
            @Override
            public void onSuccess(ApiResponse<List<RewardModel>> data) {
                data.getData().forEach(r -> rewards.put(r.getId(), r));
                runOnUiThread(() -> renderListTasks());
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> Toast.makeText(TournamentRewardsEventActivity.this, "Tải dữ liệu thưởng thất bại", Toast.LENGTH_SHORT).show());
            }
        });
    }

    private void renderListTasks() {
        adapter = new TournamentTaskAdapter(event.getTasks(), progresses, rewards);
        adapter.setOnTaskClickListener((task, progress) -> claimReward(task.getTaskId()));
        rvTasks.setAdapter(adapter);
    }

    private void claimReward(int taskId) {
        var request = new ClaimRewardRequest(event.getId(), 0, taskId);
        eventService.claimReward(request, new ApiCallback<EventProgressModel>() {
            @Override
            public void onSuccess(ApiResponse<EventProgressModel> data) {
                var task = event.getTasks().stream().filter(t -> t.getTaskId() == taskId).findFirst().orElse(null);
                progresses.clear();
                progresses.addAll(data.getData().getTaskProgresses());
                runOnUiThread(() -> {
                    if (task != null && task.getRewards() != null && !task.getRewards().isEmpty()) {
                        var firstReward = task.getRewards().get(0);
                        showRewardDialog(firstReward.getEventRewardId(), firstReward.getValue());
                    }
                    adapter.notifyDataSetChanged();
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> Toast.makeText(TournamentRewardsEventActivity.this, "Nhận thưởng thất bại: " + message, Toast.LENGTH_SHORT).show());
            }
        });
    }

    private void showRewardDialog(int rewardId, int value) {
        if (rewardMusic == null) {
            rewardMusic = MediaPlayer.create(this, R.raw.reward_event);
            rewardMusic.setLooping(false); // Chỉ chạy 1 lần
        }
        if (rewardMusic.isPlaying()) {
            rewardMusic.pause();
            rewardMusic.seekTo(0);
        }
        rewardMusic.start();

        RewardModel reward = rewards.get(rewardId);
        View dialogView = LayoutInflater.from(this).inflate(R.layout.dialog_reward, null);
        TextView tvRewardName = dialogView.findViewById(R.id.tvRewardName);
        TextView tvRewardValue = dialogView.findViewById(R.id.tvRewardValue);
        ImageView ivRewardIcon = dialogView.findViewById(R.id.ivRewardIcon);
        Button btnConfirm = dialogView.findViewById(R.id.btnConfirm);

        tvRewardName.setText(reward.getName());
        tvRewardValue.setText("+ " + value);
        ivRewardIcon.setImageResource(RewardUtil.getRewardIcon(reward));

        AlertDialog dialog = new AlertDialog.Builder(this, R.style.TransparentDialog)
                .setView(dialogView)
                .setCancelable(false)
                .create();

        btnConfirm.setOnClickListener(v -> {
            dialog.dismiss();
            if (rewardMusic != null && rewardMusic.isPlaying()) {
                rewardMusic.pause();
                rewardMusic.seekTo(0);
            }
        });
        dialog.show();
    }
}