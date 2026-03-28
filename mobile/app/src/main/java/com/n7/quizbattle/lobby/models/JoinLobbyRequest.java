package com.n7.quizbattle.lobby.models;

public class JoinLobbyRequest {
    private String battleType;
    private int numberOfPlayers;
    private String contentType;
    private Integer topicId;

    public JoinLobbyRequest(Integer topicId, String contentType, int numberOfPlayers, String battleType) {
        this.topicId = topicId;
        this.contentType = contentType;
        this.numberOfPlayers = numberOfPlayers;
        this.battleType = battleType;
    }
}
