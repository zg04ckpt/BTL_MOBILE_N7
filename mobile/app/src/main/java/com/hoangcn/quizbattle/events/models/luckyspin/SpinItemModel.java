package com.hoangcn.quizbattle.events.models.luckyspin;

import com.hoangcn.quizbattle.events.models.EventRewardListItemModel;

import java.io.Serializable;

public class SpinItemModel implements Serializable {
    private int itemId;

    private EventRewardListItemModel reward;
    private int rate;

    public SpinItemModel() {

    }

    public int getItemId() {
        return itemId;
    }

    public void setItemId(int itemId) {
        this.itemId = itemId;
    }

    public int getRate() {
        return rate;
    }

    public void setRate(int rate) {
        this.rate = rate;
    }

    public EventRewardListItemModel getReward() {
        return reward;
    }

    public void setReward(EventRewardListItemModel rewardModel) {
        this.reward = rewardModel;
    }

    public SpinItemModel(int itemId, EventRewardListItemModel reward, int rate) {
        this.itemId = itemId;
        this.reward = reward;
        this.rate = rate;
    }
}
