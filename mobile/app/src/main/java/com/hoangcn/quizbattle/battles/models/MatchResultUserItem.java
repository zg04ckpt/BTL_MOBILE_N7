package com.hoangcn.quizbattle.battles.models;

public class MatchResultUserItem {
    private int userId;
    private String name;
    private String avatarUrl;
    private int score;
    private int expGained;
    private int rankScoreGained;
    private boolean isRankProtected;

    public int getUserId() {
        return userId;
    }

    public String getName() {
        return name;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public int getScore() {
        return score;
    }

    public int getExpGained() {
        return expGained;
    }

    public int getRankScoreGained() {
        return rankScoreGained;
    }

    public boolean isRankProtected() {
        return isRankProtected;
    }
}
