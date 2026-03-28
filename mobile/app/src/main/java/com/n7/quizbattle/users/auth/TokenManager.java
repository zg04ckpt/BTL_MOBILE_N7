package com.n7.quizbattle.users.auth;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

/**
 * Lightweight singleton to store and retrieve the auth token for user-related APIs.
 */
public class TokenManager {
    private static final String PREF = "users_prefs";
    private static final String KEY_TOKEN = "token";

    private static TokenManager instance;
    private final SharedPreferences prefs;

    private TokenManager(Context context) {
        prefs = context.getApplicationContext().getSharedPreferences(PREF, Context.MODE_PRIVATE);
    }

    public static synchronized void init(Context context) {
        if (instance == null && context != null) {
            instance = new TokenManager(context);
        }
    }

    public static synchronized TokenManager getInstance() {
        if (instance == null) {
            throw new IllegalStateException("TokenManager not initialized. Call init(context) first.");
        }
        return instance;
    }

    public void saveToken(String token) {
        prefs.edit().putString(KEY_TOKEN, token).apply();
        Log.d("TokenManager", "Saved token length=" + (token == null ? 0 : token.length()));
    }

    public String getToken() {
        String token = prefs.getString(KEY_TOKEN, null);
        Log.d("TokenManager", "Read token length=" + (token == null ? 0 : token.length()));
        return token;
    }

    public void clear() {
        prefs.edit().remove(KEY_TOKEN).apply();
    }

    public String getAuthHeader() {
        String token = getToken();
        return token == null || token.isEmpty() ? null : "Bearer " + token;
    }
}
