package com.hoangcn.quizbattle.home_rank.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.home_rank.listeners.OnHomeEventClicked;
import com.hoangcn.quizbattle.home_rank.models.OngoingEvent;

import java.util.List;
import java.util.Objects;

public class HomeEventAdapter extends RecyclerView.Adapter<HomeEventAdapter.ViewHolder> {
    private List<OngoingEvent> events;
    private OnHomeEventClicked listener;

    public HomeEventAdapter(List<OngoingEvent> events) {
        this.events = events;
    }

    public void setOnItemClickedListener(OnHomeEventClicked listener) {
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new ViewHolder(LayoutInflater.from(parent.getContext()).inflate(
                R.layout.item_home_event, parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.bind(events.get(position), position);
    }

    @Override
    public int getItemCount() {
        return events.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {
        ImageView ivBanner;
        TextView tvName, tvOrder;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            ivBanner = itemView.findViewById(R.id.ivHomeEventBanner);
            tvName = itemView.findViewById(R.id.tvHomeEventName);
            tvOrder = itemView.findViewById(R.id.tvOrder);
        }

        public void bind(OngoingEvent event, int postition) {
            if (Objects.equals(event.getType(), "LuckySpin")) {
                ivBanner.setImageResource(R.drawable.lucky_spin);
            } else if (Objects.equals(event.getType(), "TournamentRewards")) {
                ivBanner.setImageResource(R.drawable.tournament_season_rewards);
            } else if (Objects.equals(event.getType(), "QuizMilestoneChallenge")) {
                ivBanner.setImageResource(R.drawable.quiz_milestone_challenge);
            }

            tvName.setText(event.getName());
            tvOrder.setText((postition + 1) + "/" + events.size());

            itemView.setOnClickListener(v -> {
                listener.onHomeEventClicked(event);
            });
        }
    }
}
