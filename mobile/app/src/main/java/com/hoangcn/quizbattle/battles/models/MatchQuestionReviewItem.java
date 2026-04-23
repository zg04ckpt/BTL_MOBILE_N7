package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class MatchQuestionReviewItem {
    private int questionId;
    private String questionContent;
    private List<String> correctAnswers = new ArrayList<>();
    private List<String> yourAnswers = new ArrayList<>();
    private boolean isCorrect;

    public int getQuestionId() {
        return questionId;
    }

    public String getQuestionContent() {
        return questionContent;
    }

    public List<String> getCorrectAnswers() {
        return correctAnswers;
    }

    public List<String> getYourAnswers() {
        return yourAnswers;
    }

    public boolean isCorrect() {
        return isCorrect;
    }
}
