package com.n7.quizbattle.events.domain.model;

import java.util.List;

public class ProgressModel {
    private List<TaskProgressModel> taskProgresses;
    private int eventId;
    private String lastChanged;

    private int todaySpinTime;

    public List<TaskProgressModel> getTaskProgresses() {
        return taskProgresses;
    }

    public void setTaskProgresses(List<TaskProgressModel> taskProgresses) {
        this.taskProgresses = taskProgresses;
    }

    public int getEventId() {
        return eventId;
    }

    public void setEventId(int eventId) {
        this.eventId = eventId;
    }

    public String getLastChanged() {
        return lastChanged;
    }

    public void setLastChanged(String lastChanged) {
        this.lastChanged = lastChanged;
    }

    public int getTodaySpinTime() {
        return todaySpinTime;
    }

    public void setTodaySpinTime(int todaySpinTime) {
        this.todaySpinTime = todaySpinTime;
    }

    public ProgressModel() {
    }
}
