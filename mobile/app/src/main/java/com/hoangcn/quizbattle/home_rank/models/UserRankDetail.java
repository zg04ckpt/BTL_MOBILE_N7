package com.hoangcn.quizbattle.home_rank.models;

import com.google.gson.annotations.SerializedName;

public class UserRankDetail {
    @SerializedName("userId")
    private int userId;

    @SerializedName("avatarUrl")
    private String avatarUrl;

    @SerializedName("displayName")
    private String displayName;

    @SerializedName("rankScore")
    private int rankScore;

    @SerializedName("rank")
    private int rank;

    @SerializedName("numberOfMatchs")
    private int numberOfMatchs;

    @SerializedName("winningRate")
    private float winningRate;

    @SerializedName("winningStreak")
    private int winningStreak;

    public int getUserId() {
        return userId;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public String getDisplayName() {
        return displayName;
    }

    public int getRankScore() {
        return rankScore;
    }

    public int getRank() {
        return rank;
    }

    public int getNumberOfMatchs() {
        return numberOfMatchs;
    }

    public float getWinningRate() {
        return winningRate;
    }

    public int getWinningStreak() {
        return winningStreak;
    }
}
