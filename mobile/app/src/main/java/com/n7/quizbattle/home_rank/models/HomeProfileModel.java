package com.n7.quizbattle.home_rank.models;

import java.io.Serializable;

public class HomeProfileModel implements Serializable {
    private static final long serialVersionUID = 1L;

    private final int id;
    private final String name;
    private final String avatarUrl;
    private final int level;
    private final int exp;
    private final int rank;
    private final int rankScore;
    private final int numberOfMatchs;
    private final float winningRate;
    private final int winningStreak;

    public HomeProfileModel(
            int id,
            String name,
            String avatarUrl,
            int level,
            int exp,
            int rank,
            int rankScore,
            int numberOfMatchs,
            float winningRate,
            int winningStreak
    ) {
        this.id = id;
        this.name = name;
        this.avatarUrl = avatarUrl;
        this.level = level;
        this.exp = exp;
        this.rank = rank;
        this.rankScore = rankScore;
        this.numberOfMatchs = numberOfMatchs;
        this.winningRate = winningRate;
        this.winningStreak = winningStreak;
    }

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

    public int getExp() {
        return exp;
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
