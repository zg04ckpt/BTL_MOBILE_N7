package com.n7.quizbattle.lobby.api;

import com.n7.quizbattle.lobby.models.JoinLobbyRequest;
import com.n7.quizbattle.lobby.models.JoinLobbyResponse;
import com.n7.quizbattle.lobby.models.StartOptionsResponse;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.api.BaseApi;

public class LobbyService extends BaseApi {
    private final LobbyApi api;

    public LobbyService(){
        this.api = getRetrofit().create(LobbyApi.class);
    }

    public void getStartOptions(ApiCallback<StartOptionsResponse> callback) {
        enqueue(api.getStartOptions(), callback);
    }

    public void joinLobby(JoinLobbyRequest request, ApiCallback<JoinLobbyResponse> callback) {
        enqueue(api.joinLobby(request), callback);
    }

    public void outLobby(String roomId, ApiCallback<JoinLobbyResponse> callback) {
        java.util.Map<String, String> body = new java.util.HashMap<>();
        body.put("lobbyRoomId", roomId);
        enqueue(api.outLobby(body), callback);
    }
}
