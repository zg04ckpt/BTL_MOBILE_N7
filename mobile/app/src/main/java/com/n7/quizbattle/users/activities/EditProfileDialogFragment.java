package com.n7.quizbattle.users.activities;

import android.app.Activity;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.net.Uri;
import android.os.AsyncTask;
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

import com.n7.quizbattle.R;
import com.n7.quizbattle.shared.api.ApiCallback;
import com.n7.quizbattle.shared.models.ApiResponse;
import com.n7.quizbattle.users.api.UsersApi;
import com.n7.quizbattle.users.auth.UsersErrorMapper;
import com.n7.quizbattle.users.models.UserModel;

import java.io.InputStream;
import java.lang.ref.WeakReference;
import java.net.HttpURLConnection;
import java.net.URL;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;

public class EditProfileDialogFragment extends DialogFragment {

    private ImageView ivAvatar;
    private EditText etName;
    private Uri selectedImageUri;
    private ActivityResultLauncher<String> pickImageLauncher;
    private ProfileUpdateListener updateListener;
    private UserModel initialUser;

    public interface ProfileUpdateListener {
        void onProfileUpdated(UserModel updatedUser);
    }

    public void setUpdateListener(ProfileUpdateListener listener) {
        this.updateListener = listener;
    }

    public void setInitialUser(UserModel user) {
        this.initialUser = user;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        if (getDialog() != null & getDialog().getWindow() != null) {
            getDialog().getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        }

        View view = inflater.inflate(R.layout.dialog_edit_profile, container, false);

        ivAvatar = view.findViewById(R.id.iv_current_avatar);
        ImageView ivClose = view.findViewById(R.id.iv_close);
        Button btnSave = view.findViewById(R.id.btn_save_changes);
        etName = view.findViewById(R.id.et_display_name);
        View tvChangeAvatar = view.findViewById(R.id.tv_change_avatar);

        setupPickImage();

        ivAvatar.setOnClickListener(v -> pickImageLauncher.launch("image/*"));
        tvChangeAvatar.setOnClickListener(v -> pickImageLauncher.launch("image/*"));

        applyInitialUser();

        ivClose.setOnClickListener(v -> dismiss());
        btnSave.setOnClickListener(v -> submitChanges());

        return view;
    }

    private void setupPickImage() {
        pickImageLauncher = registerForActivityResult(new ActivityResultContracts.GetContent(), uri -> {
            if (uri != null) {
                selectedImageUri = uri;
                ivAvatar.setImageURI(uri);
            }
        });
    }

    private void applyInitialUser() {
        if (initialUser == null) return;
        if (initialUser.getDisplayName() != null) {
            etName.setText(initialUser.getDisplayName());
        }
        String avatarUrl = resolveAvatarUrl(initialUser.getAvatar());
        if (avatarUrl != null) {
            new ImageLoadTask(avatarUrl, ivAvatar).executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR);
        }
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

        UsersApi api = new UsersApi(ctx);
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
                getActivity().runOnUiThread(() -> Toast.makeText(getActivity(), UsersErrorMapper.map(message), Toast.LENGTH_LONG).show());
            }
        });
    }

    private MultipartBody.Part buildAvatarPart(ContentResolver resolver) {
        if (selectedImageUri == null) return null;
        try {
            String mime = resolver.getType(selectedImageUri);
            if (mime == null) mime = "image/*";
            InputStream is = resolver.openInputStream(selectedImageUri);
            byte[] bytes = is.readAllBytes();
            is.close();
            RequestBody body = RequestBody.create(MediaType.parse(mime), bytes);
            return MultipartBody.Part.createFormData("avatar", "avatar.jpg", body);
        } catch (Exception e) {
            return null;
        }
    }

    private static class ImageLoadTask extends AsyncTask<Void, Void, Bitmap> {
        private final String url;
        private final WeakReference<ImageView> ivRef;

        ImageLoadTask(String url, ImageView iv) {
            this.url = url;
            this.ivRef = new WeakReference<>(iv);
        }

        @Override
        protected Bitmap doInBackground(Void... voids) {
            try {
                URL u = new URL(url);
                HttpURLConnection conn = (HttpURLConnection) u.openConnection();
                conn.setConnectTimeout(5000);
                conn.setReadTimeout(5000);
                conn.setInstanceFollowRedirects(true);
                InputStream is = conn.getInputStream();
                Bitmap b = BitmapFactory.decodeStream(is);
                is.close();
                return b;
            } catch (Exception e) {
                return null;
            }
        }

        @Override
        protected void onPostExecute(Bitmap bitmap) {
            ImageView iv = ivRef.get();
            if (bitmap != null && iv != null) {
                iv.setImageBitmap(bitmap);
            }
        }
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
}
