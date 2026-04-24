package com.hoangcn.quizbattle.events.activities;

import android.content.Intent;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.widget.Button;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.adapters.EventAdapter;
import com.hoangcn.quizbattle.events.api.EventService;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.concurrent.Executors;

public class EventActivity extends AppCompatActivity {
    private List<EventModel> events = new ArrayList<>();
    private EventService service;
    private EventAdapter adapter;
    private RecyclerView rvEvents;
    private Button btnBack;
    private MediaPlayer mediaPlayer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_event);

        initViews();
        setListeners();
        loadData();
    }

    @Override
    protected void onResume() {
        super.onResume();
        loadData();
        startBackgroundMusic();
    }

    private void startBackgroundMusic() {
        if (mediaPlayer == null) {
            mediaPlayer = MediaPlayer.create(this, R.raw.event_home);
            mediaPlayer.setLooping(true);
        }
        if (!mediaPlayer.isPlaying()) {
            mediaPlayer.start();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        if (mediaPlayer != null && mediaPlayer.isPlaying()) {
            mediaPlayer.pause();
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mediaPlayer != null) {
            mediaPlayer.stop();
            mediaPlayer.release();
            mediaPlayer = null;
        }
    }

    private void loadData() {
        Executors.newSingleThreadExecutor().execute(() -> {
            service = new EventService(this);

            // Get events
            service.getAllEvents(new ApiCallback<List<EventModel>>() {
                @Override
                public void onSuccess(ApiResponse<List<EventModel>> data) {
                    events.clear();
                    events.addAll(data.getData());
                    adapter.notifyDataSetChanged();
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> {
                        Toast.makeText(
                                EventActivity.this,
                                "Tải dữ liệu sự kiện thất bại: " + message,
                                Toast.LENGTH_SHORT).show();
                    });
                }
            });
        });

    }

    private void setListeners() {
        btnBack.setOnClickListener(l -> finish());
        adapter.setOnItemClickListener(event -> {
            if (event.getType().equals("LuckySpin")) {
                Intent intent = new Intent(EventActivity.this, LuckySpinEventActivity.class);
                intent.putExtra(LuckySpinEventActivity.KEY_EVENT, event);
                startActivity(intent);
                return;
            }

            if (event.getType().equals("QuizMilestoneChallenge")) {
                Intent intent = new Intent(EventActivity.this, QuizMilestoneChallengeActivity.class);
                intent.putExtra(QuizMilestoneChallengeActivity.KEY_EVENT, event);
                startActivity(intent);
                return;
            }

            if (event.getType().equals("TournamentRewards")) {
                Intent intent = new Intent(EventActivity.this, TournamentRewardsEventActivity.class);
                intent.putExtra(TournamentRewardsEventActivity.KEY_EVENT, event);
                startActivity(intent);
                return;
            }
        });
    }

    private void initViews() {
        rvEvents = findViewById(R.id.rv_event);
        btnBack = findViewById(R.id.btnBack);

        adapter = new EventAdapter(events);
        rvEvents.setAdapter(adapter);
    }
}