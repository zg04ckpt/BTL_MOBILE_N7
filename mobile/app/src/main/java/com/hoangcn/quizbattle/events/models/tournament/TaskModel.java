package com.hoangcn.quizbattle.events.models.tournament;

import com.hoangcn.quizbattle.events.models.EventRewardListItemModel;

import java.io.Serializable;
import java.util.List;

public class TaskModel implements Serializable {
    private int taskId;
    private String shortDesc;
    private List<EventRewardListItemModel> rewardModels;
    private String type;
    private int numberOfMatchs;

    public int getTaskId() { return taskId; }
    public void setTaskId(int taskId) { this.taskId = taskId; }
    public String getShortDesc() { return shortDesc; }
    public void setShortDesc(String shortDesc) { this.shortDesc = shortDesc; }
    public List<EventRewardListItemModel> getRewards() { return rewardModels; }
    public void setRewards(List<EventRewardListItemModel> rewardModels) { this.rewardModels = rewardModels; }
    public String getType() { return type; }
    public void setType(String type) { this.type = type; }
    public int getNumberOfMatchs() { return numberOfMatchs; }
    public void setNumberOfMatchs(int numberOfMatchs) { this.numberOfMatchs = numberOfMatchs; }

    public TaskModel(int taskId, String shortDesc, List<EventRewardListItemModel> rewardModels, String type, int numberOfMatchs) {
        this.taskId = taskId;
        this.shortDesc = shortDesc;
        this.rewardModels = rewardModels;
        this.type = type;
        this.numberOfMatchs = numberOfMatchs;
    }

    public TaskModel() {}
}
