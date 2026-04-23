package com.hoangcn.quizbattle.events.models;

import java.io.Serializable;

public class EventRewardListItemModel implements Serializable {
    private int eventRewardId;
    private int value;

    public int getEventRewardId() { return eventRewardId; }
    public void setEventRewardId(int eventRewardId) { this.eventRewardId = eventRewardId; }
    public int getValue() { return value; }
    public void setValue(int value) { this.value = value; }

    public EventRewardListItemModel(int eventRewardId, int value) {
        this.eventRewardId = eventRewardId;
        this.value = value;
    }
    public EventRewardListItemModel() {}
}
