package com.hoangcn.quizbattle.battles.api;

import android.content.Context;

import com.hoangcn.quizbattle.battles.models.MatchReviewResponse;
import com.hoangcn.quizbattle.battles.models.MatchStateResponse;
import com.hoangcn.quizbattle.battles.models.MatchInfoResponse;
import com.hoangcn.quizbattle.battles.models.MatchLoudspeakerInventoryResponse;
import com.hoangcn.quizbattle.battles.models.StartSoloMatchRequest;
import com.hoangcn.quizbattle.battles.models.SubmitMatchAnswerRequest;
import com.hoangcn.quizbattle.battles.models.UseLoudspeakerRequest;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.api.BaseApi;

public class BattleService extends BaseApi {
    private final BattleApi api;

    public BattleService(Context context) {
        super(context);
        this.api = getRetrofit().create(BattleApi.class);
    }

    public void getMatchState(ApiCallback<MatchStateResponse> callback) {
        enqueue(api.getMatchState(), callback);
    }

    public void getMatchInfo(ApiCallback<MatchInfoResponse> callback) {
        enqueue(api.getMatchInfo(), callback);
    }

    public void getMatchInfoByTracking(String trackingId, ApiCallback<MatchInfoResponse> callback) {
        enqueue(api.getMatchInfoByTracking(trackingId), callback);
    }

    public void startSoloMatch(StartSoloMatchRequest request, ApiCallback<MatchInfoResponse> callback) {
        enqueue(api.startSoloMatch(request), callback);
    }

    public void getMatchReview(int matchId, ApiCallback<MatchReviewResponse> callback) {
        enqueue(api.getMatchReview(matchId), callback);
    }

    public void submitMatchAnswer(SubmitMatchAnswerRequest request, ApiCallback<Object> callback) {
        enqueue(api.submitMatchAnswer(request), callback);
    }

    public void getLoudspeakerInventory(ApiCallback<MatchLoudspeakerInventoryResponse> callback) {
        enqueue(api.getLoudspeakerInventory(), callback);
    }

    public void useLoudspeaker(UseLoudspeakerRequest request, ApiCallback<MatchLoudspeakerInventoryResponse> callback) {
        enqueue(api.useLoudspeaker(request), callback);
    }
}
