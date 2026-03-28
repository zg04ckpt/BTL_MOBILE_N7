package com.n7.quizbattle.events.domain.repository;

import com.n7.quizbattle.events.data.remote.dto.request.DailyTaskRequestDto;
import com.n7.quizbattle.events.domain.model.EventModel;
import com.n7.quizbattle.events.domain.model.ProgressModel;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;

import java.util.List;

public interface EventTaskRepository {
    void fetchDailyTasks(ApiCallBack<List<EventModel>> callback);

    void fetchMyProgress(ApiCallBack<List<ProgressModel>> callBack);

    void fetchMappingRewards(ApiCallBack<List<RewardMappingModel>> callback);

    void submitSpinResult(DailyTaskRequestDto requestDto, ApiCallBack<Boolean> callback);
}
