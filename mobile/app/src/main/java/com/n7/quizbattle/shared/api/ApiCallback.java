package com.n7.quizbattle.shared.api;

import com.n7.quizbattle.shared.models.ApiResponse;

public interface ApiCallback<T> {
    void onSuccess(ApiResponse<T> data);
    void onError(String message);
}
