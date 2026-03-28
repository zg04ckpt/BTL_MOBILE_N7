package com.n7.quizbattle.events.presentation.activities;

import android.graphics.Color;
import android.graphics.Typeface;
import android.os.Bundle;
import android.util.TypedValue;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.button.MaterialButton;
import com.n7.quizbattle.R;
import com.n7.quizbattle.events.domain.model.SeasonalEventModel;
import com.n7.quizbattle.events.domain.model.TaskModel;

import java.util.List;

public class SeasonalEventDetailActivity extends AppCompatActivity {

    private SeasonalEventModel eventModel;
    private LinearLayout containerTasks;
    private ImageButton btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_seasonal_event_detail);

        eventModel = (SeasonalEventModel) getIntent().getSerializableExtra("SEASONAL_EVENT_DATA");

        initViews();
        displayTasks();
        initListeners();
    }

    private void initViews() {
        containerTasks = findViewById(R.id.container_tasks);
        btnBack = findViewById(R.id.btn_back);
    }

    private void initListeners() {
        findViewById(R.id.btn_play).setOnClickListener(v -> {
            Toast.makeText(this, "Bắt đầu chơi!", Toast.LENGTH_SHORT).show();
        });

        btnBack.setOnClickListener(v -> {
            finish();
        });
    }

    private void displayTasks() {
        if (eventModel == null || eventModel.getTasks() == null) return;

        List<TaskModel> tasks = eventModel.getTasks();
        containerTasks.removeAllViews();

        for (int i = 0; i < tasks.size(); i++) {
            TaskModel task = tasks.get(i);
            
            MaterialButton btnTask = new MaterialButton(this);
            btnTask.setText("Task " + (i + 1) + " : " + task.getShortDesc());
            btnTask.setAllCaps(false);
            btnTask.setTextSize(20);
            btnTask.setTypeface(null, Typeface.BOLD);
            btnTask.setTextColor(Color.WHITE);
            btnTask.setBackgroundTintList(android.content.res.ColorStateList.valueOf(Color.parseColor("#FF1744")));
            btnTask.setCornerRadius(dpToPx(20));
            
            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    dpToPx(70)
            );
            params.setMargins(0, 16, 0, 16);
            btnTask.setLayoutParams(params);
            
            containerTasks.addView(btnTask);
        }
    }

    private int dpToPx(int dp) {
        return (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, dp, getResources().getDisplayMetrics());
    }
}
