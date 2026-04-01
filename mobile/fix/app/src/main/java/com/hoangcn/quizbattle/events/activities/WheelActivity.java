package com.hoangcn.quizbattle.events.activities;

// Sửa package name chuẩn nhé

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;

// IMPORT CLASS MỚI ĐÃ SỬA
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.views.LuckyWheelView;
import com.hoangcn.quizbattle.events.models.luckyspin.WheelItemModel;


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
        List<WheelItemModel> items = new ArrayList<>();
        // Bạn ném file 'ic_coin.png', 'ic_bomb.png'... vào res/drawable trước nhé
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel( R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel( R.drawable.avatar_rank1));

        luckyWheelView.setData(items);
    }

    // DATASET 2: Giả sử lên 8 ô
    private void loadDataSet2() {
        List<WheelItemModel> items = new ArrayList<>();
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));
        items.add(new WheelItemModel(R.drawable.avatar_rank1));

        luckyWheelView.setData(items);
    }
}