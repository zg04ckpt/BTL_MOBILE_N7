package com.hoangcn.quizbattle.battles.activities;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.battles.models.MatchStateResponse;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.List;

public class WaitResultActivity extends AppCompatActivity {
    private final Handler pollingHandler = new Handler();
    private final List<String> progressRows = new ArrayList<>();
    private ArrayAdapter<String> progressAdapter;
    private BattleService battleService;
    private TextView tvWaitingStatus;
    private boolean isActive = true;
    private boolean allPlayersFinished = false;
    private boolean pendingNavigateToResult = false;

    private final Runnable pollingRunnable = new Runnable() {
        @Override
        public void run() {
            if (!isActive) return;
            loadMatchState();
            pollingHandler.postDelayed(this, allPlayersFinished ? 1000 : 2000);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_result);

        battleService = new BattleService(this);
        tvWaitingStatus = findViewById(R.id.tv_waiting_status);
        ListView lvProgress = findViewById(R.id.lv_match_progress);
        Button btnHome = findViewById(R.id.btn_home);
        progressAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, progressRows);
        lvProgress.setAdapter(progressAdapter);
        btnHome.setOnClickListener(v -> finish());

        pollingHandler.post(pollingRunnable);
    }

    private void loadMatchState() {
        battleService.getMatchState(new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchStateResponse> data) {
                if (data == null || data.getData() == null) return;
                MatchStateResponse state = data.getData();

                runOnUiThread(() -> {
                    progressRows.clear();
                    int totalQuestions = Math.max(1, state.getTotalQuestions());
                    allPlayersFinished = !state.getUsers().isEmpty();
                    for (var u : state.getUsers()) {
                        if (!(u.isFinished() || u.getProgress() >= totalQuestions)) {
                            allPlayersFinished = false;
                            break;
                        }
                    }
                    state.getUsers().forEach(u -> progressRows.add(
                        "#" + u.getRank() + " " + u.getDisplayName()
                            + " | " + u.getProgress() + "/" + totalQuestions
                            + " | " + u.getScore() + " điểm"
                            + (u.isFinished() ? " | Hoàn thành" : "")
                    ));
                    if (allPlayersFinished) {
                        tvWaitingStatus.setText("Tất cả đã hoàn thành, đang tổng kết kết quả...");
                    } else {
                        tvWaitingStatus.setText("Đang cập nhật realtime: " + state.getUsers().size() + " người chơi");
                    }
                    progressAdapter.notifyDataSetChanged();

                    if (state.isEnded() && !pendingNavigateToResult) {
                        pendingNavigateToResult = true;
                        pollingHandler.postDelayed(() -> {
                            if (!isActive) return;
                            Intent intent = new Intent(WaitResultActivity.this, MatchResultActivity.class);
                            intent.putExtra("matchId", state.getMatchId());
                            startActivity(intent);
                            finish();
                        }, 700);
                    }
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> tvWaitingStatus.setText("Chưa thể tải trạng thái trận: " + message));
            }
        });
    }

    @Override
    protected void onDestroy() {
        isActive = false;
        pollingHandler.removeCallbacksAndMessages(null);
        super.onDestroy();
    }
}