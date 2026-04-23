package com.hoangcn.quizbattle.events.models.tournament;

public class TaskProgressModel {
    private int taskId;
    private boolean rewardClaimed;
    private boolean completed;

    public TaskProgressModel(int taskId, boolean rewardClaimed, boolean completed) {
        this.taskId = taskId;
        this.rewardClaimed = rewardClaimed;
        this.completed = completed;
    }

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
