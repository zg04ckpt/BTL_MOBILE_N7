package com.hoangcn.quizbattle.users.models;

import com.google.gson.annotations.SerializedName;

public class LoginResponse {
    @SerializedName(value = "token", alternate = {"accessToken", "jwt", "bearerToken"})
    private String token;

    @SerializedName("user")
    private UserModel user;

    public String getToken() {
        return token;
    }

    public UserModel getUser() {
        return user;
    }
}
