package com.n7.quizbattle.tests.api;

import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.api.BaseApi;
import com.n7.quizbattle.tests.models.TestModel;

public class TestService extends BaseApi {
    private final TestApi api;

    public TestService() {
        this.api = getRetrofit().create(TestApi.class);
    }

    public void testApiRunning(ApiCallback<TestModel> callback) {
        enqueue(api.testApiRunning(), callback);
    }
}
