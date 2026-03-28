package com.n7.quizbattle.home_rank.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.n7.quizbattle.R;
import com.n7.quizbattle.home_rank.models.RankBoardItemModel;

import java.util.ArrayList;
import java.util.List;

public class RankingAdapter extends RecyclerView.Adapter<RankingAdapter.RankingViewHolder> {
    public interface OnRankClickListener {
        void onRankClick(RankBoardItemModel item);
    }

    private final List<RankBoardItemModel> items = new ArrayList<>();
    private final OnRankClickListener clickListener;

    public RankingAdapter(OnRankClickListener clickListener) {
        this.clickListener = clickListener;
    }

    public void submitList(List<RankBoardItemModel> data) {
        items.clear();
        if (data != null) {
            items.addAll(data);
        }
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public RankingViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_ranking, parent, false);
        return new RankingViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull RankingViewHolder holder, int position) {
        RankBoardItemModel item = items.get(position);
        holder.tvRank.setText(String.valueOf(item.getRank()));
        holder.tvUserName.setText(item.getDisplayName());
        holder.tvScore.setText(item.getRankScore() + " point");
        holder.ivAvatar.setImageResource(R.drawable.avatar_current_user);

        holder.itemView.setOnClickListener(v -> {
            if (clickListener != null) {
                clickListener.onRankClick(item);
            }
        });
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    static class RankingViewHolder extends RecyclerView.ViewHolder {
        private final TextView tvRank;
        private final TextView tvUserName;
        private final TextView tvScore;
        private final ImageView ivAvatar;

        public RankingViewHolder(@NonNull View itemView) {
            super(itemView);
            tvRank = itemView.findViewById(R.id.tvRank);
            tvUserName = itemView.findViewById(R.id.tvUserName);
            tvScore = itemView.findViewById(R.id.tvScore);
            ivAvatar = itemView.findViewById(R.id.ivAvatar);
        }
    }
}
