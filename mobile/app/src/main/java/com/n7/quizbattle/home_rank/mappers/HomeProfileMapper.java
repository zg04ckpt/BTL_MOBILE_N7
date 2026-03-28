package com.n7.quizbattle.home_rank.mappers;

import com.n7.quizbattle.home_rank.api.dtos.UserProfileDto;
import com.n7.quizbattle.home_rank.models.HomeProfileModel;

public final class HomeProfileMapper {
    private HomeProfileMapper() {
    }

    public static HomeProfileModel fromDto(UserProfileDto dto) {
        return new HomeProfileModel(
                dto.getId(),
                dto.getName(),
                dto.getAvatarUrl(),
                dto.getLevel(),
                dto.getExp(),
                dto.getRank(),
                dto.getRankScore(),
                dto.getNumberOfMatchs(),
                dto.getWinningRate(),
                dto.getWinningStreak()
        );
    }
}
