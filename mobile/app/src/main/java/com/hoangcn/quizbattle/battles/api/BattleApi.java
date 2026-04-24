package com.hoangcn.quizbattle.battles.api;

import com.hoangcn.quizbattle.battles.models.MatchReviewResponse;
import com.hoangcn.quizbattle.battles.models.MatchStateResponse;
import com.hoangcn.quizbattle.battles.models.MatchInfoResponse;
import com.hoangcn.quizbattle.battles.models.MatchLoudspeakerInventoryResponse;
import com.hoangcn.quizbattle.battles.models.StartSoloMatchRequest;
import com.hoangcn.quizbattle.battles.models.SubmitMatchAnswerRequest;
import com.hoangcn.quizbattle.battles.models.UseLoudspeakerRequest;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Body;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface BattleApi {
    @GET("battles/match-info")
    Call<ApiResponse<MatchInfoResponse>> getMatchInfo();

    @GET("battles/match-info/{trackingId}")
    Call<ApiResponse<MatchInfoResponse>> getMatchInfoByTracking(@Path("trackingId") String trackingId);

    @POST("battles/solo/start")
    Call<ApiResponse<MatchInfoResponse>> startSoloMatch(@Body StartSoloMatchRequest request);

    @GET("battles/match-state")
    Call<ApiResponse<MatchStateResponse>> getMatchState();

    @GET("battles/matches/{matchId}/review")
    Call<ApiResponse<MatchReviewResponse>> getMatchReview(@Path("matchId") int matchId);

    @POST("battles/match-answer")
    Call<ApiResponse<Object>> submitMatchAnswer(@Body SubmitMatchAnswerRequest request);

    @GET("battles/match-tools/loudspeaker")
    Call<ApiResponse<MatchLoudspeakerInventoryResponse>> getLoudspeakerInventory();

    @POST("battles/match-tools/loudspeaker/use")
    Call<ApiResponse<MatchLoudspeakerInventoryResponse>> useLoudspeaker(@Body UseLoudspeakerRequest request);
}
