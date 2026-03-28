package com.n7.quizbattle.events.data.remote;

import java.io.IOException;

import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class RetrofitClient {
    // Biến static lưu trữ instance duy nhất
    private static volatile Retrofit retrofit = null;

    public static Retrofit getClient(String baseUrl) {
        if (retrofit == null) {
            synchronized (RetrofitClient.class) {
                if (retrofit == null) {

                    // 1. Khởi tạo OkHttpClient ngay tại đây (khi thực sự cần)
                    OkHttpClient client = new OkHttpClient.Builder()
                            .addInterceptor(new Interceptor() {
                                @Override
                                public Response intercept(Chain chain) throws IOException {
                                    Request originalRequest = chain.request();

                                    // Lấy Token của user
                                    String token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9" +
                                            ".eyJuYW1laWQiOiIyIiwiZW1haWwiOiJhZG1pbkBxdWl6YmF0dGxlLmNvbSIsIm5hbWUiOi" +
                                            "JBZG1pbiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTc3NDY2NDkxMywiZXhwIjoxNzc1MzEyOTEzLCJpYXQiOjE3Nz" +
                                            "Q2NjQ5MTMsImlzcyI6IlF1aXpCYXR0bGUuQVBJIiwiYXVkIjoiUXVpekJhdHRsZS5DbGllbnQifQ" +
                                            ".GWzbl4KvW9KixBM8aHWMHju8YVoUoSXapIpzuaaF3au9_3cj_lOd6N9yyyDZp07YPu2EbEo291fUbhSYgGQ7uA";

                                    Request newRequest = originalRequest.newBuilder()
                                            .header("Authorization", "Bearer " + token)
                                            .build();

                                    return chain.proceed(newRequest);
                                }
                            }).build();

                    // 2. Gắn client vào Retrofit
                    retrofit = new Retrofit.Builder()
                            .baseUrl(baseUrl)
                            .client(client) // Lúc này client là biến cục bộ, hoàn toàn hợp lệ
                            .addConverterFactory(GsonConverterFactory.create())
                            .build();
                }
            }
        }
        return retrofit;
    }
}