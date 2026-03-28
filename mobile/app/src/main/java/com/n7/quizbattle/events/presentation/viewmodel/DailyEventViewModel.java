package com.n7.quizbattle.events.presentation.viewmodel;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MediatorLiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.n7.quizbattle.events.data.remote.dto.request.DailyTaskRequestDto;
import com.n7.quizbattle.events.data.repository.EventTaskRepositoryImpl;
import com.n7.quizbattle.events.domain.model.DailyEventModel;
import com.n7.quizbattle.events.domain.model.EventModel;
import com.n7.quizbattle.events.domain.model.LuckySpinModel;
import com.n7.quizbattle.events.domain.model.ProgressModel;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.model.SpinItemModel;
import com.n7.quizbattle.events.domain.repository.ApiCallBack;
import com.n7.quizbattle.events.domain.repository.EventTaskRepository;
import com.n7.quizbattle.events.presentation.state.UiState;

import java.util.List;
import java.util.Random;

public class DailyEventViewModel extends ViewModel {

    private EventTaskRepository repository = new EventTaskRepositoryImpl();

    private final MutableLiveData<String> _errorMessage = new MutableLiveData<>();
    private final MutableLiveData<LuckySpinModel> _luckySpinData = new MutableLiveData<>();
    private final MutableLiveData<List<ProgressModel>> _myProgress = new MutableLiveData<>();
    private final MutableLiveData<List<RewardMappingModel>> _rewardMapping = new MutableLiveData<>();
    private final MutableLiveData<DailyEventModel> _dailyEventData = new MutableLiveData<>();

    // State choq quản lý dữ liệu
    private final MediatorLiveData<UiState<Boolean>> _isDataReady = new MediatorLiveData<>();
    public LiveData<UiState<Boolean>> isDataReady = _isDataReady;

    private final MutableLiveData<UiState<Boolean>> _spinActionState = new MutableLiveData<>();
    public LiveData<UiState<Boolean>> spinActionState = _spinActionState;

    // các dữ liệu public
    public LiveData<DailyEventModel> dailyEventData = _dailyEventData;
    public LiveData<String> errorMessage = _errorMessage;
    public LiveData<LuckySpinModel> luckySpinData = _luckySpinData;

    public LiveData<List<ProgressModel>> myProgress = _myProgress;
    public LiveData<List<RewardMappingModel>> rewardMapping = _rewardMapping;


    public DailyEventViewModel() {
        // Mặc định khởi tạo là Loading
        _isDataReady.setValue(UiState.loading());

        // Lắng nghe các nguồn dữ liệu thành công
        _isDataReady.addSource(_luckySpinData, data -> checkAllDataLoaded());
        _isDataReady.addSource(_myProgress, data -> checkAllDataLoaded());
        _isDataReady.addSource(_rewardMapping, data -> checkAllDataLoaded());
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

        boolean isSpinOk = _luckySpinData.getValue() != null;
        boolean isProgressOk = _myProgress.getValue() != null;
        boolean isMappingOk = _rewardMapping.getValue() != null;
        boolean isDailyEventOk = _dailyEventData.getValue() != null;

        if (isSpinOk && isProgressOk && isMappingOk && isDailyEventOk) {
            _isDataReady.setValue(UiState.success(true));
        } else {
            _isDataReady.setValue(UiState.loading());
        }
    }


    public void loadLuckySpinEvent() {
        repository.fetchDailyTasks(new ApiCallBack<List<EventModel>>() {
            @Override
            public void onSuccess(List<EventModel> result) {
                boolean found = false;

                for (EventModel event : result) {
                    if ("LuckySpin".equals(event.getType()) && event.getLuckySpin() != null) {
                        _luckySpinData.postValue(event.getLuckySpin());

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
                        found = true;
                        break;
                    }
                }
                if (!found) _errorMessage.postValue("Không tìm thấy sự kiện Vòng Quay.");
            }
            @Override
            public void onFail(String errorMessage) {
                /* Handle error */
                _errorMessage.postValue(errorMessage);
            }
        });
    }

    public void loadProgressEvent() {
        repository.fetchMyProgress(new ApiCallBack<List<ProgressModel>>() {
            @Override
            public void onSuccess(List<ProgressModel> result) {
                // Chuyen du lieu cho activity xử lý.
                _myProgress.setValue(result);
            }

            @Override
            public void onFail(String errorMessage) {
                _errorMessage.postValue("Không tìm thấy thông tin của người dùng.");
            }
        });
    }

    public void loadRewardMapping() {
        repository.fetchMappingRewards(new ApiCallBack<List<RewardMappingModel>>() {

            @Override
            public void onSuccess(List<RewardMappingModel> result) {
                _rewardMapping.setValue(result);
            }

            @Override
            public void onFail(String errorMessage) {
                _errorMessage.postValue("Không tìm thấy thông tin của người dùng.");
            }
        });
    }

    public void submitSpinResult(int eventId, int progressJsonData) {
        // BƯỚC 1: KHÓA MÀN HÌNH NGAY LẬP TỨC
        _spinActionState.setValue(UiState.loading());

        // Tạo object dữ liệu để gửi đi
        DailyTaskRequestDto request = new DailyTaskRequestDto();
        request.setEventId(String.valueOf(eventId));
        request.setProgressJsonData(String.valueOf(progressJsonData));

        repository.submitSpinResult(request, new ApiCallBack<Boolean>() {
            @Override
            public void onSuccess(Boolean result) {
                // Khi POST thành công, ta nên load lại Progress để cập nhật số lượt quay mới nhất
                loadProgressEvent();

                // BƯỚC 2: BÁO THÀNH CÔNG
                _spinActionState.postValue(UiState.success(true));
            }

            @Override
            public void onFail(String errorMessage) {
                _errorMessage.setValue(errorMessage);
            }
        });
    }


//    CÁC HÀM Ở DƯỚI GIÚP ACTIVITY LẤY DỮ LIỆU VÀ HIỂN THỊ

    /*
    *  Nếu dữ liệu chưa được tải hoặc lỗi hàm sẽ mặc định trả về giá trị -1.
    *  Trong activites trước khi lấy dữ liệu phải tiến hành validate lại dữ liệu
    *  Trả về số lượt quay hiện tại còn lại
    */
    public int getCurrentSpinTime() {
        if (_dailyEventData.getValue() == null || _myProgress.getValue() == null) return -1;
        int eventId = _dailyEventData.getValue().getId();
        // Lấy ra list progress của DailyTask
        List<ProgressModel> progressModels = _myProgress.getValue();

        for(ProgressModel progressModel : progressModels) {
            if(progressModel.getEventId() == eventId) {
                return progressModel.getTodaySpinTime();
            }

        }
        // Nếu duyệt hết vòng chưa thấy
        return getTotalSpinTime();
    }

    /*
     *  Nếu dữ liệu chưa được tải hoặc lỗi hàm sẽ mặc định trả về giá trị -1.
     *  Trong activites trước khi lấy dữ liệu phải tiến hành validate lại dữ liệu
     *  Trả về số lượt quay mỗi ngày.
     */
    public int getTotalSpinTime() {
        LuckySpinModel model = _luckySpinData.getValue();
        if(model == null) {
            return -1; // Dữ liệu chưa tải xong.
        }
        return model.getMaxSpinTimePerDay();

    }

    /*
     *  Nếu dữ liệu chưa được tải hoặc lỗi hàm sẽ mặc định trả về giá trị null.
     *  Trong activites trước khi lấy dữ liệu phải tiến hành validate lại dữ liệu. Tránh bị Exception NullPointer
     *  Trả về Item mà người dùng chọn để quay.
     *  Thuật toán được chọn là chọn từ 1 -> 100 nếu trúng số nào ở tỉ lệ nào thì chọn ra item đó
     */
    public SpinItemModel chooseSpinItem() {
        LuckySpinModel model = _luckySpinData.getValue();
        if(model == null) {
            return null;
        }
        List<SpinItemModel> spinItems = model.getSpinItems();

        int totalWeight = 0;
        for(SpinItemModel item : spinItems) {
            totalWeight += item.getRate();
        }

        Random random = new Random();
        int randomNumber = random.nextInt(totalWeight);

        int currentSum = 0;
        for (SpinItemModel item : spinItems) {
            currentSum += item.getRate();
            if (randomNumber < currentSum) {
                return item; // Trúng ô này!
            }
        }
        // nếu lỗi trả về phần từ đầu tiên
        return spinItems.get(0);
    }

    /*
     *  Nếu dữ liệu chưa được tải hoặc lỗi hàm sẽ mặc định trả về giá trị null.
     *  Trong activites trước khi lấy dữ liệu phải tiến hành validate lại dữ liệu. Tránh bị Exception NullPointer
     *  Trả về item có id là rewardId
     */
    public RewardMappingModel getRewardMapping(int rewardId) {
        List<RewardMappingModel> models = _rewardMapping.getValue();

        if(models == null) {
            return null;
        }

        for(RewardMappingModel model : models) {
            if(model.getId() == rewardId) {
                return model;
            }
        }

        // Nếu không tìm thấy trả về null
        return null;
    }

}
