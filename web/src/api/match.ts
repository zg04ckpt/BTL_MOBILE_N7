import { MatchListItemDto, Paginated, SearchMatchRequest } from "@/types";
import { del, get } from "./config";
import { endpoints } from "./endpoints";

export const getAllMatches = async (request: SearchMatchRequest) => {
  return await get<Paginated<MatchListItemDto>>(endpoints.battles.paging(request));
};

export const deleteMatch = async (id: number) => {
  return await del(endpoints.battles.delete(id));
};
