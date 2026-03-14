package com.n7.quizbattle.shared.models;

import com.google.gson.annotations.SerializedName;

public class ApiResponse<T> {
    @SerializedName("isSuccess")
    private boolean isSuccess;

    @SerializedName("message")
    private String message;

    @SerializedName("data")
    private T data;

    // Required for Gson
    public ApiResponse() {
    }

    public ApiResponse(boolean isSuccess, String message, T data) {
        this.isSuccess = isSuccess;
        this.message = message;
        this.data = data;
    }

    public boolean isSuccess() {
        return isSuccess;
    }

    public void setSuccess(boolean success) {
        isSuccess = success;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }
}
