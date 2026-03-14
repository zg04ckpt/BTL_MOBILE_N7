package com.n7.quizbattle.tests.api;

import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.tests.models.TestModel;

import retrofit2.Call;
import retrofit2.http.GET;

public interface TestApi {
    @GET("tests")
    Call<ApiResponse<TestModel>> testApiRunning();
}
