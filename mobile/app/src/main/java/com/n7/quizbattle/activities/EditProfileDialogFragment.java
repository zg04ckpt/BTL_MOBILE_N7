package com.n7.quizbattle.activities;

import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;

import com.n7.quizbattle.R;

public class EditProfileDialogFragment extends DialogFragment {

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        if(getDialog() != null & getDialog().getWindow() != null){
            getDialog().getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        }

        View view = inflater.inflate(R.layout.dialog_edit_profile, container, false);

        ImageView ivClose = view.findViewById(R.id.iv_close);
        Button btnSave = view.findViewById(R.id.btn_save_changes);
        EditText etName = view.findViewById(R.id.et_display_name);

        etName.setText("Nguyễn Văn A");
        etName.setSelection(etName.getText().length());

        ivClose.setOnClickListener(v -> dismiss());

        btnSave.setOnClickListener(v -> dismiss());

        return view;
    }

    @Override
    public void onStart() {
        super.onStart();
        if(getDialog() != null & getDialog().getWindow() != null) {
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
