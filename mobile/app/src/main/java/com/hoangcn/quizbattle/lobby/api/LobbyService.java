package com.hoangcn.quizbattle.lobby.api;

import android.content.Context;

import com.hoangcn.quizbattle.lobby.models.JoinLobbyRequest;
import com.hoangcn.quizbattle.lobby.models.JoinLobbyResponse;
import com.hoangcn.quizbattle.lobby.models.StartOptionsResponse;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.api.BaseApi;

public class LobbyService extends BaseApi {
    private final LobbyApi api;

    public LobbyService(Context context){
        super(context);
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
