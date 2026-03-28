package com.n7.quizbattle.events.data.remote.dto;

public class SpinItemDto {
    public int itemId;
    public RewardDto reward; // Lưu ý JSON của bạn là object đơn "reward", không phải list
    public int rate;

    public int getItemId() {
        return itemId;
    }

    public RewardDto getReward() {
        return reward;
    }

    public int getRate() {
        return rate;
    }
}
