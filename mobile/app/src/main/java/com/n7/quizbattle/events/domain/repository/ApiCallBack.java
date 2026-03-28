package com.n7.quizbattle.events.domain.repository;

public interface ApiCallBack<T> {
    void onSuccess(T result);

    void onFail(String errorMessage);

}
