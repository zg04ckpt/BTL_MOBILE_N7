package com.n7.quizbattle.events.presentation.activities;

// Sửa package name chuẩn nhé

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;

// IMPORT CLASS MỚI ĐÃ SỬA
import com.n7.quizbattle.R;

import com.n7.quizbattle.events.presentation.view.LuckyWheelView;
import com.n7.quizbattle.events.presentation.view.WheelItem;

import java.util.ArrayList;
import java.util.List;

public class WheelActivity extends AppCompatActivity {

    private LuckyWheelView luckyWheelView;
    private Button btnChangeData;
    private boolean isDataToggled = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // Đảm bảo tên layout xml của bạn đúng (nên là activity_wheel)
        setContentView(R.layout.activity_wheel);

        // Gán UI và xử lý lỗi gạch đỏ R.id
        luckyWheelView = findViewById(R.id.luckyWheelView);
        btnChangeData = findViewById(R.id.btnChangeData);

        // Nạp data mặc định (6 ô)
        loadDataSet1();

        btnChangeData.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (isDataToggled) {
                    loadDataSet1();
                } else {
                    loadDataSet2();
                }
                isDataToggled = !isDataToggled;
            }
        });
    }

    // DATASET 1: Giả sử có 6 ô
    private void loadDataSet1() {
        List<WheelItem> items = new ArrayList<>();
        // Bạn ném file 'ic_coin.png', 'ic_bomb.png'... vào res/drawable trước nhé
        items.add(new WheelItem("50K", R.drawable.avatar_rank1));
        items.add(new WheelItem("Trượt", R.drawable.avatar_rank1));
        items.add(new WheelItem("Bài UNO", R.drawable.avatar_rank1));
        items.add(new WheelItem("100K", R.drawable.avatar_rank1));
        items.add(new WheelItem("Trượt", R.drawable.avatar_rank1));
        items.add(new WheelItem("200K", R.drawable.avatar_rank1));

        luckyWheelView.setData(items);
    }

    // DATASET 2: Giả sử lên 8 ô
    private void loadDataSet2() {
        List<WheelItem> items = new ArrayList<>();
        items.add(new WheelItem("UNO 1", R.drawable.avatar_rank1));
        items.add(new WheelItem("UNO 2", R.drawable.avatar_rank1));
        items.add(new WheelItem("Coin 1", R.drawable.avatar_rank1));
        items.add(new WheelItem("Coin 2", R.drawable.avatar_rank1));
        items.add(new WheelItem("Bomb 1", R.drawable.avatar_rank1));
        items.add(new WheelItem("Bomb 2", R.drawable.avatar_rank1));
        items.add(new WheelItem("Kim cương 1", R.drawable.avatar_rank1));
        items.add(new WheelItem("Kim cương 2", R.drawable.avatar_rank1));

        luckyWheelView.setData(items);
    }
}