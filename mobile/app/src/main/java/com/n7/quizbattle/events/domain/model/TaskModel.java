package com.n7.quizbattle.events.domain.model;

import java.io.Serializable;
import java.util.List;

public class TaskModel implements Serializable {
    private int taskId;
    private String shortDesc;
    private List<Reward> rewards;
    private String type;
    private int numberOfMatchs;

    public int getTaskId() { return taskId; }
    public void setTaskId(int taskId) { this.taskId = taskId; }
    public String getShortDesc() { return shortDesc; }
    public void setShortDesc(String shortDesc) { this.shortDesc = shortDesc; }
    public List<Reward> getRewards() { return rewards; }
    public void setRewards(List<Reward> rewards) { this.rewards = rewards; }
    public String getType() { return type; }
    public void setType(String type) { this.type = type; }
    public int getNumberOfMatchs() { return numberOfMatchs; }
    public void setNumberOfMatchs(int numberOfMatchs) { this.numberOfMatchs = numberOfMatchs; }

    public TaskModel(int taskId, String shortDesc, List<Reward> rewards, String type, int numberOfMatchs) {
        this.taskId = taskId;
        this.shortDesc = shortDesc;
        this.rewards = rewards;
        this.type = type;
        this.numberOfMatchs = numberOfMatchs;
    }

    public TaskModel() {}
}
