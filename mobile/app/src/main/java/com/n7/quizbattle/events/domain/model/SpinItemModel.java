package com.n7.quizbattle.events.domain.model;

import java.util.List;

public class SpinItemModel {

    private int itemId;

    private Reward reward;
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

    public Reward getReward() {
        return reward;
    }

    public void setReward(Reward reward) {
        this.reward = reward;
    }

    public SpinItemModel(int itemId, Reward reward, int rate) {
        this.itemId = itemId;
        this.reward = reward;
        this.rate = rate;
    }
}
