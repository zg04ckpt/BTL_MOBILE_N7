package com.hoangcn.quizbattle.users.api;

import android.content.Context;

import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.api.BaseApi;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.users.models.LoginRequest;
import com.hoangcn.quizbattle.users.models.LoginResponse;
import com.hoangcn.quizbattle.users.models.RegisterRequest;
import com.hoangcn.quizbattle.users.models.UserModel;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;

public class UserService extends BaseApi {

    private final UserApi api;
    public UserService(Context context) {
        super(context);
        api = getRetrofit().create(UserApi.class);
    }

    public void login(LoginRequest req, ApiCallback<LoginResponse> callback) {
        Call<ApiResponse<LoginResponse>> call = api.login(req);
        enqueue(call, callback);
    }

    public void register(RegisterRequest req, ApiCallback<LoginResponse> callback) {
        Call<ApiResponse<LoginResponse>> call = api.register(req);
        enqueue(call, callback);
    }

    public void logout(ApiCallback<Void> callback) {
        Call<ApiResponse<Void>> call = api.logout();
    }

    public void getProfile(ApiCallback<UserModel> callback) {
        Call<ApiResponse<UserModel>> call = api.getProfile();
        enqueue(call, callback);
    }

    public void updateProfile(RequestBody name, MultipartBody.Part avatar, ApiCallback<UserModel> callback) {
        Call<ApiResponse<UserModel>> call = api.updateProfile(name, avatar);
        enqueue(call, callback);
    }
}
