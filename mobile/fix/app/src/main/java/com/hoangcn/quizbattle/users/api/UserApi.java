package com.hoangcn.quizbattle.users.api;

import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.users.models.LoginRequest;
import com.hoangcn.quizbattle.users.models.LoginResponse;
import com.hoangcn.quizbattle.users.models.RegisterRequest;
import com.hoangcn.quizbattle.users.models.UserModel;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Part;

public interface UserApi {
    @POST("auth/login")
    Call<ApiResponse<LoginResponse>> login(@Body LoginRequest body);

    @POST("auth/register")
    Call<ApiResponse<LoginResponse>> register(@Body RegisterRequest body);

    @DELETE("auth/logout")
    Call<ApiResponse<Void>> logout();

    @GET("users/profile")
    Call<ApiResponse<UserModel>> getProfile();

    @Multipart
    @PUT("users/profile")
    Call<ApiResponse<UserModel>> updateProfile(@Part("name") RequestBody name,
                                               @Part MultipartBody.Part avatar);
}
