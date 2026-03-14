package com.n7.quizbattle.tests.activities;

import android.os.Bundle;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.n7.quizbattle.R;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.tests.api.TestService;
import com.n7.quizbattle.tests.models.TestModel;

public class TestActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_test);

        var api = new TestService();
        api.testApiRunning(new ApiCallback<TestModel>() {
            @Override
            public void onSuccess(ApiResponse<TestModel> data) {
                Toast.makeText(getApplicationContext(), data.getData().getName(), Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onError(String message) {
                Toast.makeText(getApplicationContext(), message, Toast.LENGTH_SHORT).show();
            }
        });
    }
}