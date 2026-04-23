import type { PagingRequest } from './api';

export interface AnalyticsFilterRequest {
  startDate?: Date | null;
  endDate?: Date | null;
  lastDays?: number | null;
}

export interface RecentUsersRequest extends PagingRequest {
  startDate?: Date | null;
  endDate?: Date | null;
  lastDays?: number | null;
}

export interface OverviewMetrics {
  newUsers: number;
  newUsersChangePercent: number;
  totalUsers: number;
  totalUsersChangePercent: number;
  totalRegistrations: number;
  totalRegistrationsChangePercent: number;
  peakCCU: number;
  peakCCUChangePercent: number;
}

export interface DailyUserTrendDto {
  date: string;
  newUsers: number;
  activeUsers: number;
}

export interface AccountStatusDistributionDto {
  active: number;
  banned: number;
  inactive: number;
  total: number;
}

export interface RecentUserDto {
  id: number;
  name: string;
  email: string;
  avatarUrl: string;
  registeredAt: string;
  status: string;
  totalMatches: number;
}

export interface SystemAnalyticsDto {
  overview: OverviewMetrics;
  userTrend: DailyUserTrendDto[];
  accountStatusDistribution: AccountStatusDistributionDto;
  recentUsers: RecentUserDto[];
  generatedAt: string;
}
