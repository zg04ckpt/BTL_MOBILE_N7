package com.n7.quizbattle.events.domain.model;

import java.util.List;

public class EventModel {
    private int id;
    private String name;
    private String desc;
    private String startTime;
    private String endTime;
    private String type;
    private boolean isLocked;

    private List<TaskModel> tasks;
    private List<ThresholdModel> thresholds;
    private LuckySpinModel luckySpin;

    private String timeType;

    public String getTimeType() {
        return timeType;
    }

    public void setTimeType(String timeType) {
        this.timeType = timeType;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDesc() {
        return desc;
    }

    public void setDesc(String desc) {
        this.desc = desc;
    }

    public String getStartTime() {
        return startTime;
    }

    public void setStartTime(String startTime) {
        this.startTime = startTime;
    }

    public String getEndTime() {
        return endTime;
    }

    public void setEndTime(String endTime) {
        this.endTime = endTime;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public boolean isLocked() {
        return isLocked;
    }

    public void setLocked(boolean locked) {
        isLocked = locked;
    }

    public List<TaskModel> getTasks() {
        return tasks;
    }

    public void setTasks(List<TaskModel> tasks) {
        this.tasks = tasks;
    }

    public List<ThresholdModel> getThresholds() {
        return thresholds;
    }

    public void setThresholds(List<ThresholdModel> thresholds) {
        this.thresholds = thresholds;
    }

    public LuckySpinModel getLuckySpin() {
        return luckySpin;
    }

    public void setLuckySpin(LuckySpinModel luckySpin) {
        this.luckySpin = luckySpin;
    }

    public EventModel(int id, String name, String startTime, String desc, String endTime, String type, boolean isLocked, List<TaskModel> tasks, List<ThresholdModel> thresholds, LuckySpinModel luckySpin) {
        this.id = id;
        this.name = name;
        this.startTime = startTime;
        this.desc = desc;
        this.endTime = endTime;
        this.type = type;
        this.isLocked = isLocked;
        this.tasks = tasks;
        this.thresholds = thresholds;
        this.luckySpin = luckySpin;
    }

    public EventModel() {
    }
}
