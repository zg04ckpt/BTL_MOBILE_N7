package com.hoangcn.quizbattle.events.models.luckyspin;

public class WinSpinItemModel {
    private SpinItemModel winItem;

    public WinSpinItemModel(SpinItemModel winItem) {
        this.winItem = winItem;
    }

    public SpinItemModel getWinItem() {
        return winItem;
    }

    public void setWinItem(SpinItemModel winItem) {
        this.winItem = winItem;
    }
}
