package com.n7.quizbattle.home_rank.repositories;

public interface RepositoryCallback<T> {
    void onSuccess(T data);
    void onError(String message);
}
