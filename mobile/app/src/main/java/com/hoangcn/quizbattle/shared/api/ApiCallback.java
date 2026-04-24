package com.hoangcn.quizbattle.shared.api;

import com.hoangcn.quizbattle.shared.models.ApiResponse;

public interface ApiCallback<T> {
    void onSuccess(ApiResponse<T> data);
    void onError(String message);
}
