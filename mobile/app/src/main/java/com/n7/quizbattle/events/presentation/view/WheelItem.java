package com.n7.quizbattle.events.presentation.view;

import androidx.annotation.DrawableRes;

public class WheelItem {
    public String name;

    // ID của file ảnh trong res/drawable (ví dụ: R.drawable.ic_coin)
    @DrawableRes
    public int iconResource;

    public WheelItem(String name, @DrawableRes int iconResource) {
        this.name = name;
        this.iconResource = iconResource;
    }
}
