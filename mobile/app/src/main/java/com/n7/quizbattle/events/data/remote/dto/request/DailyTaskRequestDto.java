package com.n7.quizbattle.events.data.remote.dto.request;

import com.google.gson.annotations.SerializedName;

public class DailyTaskRequestDto {
    @SerializedName("eventId")
    private String eventId;
    @SerializedName("progressJsonData")
    private String progressJsonData;

    public String getEventId() {
        return eventId;
    }

    public void setEventId(String eventId) {
        this.eventId = eventId;
    }

    public String getProgressJsonData() {
        return progressJsonData;
    }

    public void setProgressJsonData(String progressJsonData) {
        this.progressJsonData = progressJsonData;
    }
}
