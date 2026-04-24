package com.hoangcn.quizbattle.events.utils;

import com.hoangcn.quizbattle.R;
import com.hoangcn.quizbattle.events.models.RewardModel;

public class RewardUtil {
    public static int getRewardIcon(RewardModel reward) {
        if (reward.getType().equals("ExpScore")) {
            return R.drawable.exprience_prize;
        } else if (reward.getType().equals("MatchLoudspeaker")) {
            return R.drawable.match_loudspeaker_prize;
        } else if (reward.getType().equals("RankProtectionCard")) {
            return R.drawable.defend_rank_prize;
        } else if (reward.getType().equals("Gold")) {
            return R.drawable.gold_prize;
        }
        return R.drawable.empty;
    }
}
