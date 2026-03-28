package com.hoangcn.quizbattle.battles.models;

public class MatchHistory {
    private int avatarRes;
    private String name;
    private String time;
    private String rp;
    private boolean isWin;

    public MatchHistory(int avatarRes, String name, String time, String rp, boolean isWin) {
        this.avatarRes = avatarRes;
        this.name = name;
        this.time = time;
        this.rp = rp;
        this.isWin = isWin;
    }

    public int getAvatarRes() {
        return avatarRes;
    }

    public String getName() {
        return name;
    }

    public String getTime() {
        return time;
    }

    public String getRp() {
        return rp;
    }

    public boolean isWin() {
        return isWin;
    }
}
