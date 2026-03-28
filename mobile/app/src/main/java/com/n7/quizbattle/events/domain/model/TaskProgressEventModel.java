package com.n7.quizbattle.events.domain.model;

import java.util.List;

public class TaskProgressEventModel {
    private List<TaskProgressModel> taskProgresses;

    private int eventId;
    private String lastChanged;

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

    public TaskProgressEventModel(List<TaskProgressModel> taskProgresses, int eventId, String lastChanged) {
        this.taskProgresses = taskProgresses;
        this.eventId = eventId;
        this.lastChanged = lastChanged;
    }

    public TaskProgressEventModel() {
        // Constructor mặc định nếu cần
    }
}
