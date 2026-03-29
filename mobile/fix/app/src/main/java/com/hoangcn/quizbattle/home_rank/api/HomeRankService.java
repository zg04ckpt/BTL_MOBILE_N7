package com.hoangcn.quizbattle.home_rank.api;

import android.content.Context;

import com.hoangcn.quizbattle.home_rank.models.UserProfile;
import com.hoangcn.quizbattle.home_rank.models.UserRankDetail;
import com.hoangcn.quizbattle.home_rank.models.UserRankListItem;
import com.hoangcn.quizbattle.home_rank.models.OngoingEvent;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.api.BaseApi;

import java.util.List;

public class HomeRankService extends BaseApi {
    private final HomeRankApi api;

    public HomeRankService(Context context) {
        super(context);
        this.api = getRetrofit().create(HomeRankApi.class);
    }

    public void getOngoingEvents(ApiCallback<List<OngoingEvent>> callback) {
        enqueue(api.getOngoingEvents(), callback);
    }

    public void getProfile(ApiCallback<UserProfile> callback) {
        enqueue(api.getProfile(), callback);
    }

    public void getRankBoard(String type, ApiCallback<List<UserRankListItem>> callback) {
        enqueue(api.getRankBoard(type), callback);
    }

    public void getRankDetail(int userId, ApiCallback<UserRankDetail> callback) {
        enqueue(api.getRankDetail(userId), callback);
    }
}
