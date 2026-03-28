package com.n7.quizbattle.users.auth;

import android.content.Context;

public class UsersTokenManager {
    private final TokenManager tokenManager;

    public UsersTokenManager(Context ctx) {
        TokenManager.init(ctx);
        tokenManager = TokenManager.getInstance();
    }

    public void saveToken(String token) {
        tokenManager.saveToken(token);
    }

    public String getToken() {
        return tokenManager.getToken();
    }

    public void clear() {
        tokenManager.clear();
    }
}
