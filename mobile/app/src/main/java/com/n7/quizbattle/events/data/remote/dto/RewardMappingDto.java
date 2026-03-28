package com.n7.quizbattle.events.data.remote.dto;

public class RewardMappingDto {
    private int id;
    private String name;
    private String type;
    private String desc;
    private String unit;

    // Các trường khác của RewardMappingDto

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getType() {
        return type;
    }

    public String getDesc() {
        return desc;
    }

    public String getUnit() {
        return unit;
    }

    public RewardMappingDto() {
    }
}
