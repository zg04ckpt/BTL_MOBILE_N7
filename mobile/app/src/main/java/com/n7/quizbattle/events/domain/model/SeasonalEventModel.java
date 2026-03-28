package com.n7.quizbattle.events.domain.model;

import java.io.Serializable;
import java.util.List;

public class SeasonalEventModel implements Serializable {
    private int id;
    private String name;
    private String startTime;
    private String endTime;
    private String desc;
    private String timeType;
    private String type;
    private boolean isLocked;
    private List<TaskModel> tasks;

    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getStartTime() { return startTime; }
    public void setStartTime(String startTime) { this.startTime = startTime; }
    public String getEndTime() { return endTime; }
    public void setEndTime(String endTime) { this.endTime = endTime; }
    public String getDesc() { return desc; }
    public void setDesc(String desc) { this.desc = desc; }
    public String getTimeType() { return timeType; }
    public void setTimeType(String timeType) { this.timeType = timeType; }
    public String getType() { return type; }
    public void setType(String type) { this.type = type; }
    public boolean isLocked() { return isLocked; }
    public void setLocked(boolean locked) { this.isLocked = locked; }
    public List<TaskModel> getTasks() { return tasks; }
    public void setTasks(List<TaskModel> tasks) { this.tasks = tasks; }

    public SeasonalEventModel() {}
}
