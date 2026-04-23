package com.hoangcn.quizbattle.events.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.listeners.OnItemClicked;
import com.hoangcn.quizbattle.events.models.EventProgressModel;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.quizmilestone.ThresholdModel;
import com.hoangcn.quizbattle.events.models.quizmilestone.ThresholdProgressModel;

import java.util.HashMap;
import java.util.List;
import java.util.stream.Collectors;

public class QuizMilestoneEventThresholdAdapter extends RecyclerView.Adapter<QuizMilestoneEventThresholdAdapter.ViewHolder> {
    private final HashMap<Integer, RewardModel> rewards;
    private List<ThresholdModel> thresholds;
    private List<ThresholdProgressModel> progresses;
    private OnItemClicked<ThresholdProgressModel> listener;

    public QuizMilestoneEventThresholdAdapter(List<ThresholdModel> thresholds,
                                              List<ThresholdProgressModel> progresses,
                                              HashMap<Integer, RewardModel> rewards) {
        this.thresholds = thresholds;
        this.progresses = progresses;
        this.rewards = rewards;
    }


    public void setOnItemClickedListener(OnItemClicked<ThresholdProgressModel> listener) {
        this.listener = listener;
    }


    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new ViewHolder(
                LayoutInflater.from(parent.getContext()).inflate(R.layout.item_quizmilestone_progress, parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        // Vị trí đầu tiên hoặc không hoàn thành tiến độ
        boolean isCurrent = !progresses.get(position).isCompleted() &&
                (position == 0 || progresses.get(position - 1).isCompleted());

        holder.bind(thresholds.get(position), progresses.get(position), isCurrent);
    }

    @Override
    public int getItemCount() {
        return thresholds.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {

        RecyclerView rvRewards;
        TextView tvNumberOfQuestions;
        Button btnStartOrClaim;
        ImageView ivIcon;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            rvRewards = itemView.findViewById(R.id.rvRewards);
            tvNumberOfQuestions = itemView.findViewById(R.id.tvNumberOfQuestions);
            btnStartOrClaim = itemView.findViewById(R.id.btnStartOrClaim);
            ivIcon = itemView.findViewById(R.id.ivIcon);
        }
        public void bind(ThresholdModel threshold, ThresholdProgressModel progress, boolean isCurrent) {
            tvNumberOfQuestions.setText(threshold.getChallengeQuestionIds().size() + " Câu hỏi");
            rvRewards.setAdapter(new QuizMilestoneEventThresholdRewardAdapter(
                    threshold.getRewards().stream().map(r -> rewards.get(r.getEventRewardId()))
                            .collect(Collectors.toList())));

            // Cập nhật nút Start/Claim dựa trên trạng thái tiến độ
            if (isCurrent) {
                btnStartOrClaim.setVisibility(View.VISIBLE);
                btnStartOrClaim.setText("Bắt đầu ngay");
            } else if (progress.isCompleted() && !progress.isRewardClaimed()) {
                btnStartOrClaim.setVisibility(View.VISIBLE);
                btnStartOrClaim.setText("Nhận phần thưởng");
                btnStartOrClaim.setBackgroundTintList(itemView.getResources().getColorStateList(R.color.color9));
            } else {
                btnStartOrClaim.setVisibility(View.INVISIBLE);
            }

            // Cập nhật viền icon dựa trên trạng thái tiến độ
            if (progress.isCompleted()) {
                ivIcon.setBackgroundResource(R.drawable.bg_img_avatar);
            } else {
                ivIcon.setBackgroundResource(R.drawable.bg_img_avatar_gray);
            }

            btnStartOrClaim.setOnClickListener(l -> listener.onItemClicked(progress));
        }

    }
}
