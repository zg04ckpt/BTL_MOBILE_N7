package com.n7.quizbattle.events.data.remote.dto;

import java.util.List;

public class TaskModelDto {
    private int taskId;
    private String shortDesc;
    private List<RewardDto> rewards;
    private String type;

    private int numberOfMatchs;

    public int getTaskId() {
        return taskId;
    }

    public List<RewardDto> getRewards() {
        return rewards;
    }


    public String getShortDesc() {
        return shortDesc;
    }

    public String getType() {
        return type;
    }

    public int getNumberOfMatchs() {
        return numberOfMatchs;
    }
}
