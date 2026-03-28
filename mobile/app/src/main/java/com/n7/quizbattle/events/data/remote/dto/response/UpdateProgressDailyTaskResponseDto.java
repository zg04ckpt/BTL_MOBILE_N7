package com.n7.quizbattle.events.data.remote.dto.response;

import com.google.gson.annotations.SerializedName;
import com.n7.quizbattle.events.data.remote.dto.EventItemDto;
import com.n7.quizbattle.events.data.remote.dto.UpdateProgressDailyTaskDto;

import java.util.List;

public class UpdateProgressDailyTaskResponseDto {

    @SerializedName("isSuccess") public boolean isSuccess;
    @SerializedName("data") public UpdateProgressDailyTaskDto data;
    @SerializedName("message") public String message;


    public boolean isSuccess() {
        return isSuccess;
    }

    public UpdateProgressDailyTaskDto getData() {
        return data;
    }

    public String getMessage() {
        return message;
    }
}
