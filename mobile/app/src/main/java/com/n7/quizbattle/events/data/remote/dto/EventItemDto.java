package com.n7.quizbattle.events.data.remote.dto;

import com.google.gson.annotations.SerializedName;

import java.util.List;

public class EventItemDto {
    private int id;
    private String name;
    private String type;
    private String desc;
    private String startTime;
    private String endTime;
    private boolean isLocked;
    private List<TaskModelDto> tasks;
    private List<ThresholdModelDto> thresholds;
    private String timeType;
    @SerializedName("maxSpinTimePerDay") public int maxSpinTimePerDay;
    @SerializedName("spinItems") public List<SpinItemDto> spinItems;

    public String getTimeType() {
        return timeType;
    }

    public String getName() {
        return name;
    }

    public int getId() {
        return id;
    }

    public String getType() {
        return type;
    }

    public String getDesc() {
        return desc;
    }

    public String getStartTime() {
        return startTime;
    }

    public String getEndTime() {
        return endTime;
    }

    public boolean isLocked() {
        return isLocked;
    }

    public List<TaskModelDto> getTasks() {
        return tasks;
    }

    public List<ThresholdModelDto> getThresholds() {
        return thresholds;
    }

    public int getMaxSpinTimePerDay() {
        return maxSpinTimePerDay;
    }

    public List<SpinItemDto> getSpinItems() {
        return spinItems;
    }


}
