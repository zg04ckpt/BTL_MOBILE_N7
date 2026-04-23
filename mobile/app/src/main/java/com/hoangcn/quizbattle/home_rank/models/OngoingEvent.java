package com.hoangcn.quizbattle.home_rank.models;

public class OngoingEvent {
    private int id;
    private String name, desc, type, timeType;

    public OngoingEvent(int id, String name, String desc, String type, String timeType) {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.type = type;
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

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getTimeType() {
        return timeType;
    }

    public void setTimeType(String timeType) {
        this.timeType = timeType;
    }
}
