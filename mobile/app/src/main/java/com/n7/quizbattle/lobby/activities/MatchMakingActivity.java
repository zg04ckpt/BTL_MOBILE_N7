package com.n7.quizbattle.lobby.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.n7.quizbattle.R;

public class MatchMakingActivity extends AppCompatActivity {

    private Button btn_cancel_match;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_match_making);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        initEvents();
    }

    private void initViews() {
        btn_cancel_match = findViewById(R.id.btn_cancel_match);
    }

    private void initEvents() {
        btn_cancel_match.setOnClickListener(v -> {
            showCancelConfirmationSheet();
        });
    }

    private void showCancelConfirmationSheet() {
        BottomSheetDialog bottomSheetDialog = new BottomSheetDialog(this, R.style.BottomSheetDialogTheme);
        View sheetView = getLayoutInflater().inflate(R.layout.layout_bottom_sheet_cancel_match, null);

        View btnConfirm = sheetView.findViewById(R.id.btn_confirm_cancel);
        View btnStay = sheetView.findViewById(R.id.btn_stay_wait);

        btnConfirm.setOnClickListener(v -> {
            bottomSheetDialog.dismiss();
            finish();
        });

        btnStay.setOnClickListener(v -> {
            bottomSheetDialog.dismiss();
        });

        bottomSheetDialog.setContentView(sheetView);
        bottomSheetDialog.show();
    }
}