package com.hoangcn.quizbattle.events.models;

import com.hoangcn.quizbattle.events.models.luckyspin.LuckySpinModel;
import com.hoangcn.quizbattle.events.models.luckyspin.SpinItemModel;
import com.hoangcn.quizbattle.events.models.quizmilestone.ThresholdModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskModel;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.List;

public class EventModel implements Serializable {
    private int id;
    private String name;
    private String desc;
    private String startTime;
    private String endTime;
    private String type;
    private boolean isLocked;
    private List<TaskModel> tasks;
    private List<ThresholdModel> thresholds;
    private int maxSpinTimePerDay;
    private List<SpinItemModel> spinItems;
    private String timeType;

    public EventModel(int id, String name, String desc, String startTime, String endTime,
                      String type, boolean isLocked, List<TaskModel> tasks,
                      List<ThresholdModel> thresholds, int maxSpinTimePerDay, String timeType) {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.startTime = startTime;
        this.endTime = endTime;
        this.type = type;
        this.isLocked = isLocked;
        this.tasks = tasks;
        this.thresholds = thresholds;
        this.maxSpinTimePerDay = maxSpinTimePerDay;
        this.timeType = timeType;
    }

    public int getMaxSpinTimePerDay() {
        return maxSpinTimePerDay;
    }

    public void setMaxSpinTimePerDay(int maxSpinTimePerDay) {
        this.maxSpinTimePerDay = maxSpinTimePerDay;
    }

    public List<SpinItemModel> getSpinItems() {
        return spinItems;
    }

    public void setSpinItems(List<SpinItemModel> spinItems) {
        this.spinItems = spinItems;
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

    public String getTimeType() {
        return timeType;
    }

    public void setTimeType(String timeType) {
        this.timeType = timeType;
    }
}
