package com.n7.quizbattle.events.presentation.viewmodel;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MediatorLiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.n7.quizbattle.events.data.repository.EventTaskRepositoryImpl;
import com.n7.quizbattle.events.domain.model.DailyEventModel;
import com.n7.quizbattle.events.domain.model.EventModel;
import com.n7.quizbattle.events.domain.model.LimitedEventModel;
import com.n7.quizbattle.events.domain.model.LuckySpinModel;
import com.n7.quizbattle.events.domain.model.SeasonalEventModel;
import com.n7.quizbattle.events.domain.repository.ApiCallBack;
import com.n7.quizbattle.events.domain.repository.EventTaskRepository;
import com.n7.quizbattle.events.presentation.state.UiState;

import java.util.List;

public class HomeEventModel extends ViewModel {

    private EventTaskRepository repository = new EventTaskRepositoryImpl();

    private final MutableLiveData<String> _errorMessage = new MutableLiveData<>();
    private final LiveData<String> errorMessage =  _errorMessage;

    private MutableLiveData<DailyEventModel> _dailyEventData = new MutableLiveData<>();
    public LiveData<DailyEventModel> dailyEventData = _dailyEventData;

    private MutableLiveData<SeasonalEventModel> _seasonalEventData = new MutableLiveData<>();
    public LiveData<SeasonalEventModel> seasonalEventData = _seasonalEventData;

    private MutableLiveData<LimitedEventModel> _limitedEventData = new MutableLiveData<>();
    public LiveData<LimitedEventModel> limitedEventData = _limitedEventData;


    // State quản lý.
    private final MediatorLiveData<UiState<Boolean>> _isDataReady = new MediatorLiveData<>();
    public LiveData<UiState<Boolean>> isDataReady = _isDataReady;

    public HomeEventModel() {
        _isDataReady.setValue(UiState.loading());

        // Lắng nghe các nguồn dữ liệu thành công
        _isDataReady.addSource(_limitedEventData, data -> checkAllDataLoaded());
        _isDataReady.addSource(_seasonalEventData, data -> checkAllDataLoaded());
        _isDataReady.addSource(_dailyEventData, data -> checkAllDataLoaded());

        // Lắng nghe lỗi tổng hợp
        _isDataReady.addSource(_errorMessage, errorMsg -> {
            if (errorMsg != null) {
                _isDataReady.setValue(UiState.error(errorMsg));
            }
        });
    }

    private void checkAllDataLoaded() {
        // Nếu đang lỗi thì không check tiếp
        if (_isDataReady.getValue() != null && _isDataReady.getValue().status == UiState.Status.ERROR) {
            return;
        }
        boolean isLimitedOk = _limitedEventData.getValue() != null;
        boolean isSeasonalOk = _seasonalEventData.getValue() != null;
        boolean isDailyEventOk = _dailyEventData.getValue() != null;

        if (isLimitedOk && isSeasonalOk && isDailyEventOk) {
            _isDataReady.setValue(UiState.success(true));
        } else {
            _isDataReady.setValue(UiState.loading());
        }
    }
    public void loadLuckySpinEvent() {
        repository.fetchDailyTasks(new ApiCallBack<List<EventModel>>() {
            @Override
            public void onSuccess(List<EventModel> result) {
                for (EventModel event : result) {
                    if ("LuckySpin".equals(event.getType()) && event.getLuckySpin() != null) {
                        // Day them DailyEventModel
                        DailyEventModel dailyEventModel = new DailyEventModel();

                        dailyEventModel.setId(event.getId());
                        dailyEventModel.setDesc(event.getDesc());
                        dailyEventModel.setStartTime(event.getStartTime());
                        dailyEventModel.setEndTime(event.getEndTime());
                        dailyEventModel.setType(event.getType());
                        dailyEventModel.setTimeType(event.getTimeType());
                        dailyEventModel.setLocked(event.isLocked());
                        dailyEventModel.setLuckySpin(event.getLuckySpin());

                        _dailyEventData.setValue(dailyEventModel);

                    }else if ("QuizMilestoneChallenge".equals(event.getType()) && event.getThresholds() != null) {
                        // Day them DailyEventModel
                        LimitedEventModel limitedEventModel = new LimitedEventModel();

                        limitedEventModel.setId(event.getId());
                        limitedEventModel.setDesc(event.getDesc());
                        limitedEventModel.setStartTime(event.getStartTime());
                        limitedEventModel.setEndTime(event.getEndTime());
                        limitedEventModel.setType(event.getType());
                        limitedEventModel.setTimeType(event.getTimeType());
                        limitedEventModel.setLocked(event.isLocked());
                        limitedEventModel.setThresholds(event.getThresholds());

                        _limitedEventData.setValue(limitedEventModel);
                    }else if ("TournamentRewards".equals(event.getType()) && event.getTasks() != null) {
                        // Day them DailyEventModel
                       SeasonalEventModel seasonalEventModel = new SeasonalEventModel();

                        seasonalEventModel.setId(event.getId());
                        seasonalEventModel.setDesc(event.getDesc());
                        seasonalEventModel.setStartTime(event.getStartTime());
                        seasonalEventModel.setEndTime(event.getEndTime());
                        seasonalEventModel.setType(event.getType());
                        seasonalEventModel.setTimeType(event.getTimeType());
                        seasonalEventModel.setLocked(event.isLocked());
                        seasonalEventModel.setTasks(event.getTasks());

                        _seasonalEventData.setValue(seasonalEventModel);
                    }else {
                        // some event here
                    }
                }

            }
            @Override
            public void onFail(String errorMessage) {
                /* Handle error */
                _errorMessage.postValue(errorMessage);
            }
        });
    }

    public DailyEventModel getDailyEvent() {
        return _dailyEventData.getValue();
    }

    public SeasonalEventModel getSeasonalEvent() {
        return _seasonalEventData.getValue();
    }

    public LimitedEventModel getLimitedEvent() {
        return _limitedEventData.getValue();
    }


}
