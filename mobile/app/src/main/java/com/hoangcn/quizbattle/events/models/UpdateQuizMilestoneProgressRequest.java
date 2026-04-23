package com.hoangcn.quizbattle.events.models;

public class UpdateQuizMilestoneProgressRequest {
    private final int eventId;
    private final int thresholdId;
    private final Boolean rewardClaimed;
    private final Boolean completed;

    public UpdateQuizMilestoneProgressRequest(int eventId, int thresholdId, Boolean rewardClaimed, Boolean completed) {
        this.eventId = eventId;
        this.thresholdId = thresholdId;
        this.rewardClaimed = rewardClaimed;
        this.completed = completed;
    }
}
