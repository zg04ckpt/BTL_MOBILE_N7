package com.n7.quizbattle.events.presentation.activities;

import android.os.Bundle;
import android.view.animation.Animation;
import android.view.animation.DecelerateInterpolator;
import android.view.animation.RotateAnimation;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProvider;

import com.n7.quizbattle.R;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.model.SpinItemModel;
import com.n7.quizbattle.events.presentation.viewmodel.DailyEventViewModel;



public class DailyEventActivity extends AppCompatActivity {
    private DailyEventViewModel viewModel;
    private ImageView wheelBody;
    private Button btnSpin;
    private float currentDegree = 0f;
    private final int TOTAL_SEGMENTS = 8;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_daily_event);

        wheelBody = findViewById(R.id.wheel_body);
        btnSpin = findViewById(R.id.btn_spin);

        viewModel = new ViewModelProvider(this).get(DailyEventViewModel.class);
        btnSpin.setEnabled(false); // Mặc định set false khi chưa tải xong dữ liệu

        // Chạy để call các dữ lệu càn thiết
        setupObservers();

        // Thực hiện quay vòng
        btnSpin.setOnClickListener(v ->
        {
            handleSpinClick();
        });

        viewModel.loadLuckySpinEvent();
        viewModel.loadProgressEvent();
        viewModel.loadRewardMapping();
    }

    private void setupObservers() {
        // --- OBSERVER 1: XỬ LÝ TRẠNG THÁI MÀN HÌNH CHUNG ---
        viewModel.isDataReady.observe(this, state -> {
            switch (state.status) {
                case LOADING:
                    // Mạng đang tải: Khóa nút, có thể hiện ProgressBar xoay vòng
                    btnSpin.setEnabled(false);
                    btnSpin.setAlpha(0.5f);
                    break;
                case SUCCESS:
                    // Dữ liệu đã đầy đủ: Kiểm tra số lượt quay để quyết định mở khóa
                    int currentSpinTime = viewModel.getCurrentSpinTime();
                    if (currentSpinTime > 0) {
                        btnSpin.setEnabled(true);
//                        btnSpin.setAlpha(1.0f);
                    } else {
                        btnSpin.setEnabled(false);
//                        btnSpin.setAlpha(0.5f);
                        Toast.makeText(this, "Bạn đã hết lượt quay hôm nay!", Toast.LENGTH_SHORT).show();
                    }
                    break;
                case ERROR:
                    // Lỗi mạng/API: Khóa nút và báo lỗi
                    btnSpin.setEnabled(false);
                    Toast.makeText(this, "Lỗi tải sự kiện: " + state.message, Toast.LENGTH_LONG).show();
                    break;
            }
        });

        // --- OBSERVER 2: XỬ LÝ TRẠNG THÁI SAU KHI QUAY XONG VÀ GỬI API ---
        viewModel.spinActionState.observe(this, state -> {
            switch (state.status) {
                case LOADING:
                    // Đang gửi kết quả lên Server: Khóa nút tuyệt đối chống Spam
                    btnSpin.setEnabled(false);
                    Toast.makeText(this, "Đang lưu kết quả...", Toast.LENGTH_SHORT).show();
                    break;
                case SUCCESS:
                    // Gửi thành công: Không cần tự mở nút.
                    // ViewModel đã tự gọi loadProgressEvent() -> Observer 1 sẽ tự cập nhật nút!
                    Toast.makeText(this, "Lưu thành công!", Toast.LENGTH_SHORT).show();
                    break;
                case ERROR:
                    // Gửi thất bại: Trả lại quyền bấm nút để người dùng thử lại
                    btnSpin.setEnabled(true);
                    Toast.makeText(this, "Lỗi lưu kết quả: " + state.message, Toast.LENGTH_LONG).show();
                    break;
            }
        });

    }



    private void checkDataReadyToEnableButton() {
        // Hỏi ViewModel xem mọi thứ đã sẵn sàng chưa
        int total = viewModel.getTotalSpinTime();
        int current = viewModel.getCurrentSpinTime();
        boolean hasMapping = viewModel.rewardMapping.getValue() != null;

        // CHUẨN CHỈ: Chỉ mở nút khi tất cả dữ liệu đã khác -1 và mapping đã có
        if (total != -1 && current != -1 && hasMapping) {
            if (current > 0) {
                // Chp phép quay
                btnSpin.setEnabled(true);
            } else {
                btnSpin.setEnabled(false);
                Toast.makeText(this, "Bạn đã hết lượt quay hôm nay!", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private void handleSpinClick() {
        int current = viewModel.getCurrentSpinTime();

        if (current <= 0) return;


        // Lấy Item trúng thưởng từ Logic của ViewModel
        SpinItemModel chosenItem = viewModel.chooseSpinItem();
        if (chosenItem == null) {
            Toast.makeText(this, "Có lỗi xảy ra, vui lòng thử lại!", Toast.LENGTH_SHORT).show();
            return;
        }

        // Khóa nút ngay lập tức để chống spam click
        btnSpin.setEnabled(false);
        // Thực hiện Animation
        spinToSegment(chosenItem.getItemId(),chosenItem);
    }

    private void spinToSegment(int targetSegment, SpinItemModel chosenItem) {
        float segmentAngle = 360f / TOTAL_SEGMENTS; // góc chiếm một khoảng bn
        float offsetToCenter = segmentAngle / 2f;   // phần bù để nó chỉ đến giữa
        float angleToTarget = 360f - ((targetSegment * segmentAngle) + offsetToCenter); // góc quay cần thiê
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

                // Hiển thị kết quả sau khi quay xong
                showResult(chosenItem);

                int eventId = viewModel.dailyEventData.getValue().getId();
                int current = viewModel.getCurrentSpinTime();
                // Gửi kết quả quay về Server ( số lượt -1);
                viewModel.submitSpinResult(eventId,current-2);
            }

            @Override
            public void onAnimationRepeat(Animation animation) {}
        });

        wheelBody.startAnimation(rotateAnim);
    }

    private void showResult(SpinItemModel item) {
        RewardMappingModel mapping = viewModel.getRewardMapping(item.getReward().getEventRewardId());
        String name = (mapping != null) ? mapping.getName() : "quà tặng";
        String msg = "Chúc mừng! Bạn nhận được " + item.getReward().getValue() + " " + name;
        Toast.makeText(this, msg, Toast.LENGTH_LONG).show();
    }
}