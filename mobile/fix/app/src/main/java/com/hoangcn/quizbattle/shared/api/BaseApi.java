package com.hoangcn.quizbattle.shared.api;

import android.content.Context;
import android.content.Intent;
import android.util.Log;

import androidx.annotation.NonNull;

import com.franmontiel.persistentcookiejar.PersistentCookieJar;
import com.franmontiel.persistentcookiejar.cache.SetCookieCache;
import com.franmontiel.persistentcookiejar.persistence.SharedPrefsCookiePersistor;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.users.activities.LoginActivity;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public abstract class BaseApi {
    private static final String TAG = "BaseApi";
    private final String BASE_API_URL = "https://quizbattle.hoangcn.com/api/";
    private final String BASE_IMAGE_URL = "https://quizbattle.hoangcn.com";
    private static Retrofit retrofit;

    public BaseApi(Context context) {
        this.context = context;
    }

    protected Context context;

    protected synchronized Retrofit getRetrofit() {
        if (retrofit == null) {
            var cookieJar = new PersistentCookieJar(
                new SetCookieCache(),
                new SharedPrefsCookiePersistor(context)
            );

            var client = new OkHttpClient.Builder()
                .cookieJar(cookieJar)
                .build();

            retrofit = new Retrofit.Builder()
                .baseUrl(BASE_API_URL)
                .client(client)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        }
        return retrofit;
    }

    public String getFullImageUrl(String relativeUrl) {
        return BASE_IMAGE_URL + relativeUrl;
    }

    protected <T> void enqueue(Call<ApiResponse<T>> callApi, ApiCallback<T> callback) {
        callApi.enqueue(new Callback<>() {
            @Override
            public void onResponse(Call<ApiResponse<T>> call, Response<ApiResponse<T>> response) {
                if (!response.isSuccessful()) {

                    // Nếu chưa đăng nhập thì chuyển sang activity đăng nhập
                    if (response.code() == 401) {
                        Log.d("401", "Yêu cầu đăng nhập");
                        Intent intent = new Intent(context, LoginActivity.class);
                        context.startActivity(intent);
                        return;
                    }

                    try {
                        var data = new JSONObject(response.errorBody().string());
                        callback.onError(data.getString("message"));
                    } catch (JSONException | IOException e) {
                        callback.onError(buildHttpError(response));
                    }
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