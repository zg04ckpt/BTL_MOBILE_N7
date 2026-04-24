import { get } from './config';
import { endpoints } from './endpoints';
import type { AnalyticsFilterRequest, RecentUsersRequest, SystemAnalyticsDto, RecentUserDto, Paginated } from '@/types';

export const getSystemAnalytics = async (request: AnalyticsFilterRequest) => {
  return await get<SystemAnalyticsDto>(endpoints.analytics.overview(request));
};

export const getRecentUsersAnalytics = async (request: RecentUsersRequest) => {
  return await get<Paginated<RecentUserDto>>(endpoints.analytics.recentUsers(request));
};
