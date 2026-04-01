package com.hoangcn.quizbattle.events.activities;

import android.os.Bundle;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.DecelerateInterpolator;
import android.view.animation.RotateAnimation;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.api.EventService;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.luckyspin.SpinItemModel;
import com.hoangcn.quizbattle.events.models.luckyspin.WinSpinItemModel;
import com.hoangcn.quizbattle.events.utils.RewardUtil;
import com.hoangcn.quizbattle.events.views.LuckyWheelView;
import com.hoangcn.quizbattle.events.models.luckyspin.WheelItemModel;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.HashMap;
import java.util.List;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;

public class LuckySpinEventActivity extends AppCompatActivity {
    public static final String KEY_EVENT = "lucky_spin_event_data";
    public static final String KEY_PROGRESS = "lucky_spin_event_progress";

    public EventService eventService;

    private EventModel event;
    private int remainingSpinTime = 0;
    private HashMap<Integer, RewardModel> rewards = new HashMap<>();

    private LuckyWheelView wheelBody;
    private Button btnSpin, btnBack;
    private TextView tvRemainingSpinTime;

    private float currentDegree = 0f;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_lucky_spin_event);

        eventService = new EventService(this);

        initViews();
        setListeners();
        initData();
        renderSpinInfo();
    }

    private void renderSpinItems() {
        var spinItems = event.getSpinItems().stream().map(e -> {
            var reward = rewards.get(e.getReward().getEventRewardId());
            return new WheelItemModel(RewardUtil.getRewardIcon(reward));
        }).collect(Collectors.toList());
        wheelBody.setData(spinItems);
    }

    private void renderSpinInfo() {
        tvRemainingSpinTime.setText("Số lượt quay còn lại trong ngày: " + remainingSpinTime);
        if (remainingSpinTime == 0) {
            btnSpin.setVisibility(View.GONE);
        } else {
            btnSpin.setVisibility(View.VISIBLE);
        }
    }

    private void initData() {
        event = (EventModel) getIntent().getSerializableExtra(KEY_EVENT);
        loadProgress();
        getRewardInfos();
    }

    private void loadProgress() {
        Executors.newSingleThreadExecutor().execute(() -> {
            eventService.getAllMyProgress(new ApiCallback<List<EventProgressModel>>() {
                @Override
                public void onSuccess(ApiResponse<List<EventProgressModel>> data) {
                    var luckySpinProgress = data.getData().stream().filter(p -> p.getEventId() == event.getId()).findFirst();
                    if (luckySpinProgress.isPresent()) {
                        remainingSpinTime = event.getMaxSpinTimePerDay() - luckySpinProgress.get().getTodaySpinTime();
                    } else {
                        remainingSpinTime = event.getMaxSpinTimePerDay();
                    }
                    renderSpinInfo();
                }

                @Override
                public void onError(String message) {

                }
            });
        });
    }

    private void getRewardInfos() {
        eventService.getRewardMapping(new ApiCallback<List<RewardModel>>() {
            @Override
            public void onSuccess(ApiResponse<List<RewardModel>> data) {
                data.getData().forEach(e -> {
                    rewards.put(e.getId(), e);
                });
                renderSpinItems();
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    Toast.makeText(
                        LuckySpinEventActivity.this,
                        "Tải dữ liệu phần thưởng thất bại: " + message,
                        Toast.LENGTH_SHORT
                    );
                });
            }
        });
    }

    private void setListeners() {
        btnSpin.setOnClickListener(v -> {
            handleSpinClick();
        });
        btnBack.setOnClickListener(v -> {
            finish();
        });
    }

    private void handleSpinClick() {
        btnSpin.setVisibility(View.GONE);
        Executors.newSingleThreadExecutor().execute(() -> {
            eventService.getWinSpinItem(event.getId(), new ApiCallback<WinSpinItemModel>() {
                @Override
                public void onSuccess(ApiResponse<WinSpinItemModel> data) {
                    spinToSegment(data.getData().getWinItem());
                }

                @Override
                public void onError(String message) {
                    runOnUiThread(() -> {
                        Toast.makeText(
                            LuckySpinEventActivity.this,
                            "Quay thất bại: " + message,
                            Toast.LENGTH_SHORT).show();
                        btnSpin.setVisibility(View.GONE);
                    });
                }
            });
        });
    }

    private void spinToSegment(SpinItemModel chosenItem) {
        float segmentAngle = 360f / event.getSpinItems().size(); // góc chiếm một khoảng bn
        float offsetToCenter = segmentAngle / 2f;   // phần bù để nó chỉ đến giữa
        float angleToTarget = 360f - ((chosenItem.getItemId() * segmentAngle) + offsetToCenter); // góc quay cần thiết
        float targetDegree = angleToTarget + (5 * 360); // 5 vòng quay quán tính

        RotateAnimation rotateAnim = new RotateAnimation(
                currentDegree % 360, targetDegree,
                Animation.RELATIVE_TO_SELF, 0.5f,
                Animation.RELATIVE_TO_SELF, 0.5f
        );

        rotateAnim.setDuration(4000);
        rotateAnim.setFillAfter(true);
        rotateAnim.setInterpolator(new DecelerateInterpolator());

        rotateAnim.setAnimationListener(new Animation.AnimationListener() {
            @Override
            public void onAnimationStart(Animation animation) {}

            @Override
            public void onAnimationEnd(Animation animation) {
                currentDegree = targetDegree;
                Toast.makeText(
                    LuckySpinEventActivity.this,
                    "Đã trúng: " + rewards.get(chosenItem.getReward().getEventRewardId()).getName()
                            + " + " + chosenItem.getReward().getValue(),
                    Toast.LENGTH_SHORT).show();

                remainingSpinTime--;
                renderSpinInfo();
            }

            @Override
            public void onAnimationRepeat(Animation animation) {}
        });

        wheelBody.startAnimation(rotateAnim);
    }

    private void initViews() {
        wheelBody = findViewById(R.id.wheel_body);
        btnSpin = findViewById(R.id.btnStartSpin);
        btnBack = findViewById(R.id.btnBack);
        tvRemainingSpinTime = findViewById(R.id.tvRemainingSpinTime);
    }


}