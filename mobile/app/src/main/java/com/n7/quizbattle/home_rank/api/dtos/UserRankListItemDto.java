package com.n7.quizbattle.home_rank.api.dtos;

import com.google.gson.annotations.SerializedName;

public class UserRankListItemDto {
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
}
