package com.n7.quizbattle.home_rank.repositories;

import android.content.Context;
import android.content.SharedPreferences;

public class SessionManager {
    private static final String PREF_NAME = "quizbattle_prefs";
    private static final String KEY_ACCESS_TOKEN = "access_token";
    private static final String KEY_USER_ID = "user_id";

    private final SharedPreferences prefs;

    public SessionManager(Context context) {
        this.prefs = context.getSharedPreferences(PREF_NAME, Context.MODE_PRIVATE);
    }

    public void saveSession(String accessToken, int userId) {
        prefs.edit()
                .putString(KEY_ACCESS_TOKEN, accessToken)
                .putInt(KEY_USER_ID, userId)
                .apply();
    }

    public String getAccessToken() {
        return prefs.getString(KEY_ACCESS_TOKEN, "");
    }

    public int getUserId() {
        return prefs.getInt(KEY_USER_ID, -1);
    }

    public void saveUserId(int userId) {
        prefs.edit().putInt(KEY_USER_ID, userId).apply();
    }

    public boolean hasToken() {
        String token = getAccessToken();
        return token != null && !token.isEmpty();
    }

    public void clear() {
        prefs.edit().remove(KEY_ACCESS_TOKEN).remove(KEY_USER_ID).apply();
    }
}
