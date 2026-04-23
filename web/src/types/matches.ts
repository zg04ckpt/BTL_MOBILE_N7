import { PagingRequest } from ".";

export interface SearchMatchRequest extends PagingRequest {
  battleType?: string | null;
  numberOfPlayers?: number | null;
  from?: string | null;
  to?: string | null;
}

export interface MatchListItemDto {
  id: number;
  from: string;
  to: string;
  battleStatus: string;
  battleType: string;
  contentType: string;
  numberOfPlayers: number;
  topicName?: string | null;
}
