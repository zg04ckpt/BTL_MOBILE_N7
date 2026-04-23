package com.hoangcn.quizbattle.events.models.luckyspin;

import java.io.Serializable;
import java.util.List;

public class LuckySpinModel implements Serializable {
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
