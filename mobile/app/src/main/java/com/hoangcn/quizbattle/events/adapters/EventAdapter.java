package com.hoangcn.quizbattle.events.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.listeners.OnItemClicked;
import com.hoangcn.quizbattle.events.models.EventModel;

import java.util.List;

public class EventAdapter extends RecyclerView.Adapter<EventAdapter.ViewHolder> {
    private List<EventModel> events;
    private OnItemClicked<EventModel> listener;

    public EventAdapter(List<EventModel> events) {
        this.events = events;
    }

    public void setOnItemClickListener(OnItemClicked<EventModel> onItemClicked) {
        this.listener = onItemClicked;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new ViewHolder(
                LayoutInflater.from(parent.getContext()).inflate(R.layout.item_event, parent, false)
        );
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.bind(events.get(position));
    }

    @Override
    public int getItemCount() {
        return events.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {
        TextView tvEventName;
        TextView tvEventTimeType;
        Button btnJoinEvent;
        ImageView ivBg;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            tvEventName = itemView.findViewById(R.id.tv_event_name);
            tvEventTimeType = itemView.findViewById(R.id.tv_event_time_type);
            btnJoinEvent = itemView.findViewById(R.id.btn_join_event);
            ivBg = itemView.findViewById(R.id.ivBg);
        }

        public void bind(EventModel event) {
            tvEventName.setText(event.getName());

            if ("Daily".equals(event.getTimeType())) {
                tvEventTimeType.setText("Sự kiện hằng ngày");
                tvEventTimeType.setBackgroundTintList(ContextCompat.getColorStateList(itemView.getContext(), R.color.color9));
            } else if ("Seasonal".equals(event.getTimeType())) {
                tvEventTimeType.setText("Sự kiện mùa");
                tvEventTimeType.setBackgroundTintList(ContextCompat.getColorStateList(itemView.getContext(), R.color.color1));
            } else if ("Limited".equals(event.getTimeType())) {
                tvEventTimeType.setText("Sự kiện giới hạn");
                tvEventTimeType.setBackgroundTintList(ContextCompat.getColorStateList(itemView.getContext(), R.color.color11));
            }

            if (event.getType().equals("LuckySpin")) {
                ivBg.setImageResource(R.drawable.lucky_spin);
            } else if (event.getType().equals("QuizMilestoneChallenge")) {
                ivBg.setImageResource(R.drawable.quiz_milestone_challenge);
            } else if (event.getType().equals("TournamentRewards")) {
                ivBg.setImageResource(R.drawable.tournament_season_rewards);
            }


            btnJoinEvent.setOnClickListener(l -> listener.onItemClicked(event));
        }

    }
}
