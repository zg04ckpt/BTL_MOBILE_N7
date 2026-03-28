package com.n7.quizbattle.events.domain.model;

public class TaskProgressModel
{
    private int taskId;
    private boolean rewardClaimed;
    private boolean completed;

    public int getTaskId() {
        return taskId;
    }

    public void setTaskId(int taskId) {
        this.taskId = taskId;
    }

    public boolean isRewardClaimed() {
        return rewardClaimed;
    }

    public void setRewardClaimed(boolean rewardClaimed) {
        this.rewardClaimed = rewardClaimed;
    }

    public boolean isCompleted() {
        return completed;
    }

    public void setCompleted(boolean completed) {
        this.completed = completed;
    }

    public TaskProgressModel(int taskId, boolean rewardClaimed, boolean completed) {
        this.taskId = taskId;
        this.rewardClaimed = rewardClaimed;
        this.completed = completed;
    }

    public TaskProgressModel() {
    }
}
