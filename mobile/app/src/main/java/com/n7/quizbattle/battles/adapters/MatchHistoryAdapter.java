package com.n7.quizbattle.battles.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;

import com.n7.quizbattle.R;
import com.n7.quizbattle.battles.models.MatchHistory;

import java.util.List;

public class MatchHistoryAdapter extends RecyclerView.Adapter<MatchHistoryAdapter.MatchViewHolder> {
    private List<MatchHistory> matchHistoryList;

    public MatchHistoryAdapter(List<MatchHistory> matchHistoryList) {
        this.matchHistoryList = matchHistoryList;
    }

    @NonNull
    @Override
    public MatchViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_history_match, parent, false);
        return new MatchViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MatchViewHolder holder, int position) {
        MatchHistory match = matchHistoryList.get(position);

        holder.ivAvatar.setImageResource(match.getAvatarRes());
        holder.tvName.setText(match.getName());
        holder.tvTime.setText(match.getTime());
        String textRp = match.getRp();
        if(match.isWin()) textRp = "+" + textRp + "RP";
        else textRp = "-" + textRp + "RP";
        holder.tvRp.setText(textRp);

        if(match.isWin()) {
            int colorWin = ContextCompat.getColor(holder.itemView.getContext(), R.color.color7);
            holder.tvRp.setTextColor(colorWin);
        } else {
            int colorLose = ContextCompat.getColor(holder.itemView.getContext(), R.color.color6);
            holder.tvRp.setTextColor(colorLose);
        }
    }

    @Override
    public int getItemCount() {
        return matchHistoryList !=null ? matchHistoryList.size() : 0;
    }


    public static class MatchViewHolder extends RecyclerView.ViewHolder {
        ImageView ivAvatar;
        TextView tvName, tvTime, tvRp;

        public MatchViewHolder(@NonNull View itemView) {
            super(itemView);
            ivAvatar = itemView.findViewById(R.id.iv_opponent_avatar);
            tvName = itemView.findViewById(R.id.tv_opponent_name);
            tvTime = itemView.findViewById(R.id.tv_match_time);
            tvRp = itemView.findViewById(R.id.tv_rp_change);
        }
    }
}