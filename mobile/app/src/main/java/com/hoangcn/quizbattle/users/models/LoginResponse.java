package com.hoangcn.quizbattle.users.models;

import com.google.gson.annotations.SerializedName;

public class LoginResponse {
    private String accessToken;
    private int id;
    private String name;
    private String avatarUrl;
    private int level;
    private int rank;
    private int rankScore;
    private String roleName;

    public LoginResponse(String accessToken, int id, String name, String avatarUrl, int level,
                         int rank, int rankScore, String roleName) {
        this.accessToken = accessToken;
        this.id = id;
        this.name = name;
        this.avatarUrl = avatarUrl;
        this.level = level;
        this.rank = rank;
        this.rankScore = rankScore;
        this.roleName = roleName;
    }

    public String getAccessToken() {
        return accessToken;
    }

    public void setAccessToken(String accessToken) {
        this.accessToken = accessToken;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAvatarUrl() {
        return avatarUrl;
    }

    public void setAvatarUrl(String avatarUrl) {
        this.avatarUrl = avatarUrl;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public int getRank() {
        return rank;
    }

    public void setRank(int rank) {
        this.rank = rank;
    }

    public int getRankScore() {
        return rankScore;
    }

    public void setRankScore(int rankScore) {
        this.rankScore = rankScore;
    }

    public String getRoleName() {
        return roleName;
    }

    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }
}
