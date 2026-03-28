export interface EventDto {
    id: number;
    name: string;
    desc: string;
    startTime: Date;
    endTime?: Date;
    timeType: EventTimeType;
    type: EventType;
    isLocked: boolean;
}

export interface TournamentRewardsEventDto extends EventDto {
    tasks: TournamentRewardsTaskDto[];
}

export interface TournamentRewardsProgressDto extends UserInEventProgressDto {
    taskProgresses: TournamentRewardsProgressInfoDto[];
}

export interface TournamentRewardsProgressInfoDto {
    taskId: number;
    rewardClaimed: boolean;
    completed: boolean;
}

export interface TournamentRewardsTaskDto {
    taskId: number;
    shortDesc: string;
    rewards: EventRewardDto[];
    type: TournamentRewardsTaskType;
    numberOfMatchs: number;
}

export interface QuizMilestoneChallengeEventDto extends EventDto {
    thresholds: QuizMilestoneChallengeThresholdDto[];
}

export interface QuizMilestoneChallengeProgressDto extends UserInEventProgressDto {
    info: QuizMilestoneChallengeProgressInfoDto;
}

export interface QuizMilestoneChallengeProgressInfoDto {
    completedQuestions: number;
    rewardClaimedThresholdIds: number[];
}

export interface QuizMilestoneChallengeThresholdDto {
    thresholdId: number;
    expScoreGained: number;
    rewards: EventRewardDto[];
    challengeQuestionIds: number[];
}

export interface LuckySpinEventDto extends EventDto {
    maxSpinTimePerDay: number;
    spinItems: LuckySpinItemDto[];
}

export interface LuckySpinItemDto {
    itemId: number;
    reward?: EventRewardDto;
    rate: number;
}

export interface LuckySpinProgress extends UserInEventProgressDto {
    todaySpinTime: number;
}

export interface EventRewardDto {
    eventRewardId: number;
    value: number;
}

export interface EventRewardInfoDto {
    id: number;
    name: string;
    type: EventRewardType;
    desc: string;
    unit: string;
}

export interface UserInEventProgressDto {
    eventId: number;
    lastChanged: Date;
}

export interface UpdateEventRequest {
    name: string;
    desc: string;
    startTime: Date;
    endTime?: Date;
    eventConfigJsonData: string;
    isLocked: boolean;
}

export enum TournamentRewardsTaskType {
    PlayMatch = "PlayMatch",
    WinMatch = "WinMatch",
    LoseMatch = "LoseMatch"
}

export const TournamentRewardsTaskTypeLabel: Record<TournamentRewardsTaskType, string> = {
    [TournamentRewardsTaskType.PlayMatch]: 'Play Match',
    [TournamentRewardsTaskType.WinMatch]: 'Win Match',
    [TournamentRewardsTaskType.LoseMatch]: 'Lose Match',
};

export enum EventType {
    QuizMilestoneChallenge = "QuizMilestoneChallenge",
    LuckySpin = 'LuckySpin',
    TournamentRewards = 'TournamentRewards'
}

export const EventTypeLabel: Record<EventType, string> = {
    [EventType.QuizMilestoneChallenge]: 'Quiz Milestone Challenge',
    [EventType.LuckySpin]: 'Lucky Spin',
    [EventType.TournamentRewards]: 'Tournament Rewards',
};

export enum EventTimeType {
    Limited = 'Limited',
    Daily = 'Daily',
    Seasonal = 'Seasonal'
}

export const EventTimeTypeLabel: Record<EventTimeType, string> = {
    [EventTimeType.Limited]: 'Limited',
    [EventTimeType.Daily]: 'Daily',
    [EventTimeType.Seasonal]: 'Seasonal',
};

export enum EventRewardType {
    RankProtectionCard,
    ExpScore,
    Gold,
    MatchLoudspeaker
}

export const EventRewardTypeLabel: Record<EventRewardType, string> = {
    [EventRewardType.RankProtectionCard]: 'Rank Protection Card',
    [EventRewardType.ExpScore]: 'Exp Score',
    [EventRewardType.Gold]: 'Gold',
    [EventRewardType.MatchLoudspeaker]: 'Match Loudspeaker',
};

export enum EventStatus {
    Pending,
    InProgress,
    Ended
}

export const EventStatusLabel: Record<EventStatus, string> = {
    [EventStatus.Pending]: 'Pending',
    [EventStatus.InProgress]: 'In Progress',
    [EventStatus.Ended]: 'Ended',
};