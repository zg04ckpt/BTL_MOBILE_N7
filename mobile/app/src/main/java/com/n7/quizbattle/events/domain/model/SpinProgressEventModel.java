package com.n7.quizbattle.events.domain.model;

public class SpinProgressEventModel {
    private int todaySpinTime;
    private int eventId;

    private String lastChanged;

    public int getTodaySpinTime() {
        return todaySpinTime;
    }

    public void setTodaySpinTime(int todaySpinTime) {
        this.todaySpinTime = todaySpinTime;
    }

    public String getLastChanged() {
        return lastChanged;
    }

    public void setLastChanged(String lastChanged) {
        this.lastChanged = lastChanged;
    }

    public int getEventId() {
        return eventId;
    }

    public void setEventId(int eventId) {
        this.eventId = eventId;
    }

    public SpinProgressEventModel(int todaySpinTime, int eventId, String lastChanged) {
        this.todaySpinTime = todaySpinTime;
        this.eventId = eventId;
        this.lastChanged = lastChanged;
    }

    public SpinProgressEventModel() {
    }
}
