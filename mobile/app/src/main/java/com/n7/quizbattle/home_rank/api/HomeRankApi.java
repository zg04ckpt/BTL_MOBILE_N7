package com.n7.quizbattle.home_rank.api;

import com.n7.quizbattle.home_rank.api.dtos.UserProfileDto;
import com.n7.quizbattle.home_rank.api.dtos.UserRankDto;
import com.n7.quizbattle.home_rank.api.dtos.UserRankListItemDto;
import com.n7.quizbattle.shared.models.ApiResponse;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface HomeRankApi {
    @GET("users/profile")
    Call<ApiResponse<UserProfileDto>> getProfile(@Header("Authorization") String authHeader);

    @GET("rank/types")
    Call<ApiResponse<List<String>>> getRankTypes();

    @GET("rank/board")
    Call<ApiResponse<List<UserRankListItemDto>>> getRankBoard(@Query("type") String type);

    @GET("rank/{userId}")
    Call<ApiResponse<UserRankDto>> getRankDetail(@Path("userId") int userId);
}
