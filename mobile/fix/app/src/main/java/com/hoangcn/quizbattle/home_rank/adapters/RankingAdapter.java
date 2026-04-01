package com.hoangcn.quizbattle.home_rank.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.home_rank.listeners.OnUserRankClicked;
import com.hoangcn.quizbattle.home_rank.models.UserRankListItem;

import java.util.List;

public class RankingAdapter extends RecyclerView.Adapter<RankingAdapter.ViewHolder> {

    private final List<UserRankListItem> items;
    private OnUserRankClicked listener;

    public RankingAdapter(List<UserRankListItem> items) {
        this.items = items;
    }

    public void setOnItemClickListener(OnUserRankClicked listener) {
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_ranking, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.bind(items.get(position));
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

     class ViewHolder extends RecyclerView.ViewHolder {
        private final TextView tvRank;
        private final TextView tvUserName;
        private final TextView tvScore;
        private final TextView tvLevel;
        private final ImageView ivAvatar;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            tvRank = itemView.findViewById(R.id.tvRank);
            tvUserName = itemView.findViewById(R.id.tvUserName);
            tvScore = itemView.findViewById(R.id.tvScore);
            ivAvatar = itemView.findViewById(R.id.ivAvatar);
            tvLevel = itemView.findViewById(R.id.tvLevel);
        }

        public void bind(UserRankListItem item) {
            tvRank.setText(String.valueOf(item.getRank()));
            tvUserName.setText(item.getDisplayName());
            tvScore.setText(item.getRankScore() + " point");
            Glide.with(itemView.getContext())
                    .load(item.getAvatarUrl())
                    .centerCrop()
                    .circleCrop()
                    .into(ivAvatar);
            itemView.setOnClickListener(v -> {
                if (listener != null) {
                    listener.onUserRankClicked(item);
                }
            });
            tvLevel.setText("lv" + item.getLevel());
        }
    }
}
