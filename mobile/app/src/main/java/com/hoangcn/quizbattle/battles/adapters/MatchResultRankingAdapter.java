package com.hoangcn.quizbattle.battles.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.bumptech.glide.Glide;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.battles.api.BattleService;
import com.hoangcn.quizbattle.battles.models.MatchResultUserItem;

import java.util.ArrayList;
import java.util.List;

public class MatchResultRankingAdapter extends RecyclerView.Adapter<MatchResultRankingAdapter.ViewHolder> {
    private final List<MatchResultUserItem> items = new ArrayList<>();
    private final BattleService service;

    public MatchResultRankingAdapter(Context context) {
        this.service = new BattleService(context);
    }

    public void submit(List<MatchResultUserItem> users) {
        items.clear();
        if (users != null) {
            items.addAll(users);
        }
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_match_result_user, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.bind(items.get(position), position + 1);
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

     class ViewHolder extends RecyclerView.ViewHolder {
        private final TextView tvRank;
        private final TextView tvName;
        private final TextView tvScore;
        private final TextView tvExp;
        private final TextView tvRankScore;
        private final ImageView ivAvatar;
        private final ImageView ivRankProtection;

        ViewHolder(@NonNull View itemView) {
            super(itemView);
            tvRank = itemView.findViewById(R.id.tvRank);
            tvName = itemView.findViewById(R.id.tvName);
            tvScore = itemView.findViewById(R.id.tvScore);
            tvExp = itemView.findViewById(R.id.tvExp);
            tvRankScore = itemView.findViewById(R.id.tvRankScore);
            ivAvatar = itemView.findViewById(R.id.ivAvatar);
            ivRankProtection = itemView.findViewById(R.id.ivRankProtection);
        }

        void bind(MatchResultUserItem u, int rank) {
            tvRank.setText("#" + rank);
            tvName.setText(u.getName() == null || u.getName().isEmpty() ? "Người chơi" : u.getName());
            tvScore.setText(u.getScore() + " điểm");
            tvExp.setText((u.getExpGained() >= 0 ? "+" : "") + u.getExpGained() + " XP");
            if (u.isRankProtected()) {
                tvRankScore.setText("-0 rank");
                ivRankProtection.setVisibility(View.VISIBLE);
            } else {
                tvRankScore.setText((u.getRankScoreGained() >= 0 ? "+" : "") + u.getRankScoreGained() + " rank");
                ivRankProtection.setVisibility(View.GONE);
            }

            Glide.with(itemView.getContext()).load(service.getFullImageUrl(u.getAvatarUrl()))
                .centerCrop()
                .circleCrop()
                .into(ivAvatar);
        }
    }
}
