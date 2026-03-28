package com.hoangcn.quizbattle.lobby.activities;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.flexbox.FlexboxLayout;
import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.lobby.api.LobbyService;
import com.hoangcn.quizbattle.lobby.models.JoinLobbyRequest;
import com.hoangcn.quizbattle.lobby.models.JoinLobbyResponse;
import com.hoangcn.quizbattle.lobby.models.OptionItem;
import com.hoangcn.quizbattle.lobby.models.StartOptionsResponse;
import com.hoangcn.quizbattle.lobby.models.TopicItem;
import com.hoangcn.quizbattle.shared.api.ApiCallback;
import com.hoangcn.quizbattle.shared.models.ApiResponse;

import java.util.List;

public class MatchConfigActivity extends AppCompatActivity {

    private LinearLayout llBattleTypes, llNumberOfPlayers;
    private FlexboxLayout flTopicOptions;
    private ImageView btnBack;
    private Button btnConfirm;
    private LobbyService lobbyService;

    private String selectedBattleType, selectedContentType;
    private int selectedNumberOfPlayers = -1, selectedTopicId = -1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_match_config);

        lobbyService = new LobbyService(this);

        initUI();
        setListeners();
        initData();
    }

    private void initUI() {
        llBattleTypes = findViewById(R.id.ll_battle_types);
        llNumberOfPlayers = findViewById(R.id.ll_number_of_players);
        flTopicOptions = findViewById(R.id.fl_topic_options);
        btnBack = findViewById(R.id.btnBack);
        btnConfirm = findViewById(R.id.btnConfirm);
    }

    private void setListeners() {
        btnBack.setOnClickListener(v -> finish());
        btnConfirm.setOnClickListener(v -> handleConfirm());
    }

    private void initData() {
        loadOptionsData();
    }

    private void loadOptionsData() {
        lobbyService.getStartOptions(new ApiCallback<StartOptionsResponse>() {
            @Override
            public void onSuccess(ApiResponse<StartOptionsResponse> data) {
                renderAllOptions(data.getData());
            }

            @Override
            public void onError(String message) {
                Log.e("API_ERROR", "Chi tiết lỗi: " + message);
                Toast.makeText(MatchConfigActivity.this, "Lỗi: " + message, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void renderAllOptions(StartOptionsResponse data) {
        if(data == null) return;
        llBattleTypes.removeAllViews();
        llNumberOfPlayers.removeAllViews();
        flTopicOptions.removeAllViews();

        render(llBattleTypes, data.getTypesOfBattle(), "TYPE");
        render(llNumberOfPlayers, data.getNumberOfPlayers(), "PLAYERS");

        List<OptionItem> filteredContentTypes = data.getContentTypes();
        if (filteredContentTypes != null && filteredContentTypes.size() >= 2) {
            filteredContentTypes = filteredContentTypes.subList(0, 2);
        }

        render(flTopicOptions, filteredContentTypes, "CONTENT_TYPE");
        render(flTopicOptions, data.getTopicsOfContent(), "TOPIC");
    }

    private void render(ViewGroup container, List<?> items, String category) {
        if(items == null) return;
        for(Object item: items) {
            View itemView = LayoutInflater.from(this).inflate(R.layout.item_option_card, container, false);
            TextView tvLabel = itemView.findViewById(R.id.tv_label);

            if(item instanceof OptionItem) tvLabel.setText(((OptionItem) item).getLabel());
            else if(item instanceof TopicItem) tvLabel.setText(((TopicItem) item).getName());
            else if(item instanceof Integer) tvLabel.setText(item + " Người");

            itemView.setOnClickListener(v -> {
                updateUISelection(container, itemView);
                saveSelection(category, item);
            });

            container.addView(itemView);
        }
    }

    private void updateUISelection(ViewGroup container, View selectedView) {
        for(int i = 0; i < container.getChildCount(); i++) {
            View child = container.getChildAt(i);
            child.setBackgroundResource(R.drawable.bg_option_card);
            ((TextView) child.findViewById(R.id.tv_label)).setTextColor(getResources().getColor(R.color.color1));
        }

        selectedView.setBackgroundResource(R.drawable.bg_option_card_selected);
        ((TextView) selectedView.findViewById(R.id.tv_label)).setTextColor(Color.WHITE);
    }

    private void saveSelection(String category, Object item) {
        if(category.equals("TYPE")) {
            OptionItem option = (OptionItem) item;
            selectedBattleType = option.getType();

            if("Single".equals(selectedBattleType)) {
                selectedNumberOfPlayers = 1;
                setPlayerCountEnabled(false);
            } else {
                selectedNumberOfPlayers = -1;
                setPlayerCountEnabled(true);
            }
        }
        else if(category.equals("PLAYERS")) selectedNumberOfPlayers = (Integer) item;
        else if(category.equals("TOPIC")) {
            selectedTopicId = ((TopicItem) item).getId();
            selectedContentType = "OnlyOne";
        }
        else if(category.equals("CONTENT_TYPE")) {
            selectedContentType = ((OptionItem) item).getType();
            selectedTopicId = -1;
        }
    }

    private void setPlayerCountEnabled(boolean enabled) {
        llNumberOfPlayers.setAlpha(enabled ? 1.0f : 0.5f);

        for(int i = 0; i < llNumberOfPlayers.getChildCount(); i++) {
            View child = llNumberOfPlayers.getChildAt(i);
            child.setEnabled(enabled);

            child.setBackgroundResource(R.drawable.bg_option_card);
            ((TextView) child.findViewById(R.id.tv_label)).setTextColor(getResources().getColor(R.color.color1));
        }
    }

    private void handleConfirm() {
        if(selectedBattleType == null) {
            Toast.makeText(this, "Vui lòng chọn kiểu thi đấu", Toast.LENGTH_SHORT).show();
            return;
        }
        if(selectedNumberOfPlayers == -1) {
            Toast.makeText(this, "Vui lòng chọn số lượng người chơi", Toast.LENGTH_SHORT).show();
            return;
        }
        if((selectedContentType == null) || ("OnlyOne".equals(selectedContentType) && selectedTopicId == -1)) {
            Toast.makeText(this, "Vui lòng chọn chủ đề thi đấu", Toast.LENGTH_SHORT).show();
            return;
        }

        Integer topicIdParam = null;
        if("OnlyOne".equals(selectedContentType)) {
            topicIdParam = selectedTopicId;
        }

        JoinLobbyRequest request = new JoinLobbyRequest(
                topicIdParam,
                selectedContentType,
                selectedNumberOfPlayers,
                selectedBattleType);

        lobbyService.joinLobby(request, new ApiCallback<JoinLobbyResponse>() {
            @Override
            public void onSuccess(ApiResponse<JoinLobbyResponse> data) {
                String roomId = data.getData().getLobbyRoomId();
                Intent intent = new Intent(MatchConfigActivity.this, MatchMakingActivity.class);
                intent.putExtra("lobbyRoomId", roomId.trim());
                startActivity(intent);
            }

            @Override
            public void onError(String message) {
                Toast.makeText(MatchConfigActivity.this, "Lỗi: " + message, Toast.LENGTH_SHORT).show();
            }
        });
    }
}