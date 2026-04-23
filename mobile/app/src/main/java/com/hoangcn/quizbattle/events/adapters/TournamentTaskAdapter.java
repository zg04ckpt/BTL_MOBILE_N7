package com.hoangcn.quizbattle.events.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.models.RewardModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskModel;
import com.hoangcn.quizbattle.events.models.tournament.TaskProgressModel;
import com.hoangcn.quizbattle.events.utils.RewardUtil;

import java.util.List;
import java.util.Map;

public class TournamentTaskAdapter extends RecyclerView.Adapter<TournamentTaskAdapter.ViewHolder> {
    private List<TaskModel> tasks;
    private List<TaskProgressModel> progresses;
    private Map<Integer, RewardModel> rewards;
    private OnTaskClickListener listener;

    public interface OnTaskClickListener {
        void onClaimClick(TaskModel task, TaskProgressModel progress);
    }

    public TournamentTaskAdapter(List<TaskModel> tasks, List<TaskProgressModel> progresses, Map<Integer, RewardModel> rewards) {
        this.tasks = tasks;
        this.progresses = progresses;
        this.rewards = rewards;
    }

    public void setOnTaskClickListener(OnTaskClickListener listener) {
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new ViewHolder(
                LayoutInflater.from(parent.getContext()).inflate(R.layout.item_tournament_task, parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        TaskModel task = tasks.get(position);
        TaskProgressModel progress = progresses.stream()
                .filter(p -> p.getTaskId() == task.getTaskId())
                .findFirst()
                .orElse(new TaskProgressModel(task.getTaskId(), false, false));
        holder.bind(task, progress);
    }

    @Override
    public int getItemCount() {
        return tasks.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {
        TextView tvTaskDesc, tvTaskStatus;
        Button btnClaim;
        LinearLayout layoutRewards;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            tvTaskDesc = itemView.findViewById(R.id.tvTaskDesc);
            tvTaskStatus = itemView.findViewById(R.id.tvTaskStatus);
            btnClaim = itemView.findViewById(R.id.btnClaimTask);
            layoutRewards = itemView.findViewById(R.id.layoutRewards);
        }

        public void bind(TaskModel task, TaskProgressModel progress) {
            tvTaskDesc.setText(task.getShortDesc());
            
            layoutRewards.removeAllViews();
            task.getRewards().forEach(r -> {
                RewardModel rewardInfo = rewards.get(r.getEventRewardId());
                if (rewardInfo != null) {
                    View rewardView = LayoutInflater.from(itemView.getContext()).inflate(R.layout.item_quizmilistone_reward, layoutRewards, false);
                    ImageView ivReward = rewardView.findViewById(R.id.ivReward);
                    ivReward.setImageResource(RewardUtil.getRewardIcon(rewardInfo));
                    layoutRewards.addView(rewardView);
                }
            });

            if (progress.isCompleted() && !progress.isRewardClaimed()) {
                btnClaim.setEnabled(true);
                btnClaim.setText("NHẬN");
                btnClaim.setAlpha(1.0f);
                tvTaskStatus.setText("HOÀN THÀNH");
                tvTaskStatus.setTextColor(itemView.getContext().getColor(R.color.color7));
            } else if (progress.isRewardClaimed()) {
                btnClaim.setEnabled(false);
                btnClaim.setText("ĐÃ NHẬN");
                btnClaim.setAlpha(0.5f);
                tvTaskStatus.setText("ĐÃ NHẬN THƯỞNG");
                tvTaskStatus.setTextColor(itemView.getContext().getColor(R.color.color3));
            } else {
                btnClaim.setEnabled(false);
                btnClaim.setText("CHƯA XONG");
                btnClaim.setAlpha(0.5f);
                tvTaskStatus.setText("CHƯA HOÀN THÀNH");
                tvTaskStatus.setTextColor(itemView.getContext().getColor(R.color.color9));
            }

            btnClaim.setOnClickListener(v -> {
                if (listener != null) listener.onClaimClick(task, progress);
            });
        }
    }
}