package com.hoangcn.quizbattle.events.models;

public class ClaimRewardRequest {
    private int eventId;
    private int thresholdId;
    private Integer taskId;

    public ClaimRewardRequest(int eventId, int thresholdId, Integer taskId) {
        this.eventId = eventId;
        this.thresholdId = thresholdId;
        this.taskId = taskId;
    }

    public int getEventId() {
        return eventId;
    }

    public void setEventId(int eventId) {
        this.eventId = eventId;
    }

    public int getThresholdId() {
        return thresholdId;
    }

    public void setThresholdId(int thresholdId) {
        this.thresholdId = thresholdId;
    }

    public Integer getTaskId() {
        return taskId;
    }

    public void setTaskId(Integer taskId) {
        this.taskId = taskId;
    }
}
