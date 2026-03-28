package com.n7.quizbattle.events.data.remote;

import com.n7.quizbattle.events.data.remote.dto.request.DailyTaskRequestDto;
import com.n7.quizbattle.events.data.remote.dto.response.EventResponseDto;
import com.n7.quizbattle.events.data.remote.dto.response.ProgressEventResponseDto;
import com.n7.quizbattle.events.data.remote.dto.RewardMappingResponseDto;
import com.n7.quizbattle.events.data.remote.dto.response.UpdateProgressDailyTaskResponseDto;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.PUT;

public interface EventApiService {
    @GET("/api/events")
    Call<EventResponseDto> getAllEventRewards();

    @GET("/api/events/users/my-progresses")
    Call<ProgressEventResponseDto> getAllMyProgress();

    @GET("/api/events/rewards/mappings")
    Call<RewardMappingResponseDto> getAllRewardMapping();

    @PUT("/api/events/my-progress")
    Call<UpdateProgressDailyTaskResponseDto> updateProgress(@Body DailyTaskRequestDto requestDto);

}
