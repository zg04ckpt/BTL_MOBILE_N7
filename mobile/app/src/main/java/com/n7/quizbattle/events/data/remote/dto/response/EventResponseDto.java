package com.n7.quizbattle.events.data.remote.dto.response;

import com.google.gson.annotations.SerializedName;
import com.n7.quizbattle.events.data.remote.dto.EventItemDto;

import java.util.List;

public class EventResponseDto {
    @SerializedName("isSuccess") public boolean isSuccess;
    @SerializedName("data") public List<EventItemDto> data;
    @SerializedName("message") public String message;

    public boolean isSuccess() {
        return isSuccess;
    }

    public List<EventItemDto> getData() {
        return data;
    }

    public String getMessage() {
        return message;
    }


}
