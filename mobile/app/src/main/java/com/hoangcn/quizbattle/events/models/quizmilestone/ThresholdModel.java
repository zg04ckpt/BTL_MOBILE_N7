package com.hoangcn.quizbattle.events.models.quizmilestone;

import com.hoangcn.quizbattle.events.models.EventRewardListItemModel;

import java.io.Serializable;
import java.util.List;

public class ThresholdModel implements Serializable {
    private int thresholdId;
    private int expScoreGained;
    private List<EventRewardListItemModel> rewards;
    private List<Integer> challengeQuestionIds;

    public int getThresholdId() { return thresholdId; }
    public void setThresholdId(int thresholdId) { this.thresholdId = thresholdId; }
    public int getExpScoreGained() { return expScoreGained; }
    public void setExpScoreGained(int expScoreGained) { this.expScoreGained = expScoreGained; }
    public List<Integer> getChallengeQuestionIds() { return challengeQuestionIds; }
    public void setChallengeQuestionIds(List<Integer> challengeQuestionIds) { this.challengeQuestionIds = challengeQuestionIds; }
    public List<EventRewardListItemModel> getRewards() { return rewards; }
    public void setRewards(List<EventRewardListItemModel> rewardModels) { this.rewards = rewardModels; }

    public ThresholdModel(int thresholdId, int expScoreGained, List<EventRewardListItemModel> rewards, List<Integer> challengeQuestionIds) {
        this.thresholdId = thresholdId;
        this.expScoreGained = expScoreGained;
        this.rewards = rewards;
        this.challengeQuestionIds = challengeQuestionIds;
    }
    public ThresholdModel() {}
}
