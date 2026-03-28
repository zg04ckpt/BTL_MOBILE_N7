package com.n7.quizbattle.users.auth;

/**
 * Convert raw API error messages to user-friendly Vietnamese strings.
 */
public final class UsersErrorMapper {
    private UsersErrorMapper() {
    }

    public static String map(String raw) {
        if (raw == null || raw.trim().isEmpty()) {
            return "Có lỗi xảy ra. Vui lòng thử lại.";
        }

        String msg = raw.toLowerCase();

        // Auth errors
        if (msg.contains("401") || msg.contains("unauthorized") || msg.contains("invalid credential")) {
            return "Tên đăng nhập hoặc mật khẩu không chính xác";
        }

        // Registration conflicts
        if (msg.contains("phone number is already in use") || msg.contains("phone already")) {
            return "Số điện thoại đã được sử dụng";
        }
        if (msg.contains("email is already in use") || msg.contains("email already")) {
            return "Email đã được sử dụng";
        }

        // Generic bad request
        if (msg.contains("400")) {
            return "Thông tin gửi lên không hợp lệ. Vui lòng kiểm tra lại.";
        }

        // Network issues
        if (msg.contains("timeout") || msg.contains("failed to connect") || msg.contains("unable to resolve host")) {
            return "Không thể kết nối tới máy chủ. Vui lòng kiểm tra mạng và thử lại.";
        }

        // Fallback: return original if it's already readable, else generic.
        return raw.startsWith("Loi") ? raw : "Có lỗi xảy ra. Vui lòng thử lại.";
    }
}
