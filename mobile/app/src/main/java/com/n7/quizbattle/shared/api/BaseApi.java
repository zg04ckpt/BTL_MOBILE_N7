package com.n7.quizbattle.shared.api;

import android.util.Log;

import androidx.annotation.NonNull;

import com.n7.quizbattle.shared.models.ApiResponse;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public abstract class BaseApi {
    private static final String TAG = "BaseApi";
    private static Retrofit retrofit;

    protected static synchronized Retrofit getRetrofit() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl("https://quizbattle.hoangcn.com/api/")
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }

    protected <T> void enqueue(@NonNull Call<ApiResponse<T>> call, @NonNull final ApiCallback<T> callback) {
        call.enqueue(new Callback<ApiResponse<T>>() {
            @Override
            public void onResponse(@NonNull Call<ApiResponse<T>> call, @NonNull Response<ApiResponse<T>> response) {
                if (!response.isSuccessful()) {
                    callback.onError(buildHttpError(response));
                    return;
                }

                ApiResponse<T> body = response.body();
                if (body == null) {
                    callback.onError("Phan hoi rong tu server");
                    return;
                }

                if (!body.isSuccess()) {
                    String message = body.getMessage() == null || body.getMessage().isEmpty()
                            ? "Yeu cau that bai"
                            : body.getMessage();
                    callback.onError(message);
                    return;
                }

                callback.onSuccess(body);
            }

            @Override
            public void onFailure(@NonNull Call<ApiResponse<T>> call, @NonNull Throwable t) {
                String errorMsg = "Loi goi API: " + (t.getLocalizedMessage() == null ? "Unknown error" : t.getLocalizedMessage());
                Log.e(TAG, errorMsg, t);
                callback.onError(errorMsg);
            }
        });
    }

    private String buildHttpError(@NonNull Response<?> response) {
        String errorMsg = "Loi " + response.code();
        try {
            if (response.errorBody() != null) {
                errorMsg += " - " + response.errorBody().string();
            }
        } catch (Exception e) {
            Log.e(TAG, "Khong doc duoc error body", e);
        }
        return errorMsg;
    }
}