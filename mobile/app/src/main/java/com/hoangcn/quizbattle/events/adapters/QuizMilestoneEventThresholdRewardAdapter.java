package com.hoangcn.quizbattle.events.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.utils.RewardUtil;

import java.util.List;

public class QuizMilestoneEventThresholdRewardAdapter extends RecyclerView.Adapter<QuizMilestoneEventThresholdRewardAdapter.ViewHolder> {
    private final List<RewardModel> rewards;

    public QuizMilestoneEventThresholdRewardAdapter(List<RewardModel> rewards) {
        this.rewards = rewards;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new ViewHolder(LayoutInflater.from(
                parent.getContext()).inflate(R.layout.item_quizmilistone_reward, parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.bind(rewards.get(position));
    }

    @Override
    public int getItemCount() {
        return rewards.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {
        ImageView ivReward;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            ivReward = itemView.findViewById(R.id.ivReward);
        }

        public void bind(RewardModel reward) {
            ivReward.setImageResource(RewardUtil.getRewardIcon(reward));
        }
    }
}
