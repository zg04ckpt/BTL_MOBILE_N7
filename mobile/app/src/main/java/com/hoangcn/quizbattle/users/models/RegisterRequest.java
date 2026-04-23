package com.hoangcn.quizbattle.users.models;

public class RegisterRequest {
    private String displayName;
    private String email;
    private String phoneNumber;
    private String password;

    public RegisterRequest(String displayName, String email, String phoneNumber, String password) {
        this.displayName = displayName;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.password = password;
    }

    public String getDisplayName() {
        return displayName;
    }

    public String getEmail() {
        return email;
    }

    public String getPhoneNumber() {
        return phoneNumber;
    }

    public String getPassword() {
        return password;
    }
}
