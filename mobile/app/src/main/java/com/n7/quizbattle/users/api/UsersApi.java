package com.n7.quizbattle.users.api;

import android.content.Context;
import android.util.Log;

import androidx.annotation.NonNull;

import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.api.BaseApi;
import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.users.auth.TokenManager;
import com.n7.quizbattle.users.models.LoginRequest;
import com.n7.quizbattle.users.models.LoginResponse;
import com.n7.quizbattle.users.models.RegisterRequest;
import com.n7.quizbattle.users.models.UserModel;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Response;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Part;

public class UsersApi extends BaseApi {

    private interface Api {
        @POST("auth/login")
        Call<ApiResponse<LoginResponse>> login(@Body LoginRequest body);

        @POST("auth/register")
        Call<ApiResponse<LoginResponse>> register(@Body RegisterRequest body);

        @GET("users/profile")
        Call<ApiResponse<UserModel>> getProfile(@Header("Authorization") String auth);

        @Multipart
        @PUT("users/profile")
        Call<ApiResponse<UserModel>> updateProfile(@Header("Authorization") String auth,
                                                  @Part("name") RequestBody name,
                                                  @Part MultipartBody.Part avatar);
    }

    private final Api api;
    private final Context ctx;

    public UsersApi() {
        this(null);
    }

    public UsersApi(Context ctx) {
        this.ctx = ctx;
        api = getRetrofit().create(Api.class);
    }

    public void login(LoginRequest req, ApiCallback<LoginResponse> callback) {
        Call<ApiResponse<LoginResponse>> call = api.login(req);
        call.enqueue(new retrofit2.Callback<ApiResponse<LoginResponse>>() {
            @Override
            public void onResponse(Call<ApiResponse<LoginResponse>> call, retrofit2.Response<ApiResponse<LoginResponse>> response) {
                if (!response.isSuccessful()) {
                    callback.onError(buildHttpError(response));
                    return;
                }
                ApiResponse<LoginResponse> body = response.body();
                if (body == null) {
                    callback.onError("Phan hoi rong tu server");
                    return;
                }
                if (!body.isSuccess()) {
                    String message = body.getMessage() == null || body.getMessage().isEmpty()
                            ? "Yeu cau that bai" : body.getMessage();
                    callback.onError(message);
                    return;
                }
                LoginResponse data = body.getData();
                Log.d("UsersApi", "login response token=" + (data == null ? "null data" : data.getToken()));
                callback.onSuccess(body);
            }

            @Override
            public void onFailure(Call<ApiResponse<LoginResponse>> call, Throwable t) {
                String errorMsg = "Loi goi API: " + (t.getLocalizedMessage() == null ? "Unknown error" : t.getLocalizedMessage());
                Log.e("UsersApi", errorMsg, t);
                callback.onError(errorMsg);
            }
        });
    }

    public void register(RegisterRequest req, ApiCallback<LoginResponse> callback) {
        Call<ApiResponse<LoginResponse>> call = api.register(req);
        enqueue(call, callback);
    }

    public void getProfile(ApiCallback<UserModel> callback) {
        String auth = null;
        try {
            if (ctx != null) {
                TokenManager.init(ctx);
                auth = TokenManager.getInstance().getAuthHeader();
            }
        } catch (Exception ignored) {
        }

        Log.d("UsersApi", "getProfile auth is null? " + (auth == null) + ", value=" + auth);

        if (auth == null || auth.isEmpty()) {
            if (callback != null) {
                callback.onError("Bạn chưa đăng nhập hoặc token đã hết hạn. Vui lòng đăng nhập lại.");
            }
            Log.w("UsersApi", "Missing auth header for getProfile");
            return;
        }

        Call<ApiResponse<UserModel>> call = api.getProfile(auth);
        enqueue(call, callback);
    }

    // updateProfile wrapper left as an example for future use
    public void updateProfile(RequestBody name, MultipartBody.Part avatar, ApiCallback<UserModel> callback) {
        String auth = null;
        try {
            if (ctx != null) {
                TokenManager.init(ctx);
                auth = TokenManager.getInstance().getAuthHeader();
            }
        } catch (Exception ignored) {
        }
        Call<ApiResponse<UserModel>> call = api.updateProfile(auth, name, avatar);
        enqueue(call, callback);
    }

    // Local copy because BaseApi.buildHttpError is private
    private String buildHttpError(@NonNull Response<?> response) {
        String errorMsg = "Loi " + response.code();
        try {
            if (response.errorBody() != null) {
                errorMsg += " - " + response.errorBody().string();
            }
        } catch (Exception e) {
            Log.e("UsersApi", "Khong doc duoc error body", e);
        }
        return errorMsg;
    }
}
