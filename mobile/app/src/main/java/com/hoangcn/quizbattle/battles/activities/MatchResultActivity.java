package com.hoangcn.quizbattle.battles.activities;

import android.media.MediaPlayer;
import android.widget.Button;
import android.widget.TextView;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.adapters.MatchResultRankingAdapter;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.home_rank.activities.HomeActivity;
import com.hoangcn.quizbattle.battles.models.MatchResultUserItem;
import com.hoangcn.quizbattle.battles.models.MatchReviewResponse;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class MatchResultActivity extends AppCompatActivity {
    private BattleService battleService;
    private MatchResultRankingAdapter rankingAdapter;
    private TextView tvTitle;
    private MediaPlayer resultSoundPlayer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_match_result);

        bindViews();
        playFinalResultSound();
        Button btnHome = findViewById(R.id.btn_home);
        btnHome.setOnClickListener(v -> {
            var intent = new android.content.Intent(this, HomeActivity.class);
            intent.setFlags(android.content.Intent.FLAG_ACTIVITY_CLEAR_TOP | android.content.Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        battleService = new BattleService(this);
        int matchId = getIntent().getIntExtra("matchId", 0);
        if (matchId > 0) {
            loadMatchReview(matchId);
        }
    }

    private void loadMatchReview(int matchId) {
        battleService.getMatchReview(matchId, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchReviewResponse> data) {
                if (data == null || data.getData() == null) return;
                runOnUiThread(() -> renderReview(data.getData()));
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    tvTitle.setText("Không thể tải kết quả: " + message);
                });
            }
        });
    }

    private void bindViews() {
        tvTitle = findViewById(R.id.tv_title);
        RecyclerView rvResultUsers = findViewById(R.id.rv_result_users);
        rvResultUsers.setLayoutManager(new LinearLayoutManager(this));
        rankingAdapter = new MatchResultRankingAdapter(this);
        rvResultUsers.setAdapter(rankingAdapter);
    }

    private void renderReview(MatchReviewResponse response) {
        var users = response.getUsers() == null ? new ArrayList<MatchResultUserItem>() : new ArrayList<>(response.getUsers());
        users.sort(Comparator.comparingInt(MatchResultUserItem::getScore).reversed().thenComparing(MatchResultUserItem::getUserId));
        rankingAdapter.submit(users);
        tvTitle.setText("Kết quả xếp hạng");
    }

    private void playFinalResultSound() {
        int resourceId = getResources().getIdentifier("showfinalresult", "raw", getPackageName());
        if (resourceId == 0) {
            return;
        }
        releaseFinalResultSound();
        resultSoundPlayer = MediaPlayer.create(this, resourceId);
        if (resultSoundPlayer == null) {
            return;
        }
        resultSoundPlayer.setOnCompletionListener(mp -> {
            mp.release();
            if (resultSoundPlayer == mp) {
                resultSoundPlayer = null;
            }
        });
        resultSoundPlayer.start();
    }

    private void releaseFinalResultSound() {
        if (resultSoundPlayer == null) {
            return;
        }
        try {
            if (resultSoundPlayer.isPlaying()) {
                resultSoundPlayer.stop();
            }
        } catch (IllegalStateException ignored) {
        }
        resultSoundPlayer.release();
        resultSoundPlayer = null;
    }

    @Override
    protected void onDestroy() {
        releaseFinalResultSound();
        super.onDestroy();
    }
}