<template>
    <div class="settings-page">
        <div class="page-header">
            <div>
                <a-typography-title :level="2" class="mb-0">Quản lý cấu hình</a-typography-title>
            </div>
            <a-space>
                <!-- <a-button @click="() => reset()">Khôi phục cấu hình mặc định</a-button> -->
                <a-button type="primary" size="large" @click="handleUpdate">Lưu thay đổi</a-button>
            </a-space>
        </div>

        <a-row :gutter="24" class="mt-4" v-if="settings">
            <a-col :xs="24" :lg="12">
                <a-card title="Cấu hình chung" class="settings-card">
                    <a-form layout="vertical">
                        <a-form-item label="Chế độ bảo trì">
                            <div class="switch-row">
                                <div>
                                    <a-typography-text strong>Maintenance mode</a-typography-text>
                                    <div class="helper-text">Người dùng sẽ không thể truy cập hệ thống khi bật.</div>
                                </div>
                                <a-switch v-model:checked="settings.maintenanceMode" />
                            </div>
                        </a-form-item>
                        <a-form-item label="Whitelist IP">
                            <a-input v-model:value="settings.whitelistIPs" placeholder="127.0.0.1, 192.168.1.1" />
                        </a-form-item>
                        <a-form-item label="Thời gian sống phiên đăng nhập (phút)">
                            <a-input-number v-model:value="settings.loginLiveTime" :min="1" :step="5" class="w-100" />
                        </a-form-item>
                        <a-form-item label="Cập nhật lần cuối">
                            <a-input v-model:value="settings.lastUpdated" disabled />
                        </a-form-item>
                    </a-form>
                </a-card>
            </a-col>

            <a-col :xs="24" :lg="12">
                <a-card title="Cấu hình trận đấu" class="settings-card">
                    <a-form layout="vertical">
                        <a-form-item label="Thời gian trả lời (giây)">
                            <a-input-number v-model:value="settings.questionTimeLimit" :min="5" :step="1"
                                class="w-100" />
                        </a-form-item>
                        <a-form-item label="Số câu hỏi mỗi trận">
                            <a-input-number v-model:value="settings.questionsPerMatch" :min="1" :step="1"
                                class="w-100" />
                        </a-form-item>
                        <a-row :gutter="16">
                            <a-col :xs="24" :md="12">
                                <a-form-item label="Điểm thắng cơ bản">
                                    <a-input-number v-model:value="settings.baseWinPoints" :min="0" :step="1"
                                        class="w-100" />
                                </a-form-item>
                            </a-col>
                            <a-col :xs="24" :md="12">
                                <a-form-item label="Điểm thua cơ bản">
                                    <a-input-number v-model:value="settings.baseLosePoints" :min="0" :step="1"
                                        class="w-100" />
                                </a-form-item>
                            </a-col>
                        </a-row>
                        <a-form-item label="Hệ số ELO K-factor">
                            <a-input-number v-model:value="settings.eloKFactor" :min="1" :step="1" class="w-100" />
                        </a-form-item>
                    </a-form>
                </a-card>
            </a-col>
        </a-row>
    </div>
</template>

<script setup lang="ts">
import { updateEvent } from '@/api/event';
import { getAllSettings, updateSetting } from '@/api/settings';
import router from '@/router';
import { SettingsDto } from '@/types';
import { message } from 'ant-design-vue';
import { onMounted, ref } from 'vue';

const settings = ref<SettingsDto | null>(null);

const handleGetAllSettings = async () => {
    const hideLoading = message.loading("Đang tải dữ liệu ...");
    const res = await getAllSettings();
    hideLoading();

    if (res.isSuccess) {
        settings.value = res.data!;
        settings.value.lastUpdated = new Date(settings.value.lastUpdated);
    } else {
        message.error("Tải dữ liệu cấu hình thất bại: " + res.message);
    }
}

const handleUpdate = async () => {
    if (!settings.value) return;

    const hideLoading = message.loading("Đang cập nhật ...");
    const res = await updateSetting({ 
        baseLosePoints: settings.value!.baseLosePoints,
        baseWinPoints: settings.value!.baseWinPoints,
        eloKFactor: settings.value!.eloKFactor,
        loginLiveTime: settings.value!.loginLiveTime,
        maintenanceMode: settings.value!.maintenanceMode,
        questionsPerMatch: settings.value!.questionsPerMatch,
        questionTimeLimit: settings.value!.questionTimeLimit,
        whitelistIPs: settings.value!.whitelistIPs,
    });
    hideLoading();

    if (res.isSuccess) {
        message.success("Cập nhật cấu hình thành công");
    } else {
        message.error("Tải dữ liệu cấu hình thất bại: " + res.message);
    }
}

const reset = () => {
    location.reload();
}
 
onMounted(() => {
    handleGetAllSettings();
});

</script>

<style scoped>
.settings-page {
    padding: 16px 24px 32px;
}

.page-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
    flex-wrap: wrap;
}

.settings-card {
    border-radius: 12px;
}

.switch-row {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 16px;
    padding: 12px 16px;
    border: 1px solid #f0f0f0;
    border-radius: 12px;
    background: #fafafa;
}

.helper-text {
    color: #8c8c8c;
    font-size: 12px;
    margin-top: 4px;
}

.w-100 {
    width: 100%;
}

.mt-4 {
    margin-top: 16px;
}

.mb-0 {
    margin-bottom: 0;
}
</style>
