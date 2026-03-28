package com.n7.quizbattle.events.data.remote.dto;

import java.util.List;

public class ProgressModelDto {
    private List<TaskProgressModelDto> taskProgresses;
    private int eventId;
    private String lastChanged;

    private int todaySpinTime;

    public void setTaskProgresses(List<TaskProgressModelDto> taskProgresses) {
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

    public List<TaskProgressModelDto> getTaskProgresses() {
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
