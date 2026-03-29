package com.hoangcn.quizbattle.home_rank.models;

import com.google.gson.annotations.SerializedName;

public class UserRankListItem {
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

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public void setAvatarUrl(String avatarUrl) {
        this.avatarUrl = avatarUrl;
    }

    public void setDisplayName(String displayName) {
        this.displayName = displayName;
    }

    public void setRankScore(int rankScore) {
        this.rankScore = rankScore;
    }

    public void setRank(int rank) {
        this.rank = rank;
    }
}
