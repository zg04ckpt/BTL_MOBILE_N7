package com.hoangcn.quizbattle;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;

import androidx.appcompat.app.AppCompatActivity;

import com.hoangcn.quizbattle.home_rank.activities.HomeActivity;
import com.hoangcn.quizbattle.shared.utils.SharedPreferenceUtil;
import com.hoangcn.quizbattle.users.activities.LoginActivity;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Đợi 2 giây rồi chuyển màn hình
        new Handler(Looper.getMainLooper()).postDelayed(() -> {
            checkLoginStatus();
        }, 2000);
    }

    private void checkLoginStatus() {
        int userId = SharedPreferenceUtil.getInstance(this).getInt("userId", -1);

        Intent intent;
        if (userId != -1) {
            // Đã đăng nhập -> Vào Home
            intent = new Intent(MainActivity.this, HomeActivity.class);
        } else {
            // Chưa đăng nhập -> Vào Login
            intent = new Intent(MainActivity.this, LoginActivity.class);
        }
        
        startActivity(intent);
        finish(); // Đóng Splash Screen để không quay lại được bằng nút Back
    }
}
