export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginSesssionDto {
    accessToken: string;
    id: number;
    name: string;
    avatarUrl: string;
    level: number;
    rank: number;
    rankScore: number;
}
