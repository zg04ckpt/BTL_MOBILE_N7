package com.n7.quizbattle.events.data.remote.dto.response;

import com.google.gson.annotations.SerializedName;
import com.n7.quizbattle.events.data.remote.dto.ProgressModelDto;

import java.util.List;

public class ProgressEventResponseDto {
    @SerializedName("isSuccess") private boolean isSuccess;
    @SerializedName("message")  private String message;

    @SerializedName("data") private List<ProgressModelDto> data;

    public boolean isSuccess() {
        return isSuccess;
    }

    public String getMessage() {
        return message;
    }

    public List<ProgressModelDto> getData() {
        return data;
    }
}
