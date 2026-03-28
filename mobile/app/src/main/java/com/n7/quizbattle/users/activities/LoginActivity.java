package com.n7.quizbattle.users.activities;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import android.widget.TextView;
import android.content.Intent;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.n7.quizbattle.R;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.users.api.UsersApi;
import com.n7.quizbattle.users.auth.UsersErrorMapper;
import com.n7.quizbattle.users.auth.UsersTokenManager;
import com.n7.quizbattle.users.models.LoginRequest;
import com.n7.quizbattle.users.models.LoginResponse;

public class LoginActivity extends AppCompatActivity {

    private EditText etUsername, etPassword;
    private Button btnLogin;
    private TextView tvRegisterLink;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_login);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        etUsername = findViewById(R.id.et_username);
        etPassword = findViewById(R.id.et_password);
        btnLogin = findViewById(R.id.btn_login);
        tvRegisterLink = findViewById(R.id.tv_register_link);

        // Auto skip login if token is already stored (user still signed in)
        String cachedToken = new UsersTokenManager(this).getToken();
        if (cachedToken != null && !cachedToken.isEmpty()) {
            Intent intent = new Intent(LoginActivity.this, ProfileActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
            finish();
            return;
        }

        btnLogin.setOnClickListener(v -> doLogin());

        // Click "Đăng ký ngay" -> open RegisterActivity
        tvRegisterLink.setOnClickListener(v -> {
            Intent intent = new Intent(LoginActivity.this, RegisterActivity.class);
            startActivity(intent);
        });
    }

    private void doLogin() {
        String email = etUsername.getText().toString().trim();
        String password = etPassword.getText().toString().trim();
        if (email.isEmpty() || password.isEmpty()) {
            Toast.makeText(this, "Vui lòng nhập đủ thông tin", Toast.LENGTH_SHORT).show();
            return;
        }

        btnLogin.setEnabled(false);
        btnLogin.setText("Đang đăng nhập...");

        UsersApi api = new UsersApi(LoginActivity.this);
        api.login(new LoginRequest(email, password), new ApiCallback<LoginResponse>() {
            @Override
            public void onSuccess(ApiResponse<LoginResponse> data) {
                String token = null;
                if (data != null && data.getData() != null) {
                    token = data.getData().getToken();
                }
                if (token != null && !token.isEmpty()) {
                    new UsersTokenManager(LoginActivity.this).saveToken(token);
                }

                final String finalToken = token;
                runOnUiThread(() -> {
                    btnLogin.setEnabled(true);
                    btnLogin.setText("Đăng nhập");
                    if (finalToken == null || finalToken.isEmpty()) {
                        Toast.makeText(LoginActivity.this, "Không nhận được token. Vui lòng thử lại.", Toast.LENGTH_LONG).show();
                        return;
                    }
                    Toast.makeText(LoginActivity.this, "Đăng nhập thành công", Toast.LENGTH_SHORT).show();
                    // Open ProfileActivity after successful login for testing
                    Intent intent = new Intent(LoginActivity.this, ProfileActivity.class);
                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
                    startActivity(intent);
                    finish();
                });
            }

            @Override
            public void onError(String message) {
                runOnUiThread(() -> {
                    btnLogin.setEnabled(true);
                    btnLogin.setText("Đăng nhập");
                    String display = UsersErrorMapper.map(message);
                    Toast.makeText(LoginActivity.this, display, Toast.LENGTH_LONG).show();
                });
            }
        });
    }
}