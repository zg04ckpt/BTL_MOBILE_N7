package com.n7.quizbattle.home_rank.models;

public class RankDetailModel {
    private final int userId;
    private final String displayName;
    private final String avatarUrl;
    private final int rank;
    private final int rankScore;
    private final int numberOfMatchs;
    private final float winningRate;
    private final int winningStreak;

    public RankDetailModel(
            int userId,
            String displayName,
            String avatarUrl,
            int rank,
            int rankScore,
            int numberOfMatchs,
            float winningRate,
            int winningStreak
    ) {
        this.userId = userId;
        this.displayName = displayName;
        this.avatarUrl = avatarUrl;
        this.rank = rank;
        this.rankScore = rankScore;
        this.numberOfMatchs = numberOfMatchs;
        this.winningRate = winningRate;
        this.winningStreak = winningStreak;
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
