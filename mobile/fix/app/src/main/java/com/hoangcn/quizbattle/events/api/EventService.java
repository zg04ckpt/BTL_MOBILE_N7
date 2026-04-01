package com.hoangcn.quizbattle.events.api;

import android.content.Context;

import com.hoangcn.quizbattle.events.models.ClaimRewardRequest;
import com.hoangcn.quizbattle.events.models.EventModel;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.luckyspin.WinSpinItemModel;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.api.BaseApi;

import java.util.List;

public class EventService extends BaseApi {
    private final EventApi api;

    public EventService(Context context) {
        super(context);
        this.api = getRetrofit().create(EventApi.class);
    }

    public void getAllEvents(ApiCallback<List<EventModel>> callback) {
        var request = this.api.getAllEventRewards();
        enqueue(request, callback);
    }

    public void getProgress(int eventId, ApiCallback<EventProgressModel> callback) {
        var request = this.api.getMyProgress(eventId);
        enqueue(request, callback);
    }

    public void getRewardMapping(ApiCallback<List<RewardModel>> callback) {
        var request = this.api.getAllRewardMapping();
        enqueue(request, callback);
    }

    public void getWinSpinItem(int eventId, ApiCallback<WinSpinItemModel> callback) {
        var request = this.api.getWinSpinItem(eventId);
        enqueue(request, callback);
    }

    public void claimReward(ClaimRewardRequest claimRewardRequest, ApiCallback<EventProgressModel> callback) {
        var request = this.api.claimReward(claimRewardRequest);
        enqueue(request, callback);
    }
}
