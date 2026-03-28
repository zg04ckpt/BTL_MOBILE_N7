package com.n7.quizbattle.users.models;

import com.google.gson.annotations.SerializedName;

public class UserModel {
    @SerializedName("id")
    private String id;

    // Map both "displayName" and "name" from backend into this field
    @SerializedName(value = "displayName", alternate = {"name"})
    private String displayName;

    @SerializedName("email")
    private String email;

    // Backend returns "avatarUrl"
    @SerializedName(value = "avatar", alternate = {"avatarUrl"})
    private String avatar;

    @SerializedName("level")
    private Integer level;

    @SerializedName("exp")
    private Integer exp;

    @SerializedName("rank")
    private Integer rank;

    @SerializedName("rankScore")
    private Integer rankScore;

    @SerializedName("winningStreak")
    private Integer winningStreak;

    @SerializedName("winningRate")
    private Double winningRate;

    // Note: API field spelled "numberOfMatchs"
    @SerializedName(value = "numberOfMatches", alternate = {"numberOfMatchs"})
    private Integer numberOfMatches;

    public String getId() {
        return id;
    }

    public String getDisplayName() {
        return displayName;
    }

    public String getEmail() {
        return email;
    }

    public String getAvatar() {
        return avatar;
    }

    public Integer getLevel() {
        return level;
    }

    public Integer getExp() {
        return exp;
    }

    public Integer getRank() {
        return rank;
    }

    public Integer getRankScore() {
        return rankScore;
    }

    public Integer getWinningStreak() {
        return winningStreak;
    }

    public Double getWinningRate() {
        return winningRate;
    }

    public Integer getNumberOfMatches() {
        return numberOfMatches;
    }
}
