package com.n7.quizbattle.home_rank.api;

import com.n7.quizbattle.home_rank.api.dtos.UserProfileDto;
import com.n7.quizbattle.home_rank.api.dtos.UserRankDto;
import com.n7.quizbattle.home_rank.api.dtos.UserRankListItemDto;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.api.BaseApi;

import java.util.List;

public class HomeRankService extends BaseApi {
    private final HomeRankApi api;

    public HomeRankService() {
        this.api = getRetrofit().create(HomeRankApi.class);
    }

    public void getProfile(String authHeader, ApiCallback<UserProfileDto> callback) {
        enqueue(api.getProfile(authHeader), callback);
    }

    public void getRankTypes(ApiCallback<List<String>> callback) {
        enqueue(api.getRankTypes(), callback);
    }

    public void getRankBoard(String type, ApiCallback<List<UserRankListItemDto>> callback) {
        enqueue(api.getRankBoard(type), callback);
    }

    public void getRankDetail(int userId, ApiCallback<UserRankDto> callback) {
        enqueue(api.getRankDetail(userId), callback);
    }
}
