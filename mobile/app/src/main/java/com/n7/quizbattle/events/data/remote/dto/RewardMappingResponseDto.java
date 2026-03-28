package com.n7.quizbattle.events.data.remote.dto;

import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RewardMappingResponseDto {
    @SerializedName("isSuccess") public boolean isSuccess;
    @SerializedName("data") public List<RewardMappingDto> data;
    @SerializedName("message") public String message;
}
