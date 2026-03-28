package com.n7.quizbattle.events.data.repository;

import com.n7.quizbattle.events.data.remote.EventApiService;
import com.n7.quizbattle.events.data.remote.RetrofitClient;
import com.n7.quizbattle.events.data.remote.dto.EventItemDto;
import com.n7.quizbattle.events.data.remote.dto.request.DailyTaskRequestDto;
import com.n7.quizbattle.events.data.remote.dto.response.EventResponseDto;
import com.n7.quizbattle.events.data.remote.dto.response.ProgressEventResponseDto;
import com.n7.quizbattle.events.data.remote.dto.ProgressModelDto;
import com.n7.quizbattle.events.data.remote.dto.RewardDto;
import com.n7.quizbattle.events.data.remote.dto.RewardMappingDto;
import com.n7.quizbattle.events.data.remote.dto.RewardMappingResponseDto;
import com.n7.quizbattle.events.data.remote.dto.SpinItemDto;
import com.n7.quizbattle.events.data.remote.dto.TaskModelDto;
import com.n7.quizbattle.events.data.remote.dto.TaskProgressModelDto;
import com.n7.quizbattle.events.data.remote.dto.ThresholdModelDto;
import com.n7.quizbattle.events.data.remote.dto.response.UpdateProgressDailyTaskResponseDto;
import com.n7.quizbattle.events.domain.model.EventModel;
import com.n7.quizbattle.events.domain.model.LuckySpinModel;
import com.n7.quizbattle.events.domain.model.ProgressModel;
import com.n7.quizbattle.events.domain.model.Reward;
import com.n7.quizbattle.events.domain.model.RewardMappingModel;
import com.n7.quizbattle.events.domain.model.SpinItemModel;
import com.n7.quizbattle.events.domain.model.TaskModel;
import com.n7.quizbattle.events.domain.model.TaskProgressModel;
import com.n7.quizbattle.events.domain.model.ThresholdModel;
import com.n7.quizbattle.events.domain.repository.ApiCallBack;
import com.n7.quizbattle.events.domain.repository.EventTaskRepository;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EventTaskRepositoryImpl implements EventTaskRepository {

    private final String BASE_URL = "https://quizbattle.hoangcn.com/";
    private final EventApiService eventApiService;

    public EventTaskRepositoryImpl() {
        this.eventApiService = RetrofitClient.getClient(BASE_URL).create(EventApiService.class);
    }

    @Override
    public void fetchDailyTasks(ApiCallBack<List<EventModel>> callback) {
        eventApiService.getAllEventRewards().enqueue(new Callback<EventResponseDto>() {
            @Override
            public void onResponse(Call<EventResponseDto> call, Response<EventResponseDto> response) {
                // nếu nhận được response
                if (response.isSuccessful() && response.body() != null) {
                    List<EventModel> models = new ArrayList<>();
                    for (EventItemDto dto : response.body().data) {
                        // Mapping logic
                        EventModel eventModel = new EventModel();

                        // Mỗi Event khác nhau sẽ có các sự kiện khác nhau như LuckySpin, QuizMilestoneChallenge, TournamentRewards,....
                        LuckySpinModel spinModel = new LuckySpinModel();
                        List<ThresholdModel> thresholdModels = new ArrayList<>();
                        List<TaskModel> taskModels = new ArrayList<>();
                        if ("LuckySpin".equals(dto.getType())) {
                            // map các trường basic
                            eventModel.setId(dto.getId());
                            eventModel.setName(dto.getName());
                            eventModel.setDesc(dto.getDesc());
                            eventModel.setStartTime(dto.getStartTime());
                            eventModel.setEndTime(dto.getEndTime());
                            eventModel.setTimeType(dto.getTimeType());
                            eventModel.setType(dto.getType());
                            eventModel.setLocked(dto.isLocked());

                            // map các trường cho sự kiện LuckySpin
                            spinModel.setMaxSpinTimePerDay(dto.maxSpinTimePerDay);

                            List<SpinItemModel> spinItems = new ArrayList<>();
                            for (SpinItemDto itemDto : dto.spinItems) {
                                SpinItemModel spinItemModel = new SpinItemModel();

                                spinItemModel.setItemId(itemDto.itemId);
                                spinItemModel.setRate(itemDto.rate);
                                // map reward
                                Reward reward = new Reward();
                                RewardDto itemRewardDto = itemDto.reward;
                                reward.setEventRewardId(itemRewardDto.getEventRewardId());
                                reward.setValue(itemRewardDto.getValue());

                                spinItemModel.setReward(reward);
                                spinItems.add(spinItemModel);
                            }
                            spinModel.setSpinItems(spinItems);

                            eventModel.setLuckySpin(spinModel);
                            models.add(eventModel);
                        }else if("QuizMilestoneChallenge".equals(dto.getType())) {
                            // map các trường basic
                            eventModel.setId(dto.getId());
                            eventModel.setName(dto.getName());
                            eventModel.setDesc(dto.getDesc());
                            eventModel.setStartTime(dto.getStartTime());
                            eventModel.setEndTime(dto.getEndTime());
                            eventModel.setTimeType(dto.getTimeType());
                            eventModel.setType(dto.getType());
                            eventModel.setLocked(dto.isLocked());

                            // map các trường cho sự kiện QuizMilestoneChallenge
                            for(ThresholdModelDto thresholdModelDto : dto.getThresholds()) {
                                ThresholdModel thresholdModel = new ThresholdModel();

                                thresholdModel.setThresholdId(thresholdModelDto.getThresholdId());
                                thresholdModel.setExpScoreGained(thresholdModelDto.getExpScoreGained());
                                thresholdModel.setChallengeQuestionIds(thresholdModelDto.getChallengeQuestionIds());

                                // map for reward
                                List<Reward> rewards = new ArrayList<>();
                                for(RewardDto rewardDto : thresholdModelDto.getRewards()) {
                                    Reward reward = new Reward();

                                    reward.setValue(rewardDto.getValue());
                                    reward.setEventRewardId(rewardDto.getEventRewardId());

                                    rewards.add(reward);
                                }
                                thresholdModel.setRewards(rewards);
                                thresholdModels.add(thresholdModel);
                            }
                            eventModel.setThresholds(thresholdModels);
                            models.add(eventModel);
                        }else if("TournamentRewards".equals(dto.getType())) {
                            // map các trường basic
                            eventModel.setId(dto.getId());
                            eventModel.setName(dto.getName());
                            eventModel.setDesc(dto.getDesc());
                            eventModel.setStartTime(dto.getStartTime());
                            eventModel.setEndTime(dto.getEndTime());
                            eventModel.setTimeType(dto.getTimeType());
                            eventModel.setType(dto.getType());
                            eventModel.setLocked(dto.isLocked());

                            // map các event cho sự kiện TournamentRewards
                            for(TaskModelDto dtoTask : dto.getTasks()) {
                                TaskModel taskModel = new TaskModel();
                                taskModel.setTaskId(dtoTask.getTaskId());
                                taskModel.setShortDesc(dtoTask.getShortDesc());
                                taskModel.setType(dtoTask.getType());
                                taskModel.setNumberOfMatchs(dtoTask.getNumberOfMatchs());

                                List<Reward> rewards = new ArrayList<>();
                                for(RewardDto rewardDto : dtoTask.getRewards()) {
                                    Reward reward = new Reward();
                                    reward.setEventRewardId(rewardDto.getEventRewardId());
                                    reward.setValue(rewardDto.getValue());
                                    rewards.add(reward);
                                }
                                taskModel.setRewards(rewards);
                                taskModels.add(taskModel);
                            }
                            eventModel.setTasks(taskModels);
                            models.add(eventModel);
                        }else {
                            // Các sự kiện khác có thể xuất hiện thêm ở đây
                        }
                    }
                    callback.onSuccess(models);
                }
            }
            @Override
            public void onFailure(Call<EventResponseDto> call, Throwable t) {
                callback.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void fetchMyProgress(ApiCallBack<List<ProgressModel>> callback) {
        eventApiService.getAllMyProgress().enqueue(new Callback<ProgressEventResponseDto>() {
            @Override
            public void onResponse(Call<ProgressEventResponseDto> call, Response<ProgressEventResponseDto> response) {
                // nếu nhận được response
                if(response.isSuccessful() && response.body() != null) {
                    List<ProgressModel> models = new ArrayList<>();
                    for(ProgressModelDto dto : response.body().getData()) {
                        ProgressModel model = new ProgressModel();
                        model.setEventId(dto.getEventId());
                        model.setLastChanged(dto.getLastChanged());
                        model.setTodaySpinTime(dto.getTodaySpinTime());

                        // map cho TaskProgressModel từ TaskProgressModelDto
                        List<TaskProgressModel> taskProgressModels = new ArrayList<>();
                        if(dto.getTaskProgresses() != null)  {
                            for(TaskProgressModelDto dtoTask : dto.getTaskProgresses()) {
                                TaskProgressModel taskProgressModel = new TaskProgressModel();

                                taskProgressModel.setTaskId(dtoTask.getTaskId());
                                taskProgressModel.setCompleted(dtoTask.isCompleted());
                                taskProgressModel.setRewardClaimed(dtoTask.isRewardClaimed());

                                taskProgressModels.add(taskProgressModel);
                            }
                        }

                        model.setTaskProgresses(taskProgressModels);
                        models.add(model);
                    }
                    callback.onSuccess(models);
                }else {
                    callback.onFail("Không tìm thấy thông tin của người dùng.");
                }
            }

            @Override
            public void onFailure(Call<ProgressEventResponseDto> call, Throwable t) {
                callback.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void fetchMappingRewards(ApiCallBack<List<RewardMappingModel>> callback) {
        eventApiService.getAllRewardMapping().enqueue(new Callback<RewardMappingResponseDto>() {
            @Override
            public void onResponse(Call<RewardMappingResponseDto> call, Response<RewardMappingResponseDto> response) {
                if(response.isSuccessful() && response.body() != null) {
                    List<RewardMappingModel> models = new ArrayList<>();

                    for(RewardMappingDto dto : response.body().data) {
                         RewardMappingModel model = new RewardMappingModel();

                         model.setId(dto.getId());
                         model.setName(dto.getName());
                         model.setType(dto.getType());
                         model.setDesc(dto.getDesc());
                         model.setUnit(dto.getUnit());

                         models.add(model);
                    }
                    callback.onSuccess(models);
                }
            }

            @Override
            public void onFailure(Call<RewardMappingResponseDto> call, Throwable t) {
                callback.onFail(t.getMessage());
            }
        });
    }

    @Override
    public void submitSpinResult(DailyTaskRequestDto requestDto, ApiCallBack<Boolean> callback) {
        eventApiService.updateProgress(requestDto).enqueue(new Callback<UpdateProgressDailyTaskResponseDto>() {
            @Override
            public void onResponse(Call<UpdateProgressDailyTaskResponseDto> call, Response<UpdateProgressDailyTaskResponseDto> response) {
                if (response.isSuccessful() && response.body() != null) {
                    // Server lưu thành công
                    callback.onSuccess(true);
                } else {
                    callback.onFail("Lỗi khi lưu kết quả: " + response.code());
                }
            }

            @Override
            public void onFailure(Call<UpdateProgressDailyTaskResponseDto> call, Throwable t) {
                callback.onFail(t.getMessage());
            }
        });
    }

}
