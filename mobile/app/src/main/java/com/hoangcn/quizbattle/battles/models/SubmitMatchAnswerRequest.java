package com.hoangcn.quizbattle.battles.models;

import java.util.ArrayList;
import java.util.List;

public class SubmitMatchAnswerRequest {
    private final String trackingId;
    private final int questionId;
    private final List<String> answers;

    public SubmitMatchAnswerRequest(String trackingId, int questionId, List<String> answers) {
        this.trackingId = trackingId;
        this.questionId = questionId;
        this.answers = answers == null ? new ArrayList<>() : answers;
    }
}
