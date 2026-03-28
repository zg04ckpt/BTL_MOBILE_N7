package com.n7.quizbattle.events.data.remote.dto;

public class UpdateProgressDailyTaskDto {
    private int todaySpinTime;

    private int eventId;

    private String lastChanged;

    public int getTodaySpinTime() {
        return todaySpinTime;
    }

    public int getEventId() {
        return eventId;
    }

    public String getLastChanged() {
        return lastChanged;
    }
}
