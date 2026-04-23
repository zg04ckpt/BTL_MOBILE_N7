package com.hoangcn.quizbattle.events.views;
// Sửa package name chuẩn nhé

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.RectF;
import android.util.AttributeSet;
import android.view.View;
 // Import class Model

import com.hoangcn.quizbattle.events.models.luckyspin.WheelItemModel;

import java.util.ArrayList;
import java.util.List;

public class LuckyWheelView extends View {
    private List<WheelItemModel> items = new ArrayList<>();

    private Paint paintArc;
    private Paint paintIcon;
    private RectF rectF;
    private float centerX;
    private float centerY;
    private float radius;
    private int iconSize; // Kích thước của icon trên ô

    public LuckyWheelView(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    private void init() {
        paintArc = new Paint(Paint.ANTI_ALIAS_FLAG);
        paintArc.setStyle(Paint.Style.FILL);

        paintIcon = new Paint(Paint.ANTI_ALIAS_FLAG);
        paintIcon.setFilterBitmap(true);
        paintIcon.setDither(true);
    }

    public void setData(List<WheelItemModel> newItems) {
        this.items = newItems;
        invalidate();
    }

    @Override
    protected void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);
        int padding = 30;
        int size = Math.min(w, h) - padding * 2;
        rectF = new RectF(padding, padding, padding + size, padding + size);

        centerX = rectF.centerX();
        centerY = rectF.centerY();
        radius = size / 2f;

        iconSize = (int) (radius * 0.35f);
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        if (items == null || items.isEmpty() || rectF == null) return;

        int n = items.size();
        float sweepAngle = 360f / n;
        float startAngle = 0f;

        for (int i = 0; i < n; i++) {
            paintArc.setColor(getPaletteColor(i));
            canvas.drawArc(rectF, startAngle, sweepAngle, true, paintArc);

            // 2. Vẽ Icon vào giữa ô
            canvas.save();

            // Tìm góc chính giữa của ô này
            float midAngle = startAngle + (sweepAngle / 2);
            canvas.rotate(midAngle, centerX, centerY);
//            canvas.rotate(midAngle - 90f, centerX, centerY);

            // Chuyển đổi icon Resource ID thành Bitmap
            Bitmap originalBitmap = BitmapFactory.decodeResource(getResources(), items.get(i).iconResource);
            if (originalBitmap != null) {
                // Resize bitmap cho đúng kích thước quy định
                Bitmap scaledBitmap = Bitmap.createScaledBitmap(originalBitmap, iconSize, iconSize, false);

                // Tính toán vị trí trung tâm để đặt icon.
                // Ta muốn đẩy nó ra xa tâm một đoạn (ví dụ 60% bán kính)
                float positionFactor = radius * 0.6f;
                float iconX = centerX + positionFactor - (iconSize / 2f);
                float iconY = centerY - (iconSize / 2f);

                canvas.drawBitmap(scaledBitmap, iconX, iconY, paintIcon);

                // Giải phóng bộ nhớ (Bắt buộc trong game Android để tránh giật lag)
                if (scaledBitmap != originalBitmap) {
                    scaledBitmap.recycle();
                }
                originalBitmap.recycle();
            }

            canvas.restore(); // Phục hồi trạng thái về góc 0 để vẽ ô tiếp theo

            startAngle += sweepAngle;
        }
    }

    // Hàm Palette màu tương tự mẫu (Vàng nhạt, vàng đậm, cam đậm)
    private int getPaletteColor(int index) {
        switch (index % 3) {
            case 0: return Color.parseColor("#FFD166"); // Vàng nhạt (Cream)
            case 1: return Color.parseColor("#FF9F1C"); // Vàng đậm (Golden)
            default: return Color.parseColor("#E56B1A"); // Cam đậm (Amber)
        }
    }
}