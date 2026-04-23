package com.hoangcn.quizbattle.home_rank.api;

import com.hoangcn.quizbattle.home_rank.models.UserProfile;
import com.hoangcn.quizbattle.home_rank.models.UserRankDetail;
import com.hoangcn.quizbattle.home_rank.models.UserRankListItem;
import com.hoangcn.quizbattle.home_rank.models.OngoingEvent;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface HomeRankApi {
    @GET("users/profile")
    Call<ApiResponse<UserProfile>> getProfile();

    @GET("events/ongoing")
    Call<ApiResponse<List<OngoingEvent>>> getOngoingEvents();

    @GET("rank/board")
    Call<ApiResponse<List<UserRankListItem>>> getRankBoard(@Query("type") String type);

    @GET("rank/{userId}")
    Call<ApiResponse<UserRankDetail>> getRankDetail(@Path("userId") int userId);
}
