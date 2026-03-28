package com.n7.quizbattle.home_rank.models;

public class RankBoardItemModel {
    private final int userId;
    private final String displayName;
    private final String avatarUrl;
    private final int rank;
    private final int rankScore;

    public RankBoardItemModel(int userId, String displayName, String avatarUrl, int rank, int rankScore) {
        this.userId = userId;
        this.displayName = displayName;
        this.avatarUrl = avatarUrl;
        this.rank = rank;
        this.rankScore = rankScore;
    }

    public int getUserId() {
        return userId;
    }

    public String getDisplayName() {
        return displayName;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public int getRank() {
        return rank;
    }

    public int getRankScore() {
        return rankScore;
    }
}
