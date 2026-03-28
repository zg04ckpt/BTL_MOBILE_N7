package com.n7.quizbattle.events.data.remote.dto;

import java.util.List;

public class ThresholdModelDto {
    private int thresholdId;
    private int expScoreGained;
    private List<RewardDto> rewards;

    private List<Integer> challengeQuestionIds;

    public int getThresholdId() {
        return thresholdId;
    }

    public int getExpScoreGained() {
        return expScoreGained;
    }

    public List<RewardDto> getRewards() {
        return rewards;
    }


    public List<Integer> getChallengeQuestionIds() {
        return challengeQuestionIds;
    }
}
