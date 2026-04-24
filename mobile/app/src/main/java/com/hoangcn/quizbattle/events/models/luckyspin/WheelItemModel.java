package com.hoangcn.quizbattle.events.models.luckyspin;

import androidx.annotation.DrawableRes;

public class WheelItemModel {
    @DrawableRes
    public int iconResource;

    public WheelItemModel(@DrawableRes int iconResource) {
        this.iconResource = iconResource;
    }
}
