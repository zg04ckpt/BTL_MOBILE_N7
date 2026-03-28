package com.n7.quizbattle.events.domain.model;

import java.io.Serializable;

public class Reward implements Serializable {
    private int eventRewardId;
    private int value;

    public int getEventRewardId() { return eventRewardId; }
    public void setEventRewardId(int eventRewardId) { this.eventRewardId = eventRewardId; }
    public int getValue() { return value; }
    public void setValue(int value) { this.value = value; }

    public Reward(int eventRewardId, int value) {
        this.eventRewardId = eventRewardId;
        this.value = value;
    }
    public Reward() {}
}
