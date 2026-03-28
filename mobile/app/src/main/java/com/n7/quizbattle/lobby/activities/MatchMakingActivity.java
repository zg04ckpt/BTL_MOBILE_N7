package com.n7.quizbattle.lobby.activities;

import android.graphics.Typeface;
import android.os.Bundle;
import android.os.Handler;
import android.transition.ChangeBounds;
import android.transition.TransitionManager;
import android.util.Log;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.bumptech.glide.Glide;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.ListenerRegistration;
import com.n7.quizbattle.R;
import com.n7.quizbattle.lobby.api.LobbyService;
import com.n7.quizbattle.lobby.models.JoinLobbyResponse;
import com.n7.quizbattle.lobby.models.LobbyPlayer;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;

import java.util.List;
import java.util.Map;

public class MatchMakingActivity extends AppCompatActivity {
    private TextView tvTimerValue, tvMatchStatus;
    private LinearLayout llPlayersContainer, llLogsContainer;

    private Button btn_cancel_match;

    private ListenerRegistration lobbyListener;

    private String roomId;
    private FirebaseFirestore db;
    private LobbyService lobbyService;

    private final int MY_USER_ID = 2;

    private int seconds = 0;
    private Handler timerHandler = new Handler();
    private Runnable timerRunnable = new Runnable() {
        @Override
        public void run() {
            seconds++;
            int mins = seconds / 60;
            int secs = seconds % 60;
            tvTimerValue.setText(String.format("%02d:%02d", mins, secs));
            timerHandler.postDelayed(this, 1000);
        }
    };


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_match_making);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        roomId = getIntent().getStringExtra("lobbyRoomId");
        lobbyService = new LobbyService();
        db = FirebaseFirestore.getInstance();

        initViews();
        startTimer();
        initEvents();
        listenToLobbyRoom();
    }

    private void initViews() {
        tvTimerValue = findViewById(R.id.tv_timer_value);
        tvMatchStatus = findViewById(R.id.tv_match_status);
        llPlayersContainer = findViewById(R.id.ll_players_container);
        llLogsContainer = findViewById(R.id.ll_logs_container);
        btn_cancel_match = findViewById(R.id.btn_cancel_match);
    }

    private void listenToLobbyRoom() {
        if(roomId == null) return;

        lobbyListener = db.collection("lobby-rooms").document(roomId)
                .addSnapshotListener((snapshot, e) -> {
                    if(e != null) {
                        Log.e("FIRESTORE_ERROR", "Lỗi lắng nghe: " + e.getMessage());
                        return;
                    }
                    if(snapshot != null && snapshot.exists()) {
                        Log.d("FIRESTORE_DATA", "Đã nhận dữ liệu: " + snapshot.getData());
                        updateUI(snapshot);
                    } else {
                        Log.d("FIRESTORE_DATA", "Document không tồn tại!");
                    }
                });
    }

    private void updateUI(DocumentSnapshot snapshot) {
        Long maxPlayersLong = snapshot.getLong("MaxPlayers");
        int maxPlayers = (maxPlayersLong != null) ? maxPlayersLong.intValue(): 0;
        List<Map<String, Object>> playersRaw = (List<Map<String, Object>>) snapshot.get("Players");
        if(playersRaw == null) return;

        tvMatchStatus.setText("Đã ghép được " + playersRaw.size() + "/" + maxPlayers);

        renderPlayers(playersRaw);

        List<String> logs = (List<String>) snapshot.get("StatusLogs");
        renderLogs(logs);
    }

    public void renderPlayers(List<Map<String, Object>> playersRaw) {
        llPlayersContainer.removeAllViews();
        for(Map<String, Object> p : playersRaw) {
            LobbyPlayer player = new LobbyPlayer();
            player.setUserId(((Long) p.get("UserId")).intValue());
            player.setDisplayName((String) p.get("DisplayName"));
            player.setLevel(((Long) p.get("Level")).intValue());
            player.setAvatarUrl((String) p.get("AvatarUrl"));

            View view = LayoutInflater.from(this).inflate(R.layout.item_player_matching, llPlayersContainer, false);

            TextView tvName = view.findViewById(R.id.tv_player_name);
            TextView tvLevel= view.findViewById(R.id.tv_player_level);
            com.google.android.material.imageview.ShapeableImageView ivAvatar = view.findViewById(R.id.iv_player_avatar);

            tvName.setText(player.getUserId() == MY_USER_ID ? "Bạn" : player.getDisplayName());
            tvLevel.setText("Lv" + player.getLevel());

            String avatarUrl = "https://quizbattle.hoangcn.com" + player.getAvatarUrl();
            Glide.with(this).load(avatarUrl).centerCrop().into(ivAvatar);

            llPlayersContainer.addView(view);
        }
    }

    private void renderLogs(List<String> logs) {
        TransitionManager.beginDelayedTransition(llLogsContainer, new ChangeBounds());

        llLogsContainer.removeAllViews();
        if(logs == null) return;

        for(int i = logs.size() - 1; i >= 0; i--) {
            TextView tv = new TextView(this);
            tv.setText(logs.get(i));
            tv.setGravity(Gravity.CENTER);

            if(i == logs.size() - 1) {
                tv.setTextSize(TypedValue.COMPLEX_UNIT_SP, 16);
                tv.setTextColor(getResources().getColor(R.color.color3));
                tv.setTypeface(null, Typeface.BOLD);
                tv.setAlpha(1.0f);
            } else {
                tv.setTextSize(TypedValue.COMPLEX_UNIT_SP, 12);
                tv.setTextColor(getResources().getColor(R.color.color11));
                tv.setAlpha(0.6f);
            }

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            params.setMargins(0, 8, 0, 8);
            llLogsContainer.addView(tv, params);

            final ScrollView svLogs = findViewById(R.id.sv_logs);
            svLogs.post(() -> svLogs.fullScroll(View.FOCUS_UP));
        }
    }

    private void startTimer() {
        timerHandler.postDelayed(timerRunnable, 0);
    }

    private void initEvents() {
        btn_cancel_match.setOnClickListener(v -> {
            showCancelConfirmationSheet();
        });
    }

    private void showCancelConfirmationSheet() {
        BottomSheetDialog bottomSheetDialog = new BottomSheetDialog(this, R.style.BottomSheetDialogTheme);
        View sheetView = getLayoutInflater().inflate(R.layout.layout_bottom_sheet_cancel_match, null);

        View btnConfirm = sheetView.findViewById(R.id.btn_confirm_cancel);
        View btnStay = sheetView.findViewById(R.id.btn_stay_wait);

        btnConfirm.setOnClickListener(v -> {
            btnConfirm.setEnabled(false);

            lobbyService.outLobby(roomId, new ApiCallback<JoinLobbyResponse>() {
                @Override
                public void onSuccess(ApiResponse<JoinLobbyResponse> data) {
                    bottomSheetDialog.dismiss();
                    finish();
                }

                @Override
                public void onError(String message) {
                    btnConfirm.setEnabled(true);
                    Toast.makeText(MatchMakingActivity.this, "Không thể huỷ trận: " + message, Toast.LENGTH_SHORT).show();
                }
            });
        });

        btnStay.setOnClickListener(v -> {
            bottomSheetDialog.dismiss();
        });

        bottomSheetDialog.setContentView(sheetView);
        bottomSheetDialog.show();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        timerHandler.removeCallbacks(timerRunnable);
        if (lobbyListener != null) lobbyListener.remove();
    }

}