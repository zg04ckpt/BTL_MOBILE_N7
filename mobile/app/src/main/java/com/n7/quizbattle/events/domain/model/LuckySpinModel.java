package com.n7.quizbattle.events.domain.model;

import java.util.List;

public class LuckySpinModel {
    private int maxSpinTimePerDay;
    private List<SpinItemModel> spinItems;
    public int getMaxSpinTimePerDay() {
        return maxSpinTimePerDay;
    }

    public void setMaxSpinTimePerDay(int maxSpinTimePerDay) {
        this.maxSpinTimePerDay = maxSpinTimePerDay;
    }

    public List<SpinItemModel> getSpinItems() {
        return spinItems;
    }

    public void setSpinItems(List<SpinItemModel> spinItems) {
        this.spinItems = spinItems;
    }

    public LuckySpinModel(int maxSpinTimePerDay, List<SpinItemModel> spinItems) {
        this.maxSpinTimePerDay = maxSpinTimePerDay;
        this.spinItems = spinItems;
    }

    public LuckySpinModel() {
    }
}
