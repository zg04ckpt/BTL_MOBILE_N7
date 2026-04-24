package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class MatchReviewResponse {
    private int matchId;
    private List<MatchResultUserItem> users = new ArrayList<>();
    private List<MatchQuestionReviewItem> questionReviews = new ArrayList<>();

    public int getMatchId() {
        return matchId;
    }

    public List<MatchResultUserItem> getUsers() {
        return users;
    }

    public List<MatchQuestionReviewItem> getQuestionReviews() {
        return questionReviews;
    }
}
