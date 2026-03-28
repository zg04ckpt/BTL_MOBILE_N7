package com.n7.quizbattle.home_rank.mappers;

import com.n7.quizbattle.home_rank.api.dtos.UserRankDto;
import com.n7.quizbattle.home_rank.api.dtos.UserRankListItemDto;
import com.n7.quizbattle.home_rank.models.RankBoardItemModel;
import com.n7.quizbattle.home_rank.models.RankDetailModel;
import com.n7.quizbattle.home_rank.models.RankTypeModel;

public final class RankMapper {
    private RankMapper() {
    }

    public static RankBoardItemModel toBoardModel(UserRankListItemDto dto) {
        return new RankBoardItemModel(
                dto.getUserId(),
                dto.getDisplayName(),
                dto.getAvatarUrl(),
                dto.getRank(),
                dto.getRankScore()
        );
    }

    public static RankDetailModel toDetailModel(UserRankDto dto) {
        return new RankDetailModel(
                dto.getUserId(),
                dto.getDisplayName(),
                dto.getAvatarUrl(),
                dto.getRank(),
                dto.getRankScore(),
                dto.getNumberOfMatchs(),
                dto.getWinningRate(),
                dto.getWinningStreak()
        );
    }

    public static RankTypeModel toTypeModel(String value) {
        return new RankTypeModel(value);
    }
}
