package com.n7.quizbattle.events.presentation.viewmodel;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.n7.quizbattle.events.data.repository.EventTaskRepositoryImpl;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.repository.ApiCallBack;
import com.n7.quizbattle.events.domain.repository.EventTaskRepository;

import java.util.List;

public class LimitedEventViewModel extends ViewModel {
    
    private EventTaskRepository repository = new EventTaskRepositoryImpl();

    private final MutableLiveData<List<RewardMappingModel>> _rewardMapping = new MutableLiveData<>();
    public LiveData<List<RewardMappingModel>> rewardMapping = _rewardMapping;

    private final MutableLiveData<String> _errorMessage = new MutableLiveData<>();
    public LiveData<String> errorMessage = _errorMessage;

    public void loadRewardMapping() {
        repository.fetchMappingRewards(new ApiCallBack<List<RewardMappingModel>>() {
            @Override
            public void onSuccess(List<RewardMappingModel> result) {
                _rewardMapping.setValue(result);
            }

            @Override
            public void onFail(String errorMessage) {
                _errorMessage.postValue("Không tìm thấy thông tin phần thưởng.");
            }
        });
    }
    
    public List<RewardMappingModel> getRewardMappings() {
        return _rewardMapping.getValue();
    }
}
