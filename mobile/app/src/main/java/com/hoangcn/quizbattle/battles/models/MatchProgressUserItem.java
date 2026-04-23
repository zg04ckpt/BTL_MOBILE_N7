package com.hoangcn.quizbattle.battles.models;

public class MatchProgressUserItem {
    private int userId;
    private String displayName;
    private String avatarUrl;
    private int score;
    private int progress;
    private int rank;
    private boolean isFinished;

    public int getUserId() {
        return userId;
    }

    public String getDisplayName() {
        return displayName;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public int getScore() {
        return score;
    }

    public int getProgress() {
        return progress;
    }

    public int getRank() {
        return rank;
    }

    public boolean isFinished() {
        return isFinished;
    }
}
