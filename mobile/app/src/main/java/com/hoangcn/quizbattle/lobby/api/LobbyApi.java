package com.hoangcn.quizbattle.lobby.api;

import com.hoangcn.quizbattle.lobby.models.JoinLobbyRequest;
import com.hoangcn.quizbattle.lobby.models.JoinLobbyResponse;
import com.hoangcn.quizbattle.lobby.models.StartOptionsResponse;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.Map;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;

public interface LobbyApi {
    @GET("battles/start-options")
    Call<ApiResponse<StartOptionsResponse>> getStartOptions();

    @POST("battles/lobbies/join")
    Call<ApiResponse<JoinLobbyResponse>> joinLobby(@Body JoinLobbyRequest request);

    @POST("battles/lobbies/out")
    Call<ApiResponse<JoinLobbyResponse>> outLobby(@Body Map<String, String> request);
}
