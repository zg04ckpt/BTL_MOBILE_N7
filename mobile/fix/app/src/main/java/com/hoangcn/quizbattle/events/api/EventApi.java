package com.hoangcn.quizbattle.events.api;

import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.luckyspin.WinSpinItemModel;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface EventApi {
    @GET("events")
    Call<ApiResponse<List<EventModel>>> getAllEventRewards();

    @GET("events/{eventId}/spin")
    Call<ApiResponse<WinSpinItemModel>> getWinSpinItem(@Path("eventId") int eventId);

    @GET("events/users/my-progresses")
    Call<ApiResponse<List<EventProgressModel>>> getAllMyProgress();

    @GET("events/rewards/mappings")
    Call<ApiResponse<List<RewardModel>>> getAllRewardMapping();
}
