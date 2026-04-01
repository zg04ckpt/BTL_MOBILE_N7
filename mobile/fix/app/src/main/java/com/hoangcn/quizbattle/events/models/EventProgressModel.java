package com.hoangcn.quizbattle.events.models;

import com.hoangcn.quizbattle.events.models.quizmilestone.ThresholdProgressModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskProgressModel;

import java.io.Serializable;
import java.util.List;

public class EventProgressModel implements Serializable {
    private List<TaskProgressModel> taskProgresses;
    private List<ThresholdProgressModel> thresholdProgresses;
    private int eventId;
    private String lastChanged;
    private int todaySpinTime;

    public List<ThresholdProgressModel> getThresholdProgresses() {
        return thresholdProgresses;
    }

    public void setThresholdProgresses(List<ThresholdProgressModel> thresholdProgresses) {
        this.thresholdProgresses = thresholdProgresses;
    }

    public void setTaskProgresses(List<TaskProgressModel> taskProgresses) {
        this.taskProgresses = taskProgresses;
    }

    public void setEventId(int eventId) {
        this.eventId = eventId;
    }

    public void setTodaySpinTime(int todaySpinTime) {
        this.todaySpinTime = todaySpinTime;
    }

    public void setLastChanged(String lastChanged) {
        this.lastChanged = lastChanged;
    }

    public List<TaskProgressModel> getTaskProgresses() {
        return taskProgresses;
    }

    public int getEventId() {
        return eventId;
    }

    public String getLastChanged() {
        return lastChanged;
    }

    public int getTodaySpinTime() {
        return todaySpinTime;
    }
}
