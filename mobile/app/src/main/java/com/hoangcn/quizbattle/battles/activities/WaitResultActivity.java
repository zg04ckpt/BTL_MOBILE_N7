package com.hoangcn.quizbattle.battles.activities;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.adapters.MatchProgressAdapter;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.battles.models.MatchProgressUserItem;
import com.hoangcn.quizbattle.battles.models.MatchStateResponse;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;

import java.util.ArrayList;
import java.util.List;

public class WaitResultActivity extends AppCompatActivity {
    private final Handler pollingHandler = new Handler();
    private final List<MatchProgressUserItem> progressItems = new ArrayList<>();
    private MatchProgressAdapter progressAdapter;
    private BattleService battleService;
    private TextView tvWaitingStatus;
    private TextView tvScore;
    private TextView tvCorrect;
    private boolean isActive = true;
    private boolean allPlayersFinished = false;
    private boolean pendingNavigateToResult = false;
    private int totalQuestions = 10;
    private int myUserId = -1;

    private final Runnable pollingRunnable = new Runnable() {
        @Override
        public void run() {
            if (!isActive) return;
            // Poll trạng thái trận để cập nhật tiến độ realtime đơn giản, ổn định.
            loadMatchState();
            pollingHandler.postDelayed(this, allPlayersFinished ? 1000 : 2000);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_wait_match_result);

        battleService = new BattleService(this);
        myUserId = SharedPreferenceUtil.getInstance(this).getInt("userId", -1);
        tvWaitingStatus = findViewById(R.id.tv_waiting_status);
        tvScore = findViewById(R.id.tv_score);
        tvCorrect = findViewById(R.id.tv_correct);
        ListView lvProgress = findViewById(R.id.lv_match_progress);
        Button btnHome = findViewById(R.id.btn_home);

        progressAdapter = new MatchProgressAdapter(this, progressItems, totalQuestions);
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
                    progressItems.clear();
                    totalQuestions = Math.max(1, state.getTotalQuestions());
                    allPlayersFinished = !state.getUsers().isEmpty();

                    for (var u : state.getUsers()) {
                        if (!(u.isFinished() || u.getProgress() >= totalQuestions)) {
                            allPlayersFinished = false;
                        }
                        progressItems.add(u);
                        if (u.getUserId() == myUserId) {
                            bindMyScoreSummary(u, totalQuestions);
                        }
                    }

                    if (allPlayersFinished) {
                        tvWaitingStatus.setText("Tất cả đã hoàn thành, đang tổng kết kết quả...");
                    } else {
                        tvWaitingStatus.setText("Đang cập nhật realtime: " + state.getUsers().size() + " người chơi");
                    }

                    ListView lvProgress = findViewById(R.id.lv_match_progress);
                    if (lvProgress.getAdapter() == null || totalQuestions != state.getTotalQuestions()) {
                        progressAdapter = new MatchProgressAdapter(WaitResultActivity.this, progressItems, totalQuestions);
                        lvProgress.setAdapter(progressAdapter);
                    } else {
                        progressAdapter.notifyDataSetChanged();
                    }

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

    private void bindMyScoreSummary(MatchProgressUserItem me, int totalQuestions) {
        if (me == null) return;
        int safeTotal = Math.max(0, totalQuestions);
        int safeCorrect = Math.max(0, Math.min(me.getCorrect(), safeTotal));
        // Sai = số câu đã làm - số câu đúng.
        int wrong = Math.max(0, me.getProgress() - safeCorrect);
        tvScore.setText(String.valueOf(Math.max(0, me.getScore())));
        tvCorrect.setText(safeCorrect + " đúng • " + wrong + " sai");
    }

    @Override
    protected void onDestroy() {
        isActive = false;
        pollingHandler.removeCallbacksAndMessages(null);
        super.onDestroy();
    }
}