package com.n7.quizbattle.events.data.remote.dto;

public class TaskProgressModelDto {
    private int taskId;
    private boolean rewardClaimed;
    private boolean completed;

    public int getTaskId() {
        return taskId;
    }

    public boolean isRewardClaimed() {
        return rewardClaimed;
    }

    public boolean isCompleted() {
        return completed;
    }
}
