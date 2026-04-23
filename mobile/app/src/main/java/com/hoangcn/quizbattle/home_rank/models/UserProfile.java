package com.hoangcn.quizbattle.home_rank.models;

import com.google.gson.annotations.SerializedName;

public class UserProfile {
    @SerializedName("id")
    private int id;

    @SerializedName("name")
    private String name;

    @SerializedName("avatarUrl")
    private String avatarUrl;

    @SerializedName("level")
    private int level;

    @SerializedName("rank")
    private int rank;

    @SerializedName("exp")
    private int exp;

    @SerializedName("expToUpLevel")
    private int expToUpLevel;

    @SerializedName("rankScore")
    private int rankScore;

    @SerializedName("winningStreak")
    private int winningStreak;

    @SerializedName("winningRate")
    private float winningRate;

    @SerializedName("numberOfMatchs")
    private int numberOfMatchs;

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public int getLevel() {
        return level;
    }

    public int getRank() {
        return rank;
    }

    public int getExp() {
        return exp;
    }

    public int getRankScore() {
        return rankScore;
    }

    public int getWinningStreak() {
        return winningStreak;
    }

    public float getWinningRate() {
        return winningRate;
    }

    public int getNumberOfMatchs() {
        return numberOfMatchs;
    }

    public int getExpToUpLevel() {
        return expToUpLevel;
    }
}
