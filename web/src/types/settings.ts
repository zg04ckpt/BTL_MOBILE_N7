export interface SettingsDto {
    maintenanceMode: boolean;
    whitelistIPs: string;
    loginLiveTime: number;
    questionTimeLimit: number;
    questionsPerMatch: number;
    baseWinPoints: number;
    baseLosePoints: number;
    eloKFactor: number;
    lastUpdated: Date;
}

export interface UpdateSettingsRequest {
    maintenanceMode: boolean;
    whitelistIPs: string;
    loginLiveTime: number;
    questionTimeLimit: number;
    questionsPerMatch: number;
    baseWinPoints: number;
    baseLosePoints: number;
    eloKFactor: number;
}
