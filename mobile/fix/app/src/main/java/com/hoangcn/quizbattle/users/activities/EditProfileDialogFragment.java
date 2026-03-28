package com.hoangcn.quizbattle.users.activities;

import android.content.ContentResolver;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Toast;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;
import com.hoangcn.quizbattle.users.api.UserService;
import com.hoangcn.quizbattle.users.models.UserModel;

import java.io.InputStream;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;

public class EditProfileDialogFragment extends DialogFragment {

    private ImageView ivAvatar;
    private EditText etName;
    private Button btnSave;
    ImageView ivClose;
    View tvChangeAvatar;
    private Uri selectedImageUri;
    private ActivityResultLauncher<String> pickImageLauncher;

    private ProfileUpdateListener updateListener;
    private UserModel initialUser;

    public interface ProfileUpdateListener {
        void onProfileUpdated(UserModel updatedUser);
    }

    @Override
    public void onStart() {
        super.onStart();
        if (getDialog() != null & getDialog().getWindow() != null) {
            getDialog().getWindow().setLayout(
                    (int) (getResources().getDisplayMetrics().widthPixels * 0.9),
                    ViewGroup.LayoutParams.WRAP_CONTENT
            );

            WindowManager.LayoutParams params = getDialog().getWindow().getAttributes();
            params.dimAmount = 0.7f;
            getDialog().getWindow().setAttributes(params);
        }
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.dialog_edit_profile, container, false);

        initViews(view);
        setListeners();
        initData();

        return view;
    }

    private void initViews(View view) {
        if (getDialog() != null & getDialog().getWindow() != null) {
            getDialog().getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        }

        ivAvatar = view.findViewById(R.id.iv_current_avatar);
        ivClose = view.findViewById(R.id.iv_close);
        btnSave = view.findViewById(R.id.btn_save_changes);
        etName = view.findViewById(R.id.et_display_name);
        tvChangeAvatar = view.findViewById(R.id.tv_change_avatar);
    }

    private void setListeners() {
        ivAvatar.setOnClickListener(v -> pickImageLauncher.launch("image/*"));
        tvChangeAvatar.setOnClickListener(v -> pickImageLauncher.launch("image/*"));
        pickImageLauncher = registerForActivityResult(new ActivityResultContracts.GetContent(), uri -> {
            if (uri != null) {
                selectedImageUri = uri;
                ivAvatar.setImageURI(uri);
            }
        });
        ivClose.setOnClickListener(v -> dismiss());
        btnSave.setOnClickListener(v -> submitChanges());
    }

    private void initData() {
        applyInitialUser();
    }

    public void setUpdateListener(ProfileUpdateListener listener) {
        this.updateListener = listener;
    }

    public void setInitialUser(UserModel user) {
        this.initialUser = user;
    }

    private void applyInitialUser() {
        if (initialUser == null) return;
        if (initialUser.getDisplayName() != null) {
            etName.setText(initialUser.getDisplayName());
        }
        String avatarUrl = resolveAvatarUrl(initialUser.getAvatar());
        Glide.with(this).load(avatarUrl)
                .centerCrop()
                .circleCrop()
                .into(ivAvatar);
    }

    private String resolveAvatarUrl(String raw) {
        if (raw == null || raw.trim().isEmpty()) return null;
        if (raw.startsWith("http")) return raw;
        return "https://quizbattle.hoangcn.com" + raw;
    }

    private void submitChanges() {
        String name = etName.getText().toString().trim();
        if (name.isEmpty()) {
            Toast.makeText(getContext(), "Tên không được để trống", Toast.LENGTH_SHORT).show();
            return;
        }

        Context ctx = getContext();
        if (ctx == null) return;

        RequestBody namePart = RequestBody.create(MediaType.parse("text/plain"), name);
        MultipartBody.Part avatarPart = buildAvatarPart(ctx.getContentResolver());

        UserService api = new UserService(ctx);
        api.updateProfile(namePart, avatarPart, new ApiCallback<UserModel>() {
            @Override
            public void onSuccess(ApiResponse<UserModel> data) {
                if (getActivity() == null) return;
                getActivity().runOnUiThread(() -> {
                    Toast.makeText(getActivity(), "Cập nhật thành công", Toast.LENGTH_SHORT).show();
                    if (updateListener != null && data != null) {
                        updateListener.onProfileUpdated(data.getData());
                    }
                    dismiss();
                });
            }

            @Override
            public void onError(String message) {
                if (getActivity() == null) return;
                getActivity().runOnUiThread(() -> {
                    Toast.makeText(getActivity(), "Cập nhật thất bại: " + message, Toast.LENGTH_LONG).show();
                });
            }
        });
    }

    private MultipartBody.Part buildAvatarPart(ContentResolver resolver) {
        if (selectedImageUri == null) return null;
        try {
            String mime = resolver.getType(selectedImageUri);
            if (mime == null) mime = "image/*";

            InputStream is = resolver.openInputStream(selectedImageUri);

            byte[] bytes = getBytes(is);

            is.close();
            RequestBody body = RequestBody.create(MediaType.parse(mime), bytes);
            return MultipartBody.Part.createFormData("avatar", "avatar.jpg", body);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    private byte[] getBytes(InputStream inputStream) throws java.io.IOException {
        java.io.ByteArrayOutputStream byteBuffer = new java.io.ByteArrayOutputStream();
        int bufferSize = 1024;
        byte[] buffer = new byte[bufferSize];

        int len = 0;
        while ((len = inputStream.read(buffer)) != -1) {
            byteBuffer.write(buffer, 0, len);
        }
        return byteBuffer.toByteArray();
    }
}
