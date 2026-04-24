package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class MatchInfoResponse {
    private int matchId;
    private String trackingId;
    private boolean isSolo;
    private List<MatchQuestionContentItem> questions = new ArrayList<>();
    private int maxSecondPerQuestions;

    public int getMatchId() {
        return matchId;
    }

    public String getTrackingId() {
        return trackingId;
    }

    public boolean isSolo() {
        return isSolo;
    }

    public List<MatchQuestionContentItem> getQuestions() {
        return questions;
    }

    public int getMaxSecondPerQuestions() {
        return maxSecondPerQuestions;
    }
}
