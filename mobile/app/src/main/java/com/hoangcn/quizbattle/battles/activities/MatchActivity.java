package com.hoangcn.quizbattle.battles.activities;

import android.content.Intent;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.widget.ImageView;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.battles.models.MatchInfoResponse;
import com.hoangcn.quizbattle.battles.models.MatchLoudspeakerInventoryResponse;
import com.hoangcn.quizbattle.battles.models.MatchQuestionContentItem;
import com.hoangcn.quizbattle.battles.models.SubmitMatchAnswerRequest;
import com.hoangcn.quizbattle.battles.models.UseLoudspeakerRequest;
import com.hoangcn.quizbattle.events.activities.QuizMilestoneChallengeActivity;
import com.hoangcn.quizbattle.events.api.EventService;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.UpdateQuizMilestoneProgressRequest;
import com.hoangcn.quizbattle.home_rank.activities.HomeActivity;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;
import com.google.firebase.Timestamp;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.ListenerRegistration;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Random;
import java.util.Set;
import java.util.HashMap;

public class MatchActivity extends AppCompatActivity {
    private static final int MAX_MATCH_INFO_RETRIES = 8;
    private static final int MATCH_INFO_RETRY_DELAY_MS = 350;

    private BattleService battleService;
    private TextView tvTimer;
    private TextView tvQuestionCount;
    private TextView tvDifficulty;
    private TextView tvCategory;
    private TextView tvQuestionText;
    private TextView tvOptionA;
    private TextView tvOptionB;
    private TextView tvOptionC;
    private TextView tvOptionD;
    private TextView tvScore;
    private TextView tvFeedback;
    private android.view.View layoutFeedback;
    private android.view.View btnExpandMenu;
    private android.view.View sidePanel;
    private ProgressBar progressBar;
    private ImageView ivQuestionImage;
    private ImageView ivSaveQuestion;
    private TextView tvToolLabel;
    private TextView tvToolCount;
    private TextView tvLoudspeakerNotice;

    private final Handler timerHandler = new Handler(Looper.getMainLooper());
    private final FirebaseFirestore db = FirebaseFirestore.getInstance();
    private EventService eventService;
    private List<MatchQuestionContentItem> questions = new ArrayList<>();
    private String trackingId;
    private int maxSecondPerQuestion = 10;
    private int currentQuestionIndex = 0;
    private int currentScore = 0;
    private int secondsLeft = 0;
    private int myUserId = -1;
    private boolean questionLocked = false;
    private int matchInfoRetryCount = 0;
    private int loudspeakerCount = 0;
    private int currentMatchId = 0;
    private boolean isSoloMatch = false;
    private boolean isChallengeMatch = false;
    private int challengeEventId = 0;
    private int challengeThresholdId = 0;
    private boolean isLeavingAfk = false;
    private EventModel challengeEvent;
    private ListenerRegistration matchRoomListener;
    private MediaPlayer soundPlayer;
    private final Set<String> seenLoudspeakerEventKeys = new HashSet<>();
    private List<TextView> optionViews;
    private final Random random = new Random();
    private final Handler heartbeatHandler = new Handler(Looper.getMainLooper());
    // Heartbeat presence 5s/lần để backend phát hiện AFK qua Firestore.
    private final Runnable heartbeatRunnable = new Runnable() {
        @Override
        public void run() {
            if (!isSoloMatch && trackingId != null && !trackingId.isEmpty()) {
                sendPresence(true);
            }
            heartbeatHandler.postDelayed(this, 5000);
        }
    };
    private final String[] loudspeakerMessages = new String[]
    {
        "Ga qua, de sieu de!",
        "Tap trung di, sap het gio!",
        "Nhanh tay len nao!"
    };

    private final Runnable timerRunnable = new Runnable() {
        @Override
        public void run() {
            if (questionLocked) return;
            secondsLeft = Math.max(0, secondsLeft - 1);
            updateTimerLabel();
            if (secondsLeft <= 0) {
                handleTimeoutAndAdvance();
                return;
            }
            timerHandler.postDelayed(this, 1000);
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_match);

        battleService = new BattleService(this);
        eventService = new EventService(this);
        myUserId = SharedPreferenceUtil.getInstance(this).getInt("userId", -1);
        isChallengeMatch = getIntent().getBooleanExtra("isChallengeMatch", false);
        challengeEventId = getIntent().getIntExtra("challengeEventId", 0);
        challengeThresholdId = getIntent().getIntExtra("challengeThresholdId", 0);
        challengeEvent = (EventModel) getIntent().getSerializableExtra(QuizMilestoneChallengeActivity.KEY_EVENT);
        bindViews();
        initEvents();
        loadMatchInfo();
    }

    private void bindViews() {
        tvTimer = findViewById(R.id.tvTimer);
        tvQuestionCount = findViewById(R.id.tvQuestionCount);
        tvDifficulty = findViewById(R.id.tvDifficulty);
        tvCategory = findViewById(R.id.tvCategory);
        tvQuestionText = findViewById(R.id.tvQuestionText);
        tvOptionA = findViewById(R.id.tvOptionA);
        tvOptionB = findViewById(R.id.tvOptionB);
        tvOptionC = findViewById(R.id.tvOptionC);
        tvOptionD = findViewById(R.id.tvOptionD);
        tvScore = findViewById(R.id.tvScore);
        tvFeedback = findViewById(R.id.tvFeedback);
        layoutFeedback = findViewById(R.id.layoutFeedback);
        btnExpandMenu = findViewById(R.id.btnExpandMenu);
        sidePanel = findViewById(R.id.sidePanel);
        progressBar = findViewById(R.id.progressBar);
        ivQuestionImage = findViewById(R.id.ivQuestionImage);
        ivSaveQuestion = findViewById(R.id.ivSaveQuestion);
        tvToolLabel = findViewById(R.id.tvToolLabel);
        tvToolCount = findViewById(R.id.tvToolCount);
        tvLoudspeakerNotice = findViewById(R.id.tvLoudspeakerNotice);
        optionViews = Arrays.asList(tvOptionA, tvOptionB, tvOptionC, tvOptionD);
    }

    private void initEvents() {
        Button btnExit = findViewById(R.id.btnExitBattle);
        btnExit.setOnClickListener(v -> leaveMatchAsAfk());

        tvOptionA.setOnClickListener(v -> submitCurrentAnswer(getTextOrNull(tvOptionA)));
        tvOptionB.setOnClickListener(v -> submitCurrentAnswer(getTextOrNull(tvOptionB)));
        tvOptionC.setOnClickListener(v -> submitCurrentAnswer(getTextOrNull(tvOptionC)));
        tvOptionD.setOnClickListener(v -> submitCurrentAnswer(getTextOrNull(tvOptionD)));
        btnExpandMenu.setOnClickListener(v -> toggleSidePanel());
        sidePanel.setOnClickListener(v -> useLoudspeaker());
    }

    private void loadMatchInfo() {
        battleService.getMatchState(new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<com.hoangcn.quizbattle.battles.models.MatchStateResponse> data) {
                if (data != null && data.getData() != null && data.getData().getTrackingId() != null
                    && !data.getData().getTrackingId().isEmpty()) {
                    loadMatchInfoByTracking(data.getData().getTrackingId());
                    return;
                }
                loadLatestMatchInfo();
            }

            @Override
            public void onError(String message) {
                loadLatestMatchInfo();
            }
        });
    }

    private void loadMatchInfoByTracking(String trackingId) {
        battleService.getMatchInfoByTracking(trackingId, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchInfoResponse> data) {
                if (data == null || data.getData() == null) return;
                matchInfoRetryCount = 0;
                runOnUiThread(() -> renderMatchInfo(data.getData()));
            }

            @Override
            public void onError(String message) {
                loadLatestMatchInfo();
            }
        });
    }

    private void loadLatestMatchInfo() {
        battleService.getMatchInfo(new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchInfoResponse> data) {
                if (data == null || data.getData() == null) return;
                matchInfoRetryCount = 0;
                runOnUiThread(() -> renderMatchInfo(data.getData()));
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    String error = message == null ? "" : message.toLowerCase(Locale.ROOT);
                    if (error.contains("match does not exist") && matchInfoRetryCount < MAX_MATCH_INFO_RETRIES) {
                        matchInfoRetryCount++;
                        tvQuestionText.setText("Đang đồng bộ trận đấu...");
                        timerHandler.postDelayed(MatchActivity.this::loadMatchInfo, MATCH_INFO_RETRY_DELAY_MS);
                        return;
                    }

                    Toast.makeText(MatchActivity.this, "Không tải được trận: " + message, Toast.LENGTH_SHORT).show();
                    finish();
                });
            }
        });
    }

    private void renderMatchInfo(MatchInfoResponse info) {
        trackingId = info.getTrackingId();
        currentMatchId = info.getMatchId();
        isSoloMatch = info.isSolo();
        maxSecondPerQuestion = Math.max(1, info.getMaxSecondPerQuestions());
        questions = info.getQuestions() == null ? new ArrayList<>() : info.getQuestions();
        currentQuestionIndex = 0;
        currentScore = 0;
        sidePanel.setVisibility(android.view.View.GONE);
        tvToolLabel.setText("Loa");
        if (!isSoloMatch) {
            loadLoudspeakerInventory();
            subscribeMatchRoomEvents();
            heartbeatHandler.removeCallbacks(heartbeatRunnable);
            heartbeatHandler.post(heartbeatRunnable);
        }
        updateScoreLabel();

        if (questions.isEmpty()) {
            tvQuestionText.setText("Đang chờ câu hỏi...");
            return;
        }

        renderCurrentQuestion();
    }

    private void bindAnswerOptions(List<String> answers) {
        int count = answers == null ? 0 : Math.min(answers.size(), optionViews.size());
        for (int i = 0; i < optionViews.size(); i++) {
            var optionView = optionViews.get(i);
            if (i < count) {
                optionView.setText(getAnswerAt(answers, i));
                optionView.setVisibility(android.view.View.VISIBLE);
            } else {
                optionView.setText("");
                optionView.setVisibility(android.view.View.GONE);
            }
        }
    }

    private String getAnswerAt(List<String> answers, int index) {
        if (answers == null || index >= answers.size()) return "";
        return answers.get(index);
    }

    private String getTextOrNull(TextView tv) {
        var value = tv.getText() == null ? "" : tv.getText().toString().trim();
        return value.isEmpty() ? null : value;
    }

    private void renderCurrentQuestion() {
        if (currentQuestionIndex >= questions.size()) {
            goToWaitResult();
            return;
        }

        var question = questions.get(currentQuestionIndex);
        tvQuestionCount.setText("Câu " + (currentQuestionIndex + 1) + "/" + questions.size());
        tvQuestionText.setText(question.getStringContent());
        tvCategory.setText(question.getTopicName() == null || question.getTopicName().isEmpty()
            ? "Tổng hợp"
            : question.getTopicName());
        tvDifficulty.setText(toDifficultyLabel(question.getLevel()));
        var imageUrl = question.getImageUrl();
        if (imageUrl != null && imageUrl.startsWith("/")) {
            imageUrl = "https://quizbattle.hoangcn.com" + imageUrl;
        }
        if (imageUrl != null && !imageUrl.trim().isEmpty()) {
            ivQuestionImage.setVisibility(android.view.View.VISIBLE);
            Glide.with(this).load(imageUrl).centerCrop().into(ivQuestionImage);
        } else {
            ivQuestionImage.setVisibility(android.view.View.GONE);
        }
        bindAnswerOptions(question.getStringAnswers());
        progressBar.setMax(Math.max(1, questions.size()));
        progressBar.setProgress(currentQuestionIndex + 1);

        questionLocked = false;
        setOptionsEnabled(true);
        secondsLeft = maxSecondPerQuestion;
        updateTimerLabel();
        timerHandler.removeCallbacks(timerRunnable);
        timerHandler.postDelayed(timerRunnable, 1000);
    }

    private String toDifficultyLabel(int level) {
        if (level <= 1) return "Dễ";
        if (level == 2) return "Trung bình";
        return "Khó";
    }

    private void toggleSidePanel() {
        sidePanel.setVisibility(sidePanel.getVisibility() == android.view.View.VISIBLE
            ? android.view.View.GONE
            : android.view.View.VISIBLE);
    }

    private void loadLoudspeakerInventory() {
        battleService.getLoudspeakerInventory(new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchLoudspeakerInventoryResponse> data) {
                if (data == null || data.getData() == null) return;
                runOnUiThread(() -> updateLoudspeakerUi(data.getData().getCount()));
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> updateLoudspeakerUi(0));
            }
        });
    }

    private void updateLoudspeakerUi(int count) {
        loudspeakerCount = Math.max(0, count);
        tvToolCount.setText("x" + loudspeakerCount);
        sidePanel.setAlpha(loudspeakerCount > 0 ? 1f : 0.5f);
        ivSaveQuestion.setAlpha(loudspeakerCount > 0 ? 1f : 0.6f);
    }

    private void useLoudspeaker() {
        if (trackingId == null || trackingId.isEmpty()) {
            return;
        }
        if (loudspeakerCount <= 0) {
            Toast.makeText(this, "Ban khong con loa", Toast.LENGTH_SHORT).show();
            return;
        }

        showLoudspeakerMessagePicker();
    }

    private void showLoudspeakerMessagePicker() {
        new AlertDialog.Builder(this)
            .setTitle("Chon thong diep loa")
            .setItems(loudspeakerMessages, (dialog, which) -> {
                if (which < 0 || which >= loudspeakerMessages.length) {
                    return;
                }
                sidePanel.setVisibility(android.view.View.GONE);
                sendLoudspeaker(loudspeakerMessages[which]);
            })
            .setNegativeButton("Huy", (dialog, which) -> sidePanel.setVisibility(android.view.View.GONE))
            .show();
    }

    private void sendLoudspeaker(String message) {
        battleService.useLoudspeaker(new UseLoudspeakerRequest(trackingId, message), new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<MatchLoudspeakerInventoryResponse> data) {
                runOnUiThread(() -> {
                    var remain = data != null && data.getData() != null ? data.getData().getCount() : (loudspeakerCount - 1);
                    updateLoudspeakerUi(remain);
                    sidePanel.setVisibility(android.view.View.GONE);
                    Toast.makeText(MatchActivity.this, "Da phat thong bao bang loa", Toast.LENGTH_SHORT).show();
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    sidePanel.setVisibility(android.view.View.GONE);
                    Toast.makeText(MatchActivity.this, "Dung loa that bai: " + message, Toast.LENGTH_SHORT).show();
                });
            }
        });
    }

    private void subscribeMatchRoomEvents() {
        if (trackingId == null || trackingId.isEmpty()) {
            return;
        }

        if (matchRoomListener != null) {
            matchRoomListener.remove();
        }
        seenLoudspeakerEventKeys.clear();

        matchRoomListener = db.collection("match-rooms").document(trackingId)
            .addSnapshotListener((snapshot, error) -> {
                if (error != null || snapshot == null || !snapshot.exists()) {
                    return;
                }
                List<Map<String, Object>> events = (List<Map<String, Object>>) snapshot.get("Events");
                if (events == null) return;
                for (Map<String, Object> event : events) {
                    if (event == null) continue;
                    var type = event.get("Type");
                    if (type == null) type = event.get("type");
                    String normalizedType = type == null ? "" : String.valueOf(type).trim().toLowerCase(Locale.ROOT);
                    if (!"loudspeaker_used".equals(normalizedType)) continue;

                    var actorObj = event.get("ActorUserId");
                    if (actorObj == null) actorObj = event.get("actorUserId");
                    int actorUserId = parseUserId(actorObj);
                    if (actorUserId == myUserId) continue;

                    var messageObj = event.get("Message");
                    if (messageObj == null) messageObj = event.get("message");
                    String text = messageObj == null ? "Thong diep tu doi thu" : String.valueOf(messageObj);
                    var timestampObj = event.get("Timestamp");
                    if (timestampObj == null) timestampObj = event.get("timestamp");
                    String eventKey = actorUserId + "|" + text + "|" + String.valueOf(timestampObj);
                    if (seenLoudspeakerEventKeys.contains(eventKey)) continue;
                    seenLoudspeakerEventKeys.add(eventKey);
                    showLoudspeakerBanner(text);
                }
            });
                
    }

    private int parseUserId(Object value) {
        if (value instanceof Number) {
            return ((Number) value).intValue();
        }
        if (value == null) {
            return -1;
        }
        try {
            return Integer.parseInt(String.valueOf(value));
        } catch (Exception ignored) {
            return -1;
        }
    }

    private void showLoudspeakerBanner(String message) {
        runOnUiThread(() -> {
            tvLoudspeakerNotice.setText("Loa đối thủ: " + message);
            tvLoudspeakerNotice.setTranslationY(-20f);
            tvLoudspeakerNotice.setAlpha(0f);
            tvLoudspeakerNotice.setVisibility(android.view.View.VISIBLE);
            tvLoudspeakerNotice.animate().translationY(0f).alpha(1f).setDuration(180).start();
            timerHandler.removeCallbacks(hideLoudspeakerBannerRunnable);
            timerHandler.postDelayed(hideLoudspeakerBannerRunnable, 2200);
        });
    }

    private final Runnable hideLoudspeakerBannerRunnable = () -> {
        if (tvLoudspeakerNotice != null) {
            tvLoudspeakerNotice.animate().translationY(-12f).alpha(0f).setDuration(150).withEndAction(() -> {
                tvLoudspeakerNotice.setVisibility(android.view.View.GONE);
                tvLoudspeakerNotice.setTranslationY(0f);
            }).start();
        }
    };

    private void updateTimerLabel() {
        tvTimer.setText("Thời gian còn lại: " + secondsLeft + "s");
    }

    private void updateScoreLabel() {
        tvScore.setText("Điểm của bạn: " + currentScore + "đ");
    }

    private void setOptionsEnabled(boolean enabled) {
        float alpha = enabled ? 1f : 0.6f;
        for (var optionView : optionViews) {
            if (optionView.getVisibility() != android.view.View.VISIBLE) {
                continue;
            }
            optionView.setEnabled(enabled);
            optionView.setAlpha(alpha);
        }
    }

    private void submitCurrentAnswer(String selectedAnswer) {
        if (questionLocked || currentQuestionIndex >= questions.size() || trackingId == null || trackingId.isEmpty()) {
            return;
        }

        questionLocked = true;
        setOptionsEnabled(false);
        timerHandler.removeCallbacks(timerRunnable);

        var question = questions.get(currentQuestionIndex);
        boolean isCorrect = isAnswerCorrect(selectedAnswer, question.getCorrectAnswers());
        List<String> answers = new ArrayList<>();
        if (selectedAnswer != null && !selectedAnswer.isEmpty()) {
            answers.add(selectedAnswer);
        }
        var request = new SubmitMatchAnswerRequest(trackingId, question.getId(), answers);
        battleService.submitMatchAnswer(request, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<Object> data) {
                runOnUiThread(() -> {
                    if (isCorrect) {
                        currentScore += 20;
                        updateScoreLabel();
                    }
                    playAnswerResultSound(isCorrect);
                    showFeedback(isCorrect);
                    timerHandler.postDelayed(() -> {
                        hideFeedback();
                        currentQuestionIndex++;
                        renderCurrentQuestion();
                    }, 700);
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    Toast.makeText(MatchActivity.this, "Gửi đáp án thất bại, thử lại", Toast.LENGTH_SHORT).show();
                    if (secondsLeft <= 0) {
                        questionLocked = false;
                        handleTimeoutAndAdvance();
                        return;
                    }
                    questionLocked = false;
                    setOptionsEnabled(true);
                    timerHandler.postDelayed(timerRunnable, 1000);
                });
            }
        });
    }

    private void handleTimeoutAndAdvance() {
        if (questionLocked) {
            return;
        }
        questionLocked = true;
        setOptionsEnabled(false);
        timerHandler.removeCallbacks(timerRunnable);
        secondsLeft = 0;
        updateTimerLabel();
        playIncorrectSound();
        showMissedFeedback();
        submitEmptyAnswerWithoutBlocking();
        timerHandler.postDelayed(() -> {
            hideFeedback();
            currentQuestionIndex++;
            renderCurrentQuestion();
        }, 800);
    }

    private void submitEmptyAnswerWithoutBlocking() {
        if (trackingId == null || trackingId.isEmpty() || currentQuestionIndex >= questions.size()) {
            return;
        }

        var question = questions.get(currentQuestionIndex);
        var request = new SubmitMatchAnswerRequest(trackingId, question.getId(), new ArrayList<>());
        battleService.submitMatchAnswer(request, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<Object> data) { }

            @Override
            public void onError(String message) { }
        });
    }

    private boolean isAnswerCorrect(String selectedAnswer, List<String> correctAnswers) {
        if (selectedAnswer == null || correctAnswers == null || correctAnswers.isEmpty()) {
            return false;
        }
        String normalized = selectedAnswer.trim().toLowerCase(Locale.ROOT);
        if (correctAnswers.size() != 1) {
            // UI currently supports single-choice selection only.
            return false;
        }
        return correctAnswers.get(0) != null
            && normalized.equals(correctAnswers.get(0).trim().toLowerCase(Locale.ROOT));
    }

    private void showFeedback(boolean isCorrect) {
        layoutFeedback.setVisibility(android.view.View.VISIBLE);
        tvFeedback.setText(isCorrect ? "Bạn đã chọn đúng" : "Bạn đã chọn sai");
        layoutFeedback.setBackgroundResource(isCorrect ? R.drawable.bg_option_card_selected : R.drawable.bg_button_red);
    }

    private void playAnswerResultSound(boolean isCorrect) {
        if (isCorrect) {
            playRandomCorrectSound();
            return;
        }
        playIncorrectSound();
    }

    private void playRandomCorrectSound() {
        String[] candidates = new String[] {
            "correct",
            "correct1", "correct2", "correct3",
            "correct_1", "correct_2", "correct_3"
        };
        List<Integer> existing = new ArrayList<>();
        for (String candidate : candidates) {
            int id = findRawResourceId(candidate);
            if (id != 0) {
                existing.add(id);
            }
        }
        if (existing.isEmpty()) {
            return;
        }
        playSound(existing.get(random.nextInt(existing.size())));
    }

    private void playIncorrectSound() {
        int resourceId = findRawResourceId("incorrect");
        if (resourceId != 0) {
            playSound(resourceId);
        }
    }

    private int findRawResourceId(String name) {
        return getResources().getIdentifier(name, "raw", getPackageName());
    }

    private void playSound(int rawResourceId) {
        if (rawResourceId == 0) {
            return;
        }
        releaseSoundPlayer();
        soundPlayer = MediaPlayer.create(this, rawResourceId);
        if (soundPlayer == null) {
            return;
        }
        soundPlayer.setOnCompletionListener(mp -> {
            mp.release();
            if (soundPlayer == mp) {
                soundPlayer = null;
            }
        });
        soundPlayer.start();
    }

    private void releaseSoundPlayer() {
        if (soundPlayer == null) {
            return;
        }
        try {
            if (soundPlayer.isPlaying()) {
                soundPlayer.stop();
            }
        } catch (IllegalStateException ignored) {
        }
        soundPlayer.release();
        soundPlayer = null;
    }

    private void showMissedFeedback() {
        layoutFeedback.setVisibility(android.view.View.VISIBLE);
        tvFeedback.setText("Bạn đã bỏ lỡ 1 câu");
        layoutFeedback.setBackgroundResource(R.drawable.bg_button_red);
    }

    private void hideFeedback() {
        layoutFeedback.setVisibility(android.view.View.GONE);
    }

    /**
     * Người chơi chủ động thoát / AFK: không đi WaitResult → MatchResult (dễ lỗi review không tồn tại), mà về Home.
     * Trận thử thách milestone vẫn cập nhật tiến độ rồi quay lại màn thử thách.
     */
    private void finishAfterUserLeftMatch() {
        heartbeatHandler.removeCallbacks(heartbeatRunnable);
        sendPresence(false);
        if (isChallengeMatch && challengeEventId > 0 && challengeThresholdId > 0) {
            eventService.updateQuizMilestoneProgress(
                new UpdateQuizMilestoneProgressRequest(challengeEventId, challengeThresholdId, null, true),
                new ApiCallback<>() {
                    @Override
                    public void onSuccess(ApiResponse<com.hoangcn.quizbattle.events.models.EventProgressModel> data) {
                        navigateBackToChallenge();
                    }

                    @Override
                    public void onError(String message) {
                        runOnUiThread(() -> Toast.makeText(MatchActivity.this, "Cập nhật tiến độ thử thách thất bại: " + message, Toast.LENGTH_SHORT).show());
                        navigateBackToChallenge();
                    }
                });
            return;
        }
        Intent homeIntent = new Intent(this, HomeActivity.class);
        homeIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
        startActivity(homeIntent);
        finish();
    }

    private void goToWaitResult() {
        heartbeatHandler.removeCallbacks(heartbeatRunnable);
        sendPresence(false);
        if (isChallengeMatch && challengeEventId > 0 && challengeThresholdId > 0) {
            eventService.updateQuizMilestoneProgress(
                new UpdateQuizMilestoneProgressRequest(challengeEventId, challengeThresholdId, null, true),
                new ApiCallback<>() {
                    @Override
                    public void onSuccess(ApiResponse<com.hoangcn.quizbattle.events.models.EventProgressModel> data) {
                        navigateBackToChallenge();
                    }

                    @Override
                    public void onError(String message) {
                        runOnUiThread(() -> Toast.makeText(MatchActivity.this, "Cập nhật tiến độ thử thách thất bại: " + message, Toast.LENGTH_SHORT).show());
                        navigateBackToChallenge();
                    }
                });
            return;
        }

        if (isSoloMatch && currentMatchId > 0) {
            Intent intent = new Intent(this, MatchResultActivity.class);
            intent.putExtra("matchId", currentMatchId);
            startActivity(intent);
            finish();
            return;
        }
        Intent intent = new Intent(this, WaitResultActivity.class);
        startActivity(intent);
        finish();
    }

    private void leaveMatchAsAfk() {
        if (isLeavingAfk) return;
        isLeavingAfk = true;
        questionLocked = true;
        timerHandler.removeCallbacks(timerRunnable);
        setOptionsEnabled(false);

        if (isSoloMatch || trackingId == null || trackingId.isEmpty() || questions.isEmpty()) {
            finishAfterUserLeftMatch();
            return;
        }

        // AFK: gửi đáp án rỗng cho toàn bộ câu còn lại để không giữ trận.
        List<Integer> remainingQuestionIds = new ArrayList<>();
        for (int i = Math.max(0, currentQuestionIndex); i < questions.size(); i++) {
            remainingQuestionIds.add(questions.get(i).getId());
        }
        submitAfkAnswersSequentially(remainingQuestionIds, 0);
    }

    private void submitAfkAnswersSequentially(List<Integer> questionIds, int index) {
        if (questionIds == null || index >= questionIds.size()) {
            finishAfterUserLeftMatch();
            return;
        }
        int questionId = questionIds.get(index);
        var request = new SubmitMatchAnswerRequest(trackingId, questionId, new ArrayList<>());
        battleService.submitMatchAnswer(request, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<Object> data) {
                submitAfkAnswersSequentially(questionIds, index + 1);
            }

            @Override
            public void onError(String message) {
                submitAfkAnswersSequentially(questionIds, index + 1);
            }
        });
    }

    @Override
    public void onBackPressed() {
        leaveMatchAsAfk();
    }

    private void navigateBackToChallenge() {
        runOnUiThread(() -> {
            Intent intent = new Intent(MatchActivity.this, QuizMilestoneChallengeActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            if (challengeEvent != null) {
                intent.putExtra(QuizMilestoneChallengeActivity.KEY_EVENT, challengeEvent);
            }
            startActivity(intent);
            finish();
        });
    }

    @Override
    protected void onDestroy() {
        timerHandler.removeCallbacksAndMessages(null);
        heartbeatHandler.removeCallbacksAndMessages(null);
        sendPresence(false);
        if (matchRoomListener != null) {
            matchRoomListener.remove();
        }
        releaseSoundPlayer();
        super.onDestroy();
    }

    private void sendPresence(boolean isOnline) {
        if (isSoloMatch || trackingId == null || trackingId.isEmpty()) {
            return;
        }
        // Client chỉ cập nhật presence của chính mình trong Players.
        var docRef = db.collection("match-rooms").document(trackingId);
        docRef.get().addOnSuccessListener(snapshot -> {
            if (snapshot == null || !snapshot.exists()) return;
            List<Map<String, Object>> players = (List<Map<String, Object>>) snapshot.get("Players");
            if (players == null) return;
            boolean changed = false;
            for (Map<String, Object> player : players) {
                if (player == null) continue;
                int userId = parseUserId(player.get("UserId"));
                if (userId != myUserId) continue;
                Map<String, Object> mutable = new HashMap<>(player);
                mutable.put("IsOnline", isOnline);
                mutable.put("LastSeenAtUtc", Timestamp.now());
                player.clear();
                player.putAll(mutable);
                changed = true;
                break;
            }
            if (!changed) return;
            docRef.update("Players", players);
        });
    }
}