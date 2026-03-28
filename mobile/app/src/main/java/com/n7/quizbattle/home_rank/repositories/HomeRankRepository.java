package com.n7.quizbattle.home_rank.repositories;

import androidx.annotation.NonNull;

import com.n7.quizbattle.home_rank.api.HomeRankService;
import com.n7.quizbattle.home_rank.api.dtos.UserRankListItemDto;
import com.n7.quizbattle.home_rank.mappers.HomeProfileMapper;
import com.n7.quizbattle.home_rank.mappers.RankMapper;
import com.n7.quizbattle.home_rank.models.HomeProfileModel;
import com.n7.quizbattle.home_rank.models.RankBoardItemModel;
import com.n7.quizbattle.home_rank.models.RankDetailModel;
import com.n7.quizbattle.home_rank.models.RankTypeModel;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;

import java.util.ArrayList;
import java.util.List;

public class HomeRankRepository {
    private final HomeRankService service;

    public HomeRankRepository() {
        this.service = new HomeRankService();
    }

    public void getHomeProfile(@NonNull String token, @NonNull RepositoryCallback<HomeProfileModel> callback) {
        if (token.isEmpty()) {
            callback.onError("Chua co token dang nhap");
            return;
        }

        service.getProfile("Bearer " + token, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<com.n7.quizbattle.home_rank.api.dtos.UserProfileDto> data) {
                callback.onSuccess(HomeProfileMapper.fromDto(data.getData()));
            }

            @Override
            public void onError(String message) {
                callback.onError(message);
            }
        });
    }

    public void getRankTypes(@NonNull RepositoryCallback<List<RankTypeModel>> callback) {
        service.getRankTypes(new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<List<String>> data) {
                List<RankTypeModel> models = new ArrayList<>();
                if (data.getData() != null) {
                    for (String item : data.getData()) {
                        models.add(RankMapper.toTypeModel(item));
                    }
                }
                callback.onSuccess(models);
            }

            @Override
            public void onError(String message) {
                callback.onError(message);
            }
        });
    }

    public void getRankBoard(@NonNull String type, @NonNull RepositoryCallback<List<RankBoardItemModel>> callback) {
        service.getRankBoard(type, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<List<UserRankListItemDto>> data) {
                List<RankBoardItemModel> models = new ArrayList<>();
                if (data.getData() != null) {
                    for (UserRankListItemDto item : data.getData()) {
                        models.add(RankMapper.toBoardModel(item));
                    }
                }
                callback.onSuccess(models);
            }

            @Override
            public void onError(String message) {
                callback.onError(message);
            }
        });
    }

    public void getRankDetail(int userId, @NonNull RepositoryCallback<RankDetailModel> callback) {
        service.getRankDetail(userId, new ApiCallback<>() {
            @Override
            public void onSuccess(ApiResponse<com.n7.quizbattle.home_rank.api.dtos.UserRankDto> data) {
                callback.onSuccess(RankMapper.toDetailModel(data.getData()));
            }

            @Override
            public void onError(String message) {
                callback.onError(message);
            }
        });
    }
}
