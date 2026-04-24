package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class MatchStateResponse {
    private int matchId;
    private String trackingId;
    private boolean isEnded;
    private int totalQuestions;
    private List<MatchProgressUserItem> users = new ArrayList<>();

    public int getMatchId() {
        return matchId;
    }

    public String getTrackingId() {
        return trackingId;
    }

    public boolean isEnded() {
        return isEnded;
    }

    public int getTotalQuestions() {
        return totalQuestions;
    }

    public List<MatchProgressUserItem> getUsers() {
        return users;
    }
}
