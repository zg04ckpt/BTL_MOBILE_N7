package com.hoangcn.quizbattle.battles.models;

import java.util.List;

public class StartSoloMatchRequest {
    private final String contentType;
    private final Integer topicId;
    private final List<Integer> fixedQuestionIds;

    public StartSoloMatchRequest(String contentType, Integer topicId, List<Integer> fixedQuestionIds) {
        this.contentType = contentType;
        this.topicId = topicId;
        this.fixedQuestionIds = fixedQuestionIds;
    }
}
