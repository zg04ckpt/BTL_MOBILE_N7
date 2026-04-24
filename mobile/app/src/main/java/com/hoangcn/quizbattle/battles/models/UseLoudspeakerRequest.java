package com.hoangcn.quizbattle.battles.models;

public class UseLoudspeakerRequest {
    private final String trackingId;
    private final String message;

    public UseLoudspeakerRequest(String trackingId, String message) {
        this.trackingId = trackingId;
        this.message = message;
    }
}
