package com.hoangcn.quizbattle.battles.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.bitmap.CircleCrop;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.models.MatchProgressUserItem;

import java.util.List;

public class MatchProgressAdapter extends ArrayAdapter<MatchProgressUserItem> {
    private final int totalQuestions;

    public MatchProgressAdapter(@NonNull Context context, @NonNull List<MatchProgressUserItem> objects, int totalQuestions) {
        super(context, 0, objects);
        this.totalQuestions = totalQuestions;
    }

    @NonNull
    @Override
    public View getView(int position, @Nullable View convertView, @NonNull ViewGroup parent) {
        if (convertView == null) {
            convertView = LayoutInflater.from(getContext()).inflate(R.layout.item_match_progress, parent, false);
        }

        MatchProgressUserItem item = getItem(position);
        if (item != null) {
            TextView tvRank = convertView.findViewById(R.id.tvRank);
            ImageView ivAvatar = convertView.findViewById(R.id.ivAvatar);
            TextView tvDisplayName = convertView.findViewById(R.id.tvDisplayName);
            TextView tvStatus = convertView.findViewById(R.id.tvStatus);
            TextView tvScore = convertView.findViewById(R.id.tvScore);
            TextView tvProgress = convertView.findViewById(R.id.tvProgress);

            tvRank.setText(String.valueOf(item.getRank()));
            tvDisplayName.setText(item.getDisplayName());
            tvScore.setText(item.getScore() + " pts");
            tvProgress.setText(item.getProgress() + "/" + Math.max(1, totalQuestions));

            if (item.isFinished()) {
                tvStatus.setText("Đã hoàn thành");
                tvStatus.setTextColor(getContext().getResources().getColor(R.color.color7));
            } else {
                tvStatus.setText("Đang làm bài...");
                tvStatus.setTextColor(getContext().getResources().getColor(R.color.color5));
            }

            Glide.with(getContext())
                    .load(item.getAvatarUrl() != null && !item.getAvatarUrl().isEmpty() ? item.getAvatarUrl() : R.drawable.avatar_current_user)
                    .transform(new CircleCrop())
                    .placeholder(R.drawable.avatar_current_user)
                    .into(ivAvatar);
        }

        return convertView;
    }
}
