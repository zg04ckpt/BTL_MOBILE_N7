<template>
    <h2 class="mb-4">Quản lý sự kiện</h2>

    <div class="row g-3">
        <div v-for="event in events" class="col-6">
            <div class="card card-body d-flex flex-row" style="height: 190px;">
                <img v-if="event.type == EventType.LuckySpin" class="me-2" src="@/assets/images/lucky_spin.png" alt="">
                <img v-else-if="event.type == EventType.QuizMilestoneChallenge" class="me-2" src="@/assets/images/quiz_milestone_challenge.png" alt="">
                <div class="d-flex flex-column">
                    <h4>{{ event.name }}</h4>
                    <div class="d-flex mb-2">
                        <!-- <a-tag color="blue">{{ formatStartTime(event.startTime) }}</a-tag> -->
                        <a-tag color="purple">{{ event.timeType }}</a-tag>
                        <a-tag color="red">{{ getRemainingTime(event.endTime!) }}</a-tag>
                        <a-tag v-if="event.type == EventType.QuizMilestoneChallenge" color="#2db7f5">{{ event.thresholds.length }} Mức</a-tag>
                        <a-tag v-if="event.type == EventType.LuckySpin" color="#87d068">{{ event.spinItems.length }} Vật phẩm</a-tag>
                    </div>
                    <small class="fs-6 mb-2 desc flex-fill">
                        <div class="desc">{{ event.desc }}</div>
                    </small>
                    <div class="d-flex ">
                        <a-button size="small" @click="() => {
                            showUpdateModal = true;
                            selectedEvent = event;
                            console.log(selectedEvent);
                        }">Chỉnh sửa</a-button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Update dialog -->
    <a-modal v-if="selectedEvent != null" v-model:open="showUpdateModal" title="Cập nhật sự kiện">
        <template #footer>
            <a-button key="back" @click="handleCancel">Hủy</a-button>
            <a-button key="submit" type="primary" @click="handleUpdate">Cập nhật</a-button>
        </template>
        <a-form
            class="mt-2"
            :value-col="{ span: 4 }"
            :model="selectedEvent">

            <!-- Tên sự kiện -->
            <div class="mb-1">Tên sự kiện</div>
            <a-input placeholder="Nhập tên sự kiện" class="mb-2" v-model:value="selectedEvent.name"/>

            <!-- Mô tả -->
            <div class="mb-1">Mô tả sự kiện</div>
            <a-textarea placeholder="Nhập mô tả sự kiện" class="mb-2" v-model:value="selectedEvent.desc"/>

            <!-- Start date -->
            <div class="mb-1">Ngày bắt đầu</div>
            <a-date-picker
                show-time
                type="date"
                v-model:value="selectedEvent.startTime"
                placeholder="Chọn ngày bắt đầu"
                style="width: 100%"
                class="mb-2"
            />

            <!-- End date -->
            <div class="mb-1">Ngày kết thúc</div>
            <a-date-picker
                show-time
                v-model:value="selectedEvent.endTime"
                type="date"
                placeholder="Chọn ngày kết thúc"
                style="width: 100%"
                class="mb-2"
            />

            <!-- Kiểu sự kiện -->
            <div class="mb-1">Trạng thái</div>
            <a-checkbox v-model:checked="selectedEvent.isLocked">Khóa sự kiện</a-checkbox>

            <a-divider style="border-color: black" dashed class="my-3"/>

            <!-- Custome for Lucky Spin -->
            <div v-if="selectedEvent.type == EventType.LuckySpin">
                <!-- Số lượt quay tối đa trong ngày -->
                <div class="mb-1">Số lượt quay tối đa trong ngày</div>
                <a-input type="number" :min="1" v-model:value.number="selectedEvent.maxSpinTimePerDay" placeholder="Nhập số lượng" class="mb-2"/>
                
                <!-- Items -->
                <div class="mb-2">Danh sách vật phẩm</div>
                <div v-for="(item, index) in selectedEvent.spinItems" class="d-flex mb-2">
                    <a-select class="me-2 flex-fill" v-model:value="item.reward.eventRewardId" @change="(value:any) => item.reward.eventRewardId = value">
                        <a-select-option v-for="item in rewards" :value="item.id">{{ item.name }}</a-select-option>
                    </a-select>
                    <a-input placeholder="Nhập giá trị" type="number" v-model:value.number="item.reward.value" class="me-2 w-25"/>
                    <a-input suffix="%" placeholder="Nhập tỉ lệ" type="number" v-model:value.number="item.rate" class="me-2 w-25"/>
                    <a-button :icon="h(MinusOutlined)" style="aspect-ratio: 1/1;" @click="() => selectedEvent.spinItems.splice(index, 1)"></a-button>
                </div>
                <div class="d-flex">
                    <a-button type="primary" class="me-2" :icon="h(PlusOutlined)" @click="() => {
                        selectedEvent.spinItems.push({
                            itemId: -1,
                            reward: {
                                eventRewardId: 1,
                                value: 0
                            },
                            rate: 0
                        });
                    }">Thêm vật phẩm</a-button>

                    <a-button class="me-2" @click="() => {
                        if (selectedEvent.spinItems.length === 0) return;
                        if (selectedEvent.spinItems.length === 1) {
                            selectedEvent.spinItems[0].rate = 100;
                            return;
                        }
                        const cutPoints = Array.from({ length: selectedEvent.spinItems.length - 1 }, () => Math.floor(Math.random() * 101));
                        cutPoints.sort((a: number, b: number) => a - b);
                        const points = [0, ...cutPoints, 100];
                        selectedEvent.spinItems.forEach((item: any, index: number) => {
                            item.rate = points[index + 1] - points[index];
                        });
                    }">Random tỉ lệ</a-button>
                </div>
            </div>

            <!-- Custome for QuizMilestoneChallenge -->
            <div v-else-if="selectedEvent.type == EventType.QuizMilestoneChallenge">

                <!-- List ngưỡng -->
                <div v-for="(threshold, index) in selectedEvent.thresholds" class="mb-3">
                    <div class="d-flex mb-2">
                        <div class="mb-1 fw-bold flex-fill">Ngưỡng {{ index as number + 1 }}</div>
                        <a-button :icon="h(MinusOutlined)" style="aspect-ratio: 1/1;" @click="() => selectedEvent.thresholds.splice(index, 1)"></a-button>
                    </div>
                    <!-- Exp -->
                    <div class="d-flex align-items-center mb-2">
                        <div class="me-2 flex-fill flex-shrink-0">- Điểm cộng kinh nghiệm</div>
                        <a-input type="number" v-model:value.number="threshold.expScoreGained" :min="1" placeholder="Nhập số điểm cộng"/>
                    </div>

                    <!-- Threshold rewards -->
                    <div class="d-flex flex-column">
                        <div v-for="(r, rindex) in threshold.rewards" class="d-flex mb-2 align-items-center">
                            <div class="me-2">- Phần thưởng {{ rindex as number + 1 }}</div>
                            <a-select class="me-2 flex-fill" v-model:value="r.eventRewardId" @change="(value:any) => r.eventRewardId = value">
                                <a-select-option v-for="item in rewards" :value="item.id">{{ item.name }}</a-select-option>
                            </a-select>
                            <a-input placeholder="Nhập giá trị" type="number" v-model:value.number="r.value" class="me-2 w-25"/>
                            <a-button :icon="h(MinusOutlined)" style="aspect-ratio: 1/1;" @click="() => threshold.rewards.splice(rindex, 1)"></a-button>
                        </div>
                        <a-button  type="link" size="small" :icon="h(PlusOutlined)" @click="() => {
                            threshold.rewards.push({
                                eventRewardId: 1,
                                value: 0
                            });
                        }">Thêm phần thưởng</a-button>
                    </div>

                    <!-- Questions -->
                    <div class="d-flex flex-column">
                        <div v-for="(qid, index) in threshold.challengeQuestionIds" class="d-flex mb-2 align-items-center">
                            <div class="me-2">- Câu hỏi {{ index as number + 1 }}</div>
                            <a-select class="me-2 flex-fill" v-model:value="threshold.challengeQuestionIds[index]" @change="(value:any) => threshold.challengeQuestionIds[index] = value">
                                <a-select-option v-for="item in questions" :value="item.id">[{{ item.topicName }}] {{ item.stringContent }}</a-select-option>
                            </a-select>
                            <a-button :icon="h(MinusOutlined)" style="aspect-ratio: 1/1;" @click="() => threshold.challengeQuestionIds.splice(index, 1)"></a-button>
                        </div>
                        <a-button  type="link" size="small" :icon="h(PlusOutlined)" @click="() => {
                            threshold.challengeQuestionIds.push(questions[0].id);
                        }">Thêm câu hỏi</a-button>
                    </div>
                </div>

                <!-- Add -->
                <a-button type="primary" class="me-2 w-100 mt-2" :icon="h(PlusOutlined)" @click="() => {
                    selectedEvent.thresholds.push({
                        thresholdId: -1,
                        expScoreGained: 100,
                        rewards: [
                            {
                                eventRewardId: 1,
                                value: 1
                            }
                        ],
                        challengeQuestionIds: [
                            1,
                        ]
                    });
                }">Thêm ngưỡng</a-button>
            </div>

        </a-form>
    </a-modal>
</template>

<script setup lang="ts">
import { getAllQuestions } from '@/api';
import { getAllEvents, getAllRewards, updateEvent } from '@/api/event';
import { QuestionListItemDto } from '@/types';
import { EventRewardInfoDto, EventType, UpdateEventRequest } from '@/types/event';
import { MinusOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';
import { h, onMounted, ref } from 'vue';
import dayjs from 'dayjs';

const events = ref<any[]>([]);
const questions = ref<QuestionListItemDto[]>([]);
const rewards = ref<EventRewardInfoDto[]>([]);
const showUpdateModal = ref(true);
const selectedEvent = ref<any>(null);

type ValidationResult = {
    isValid: boolean;
    message?: string;
};

const validateEventForUpdate = (event: any): ValidationResult => {
    if (!event) return { isValid: false, message: "Dữ liệu sự kiện không hợp lệ" };

    if (!event.name || !event.name.trim()) {
        return { isValid: false, message: "Tên sự kiện không được để trống" };
    }

    if (!event.desc || !event.desc.trim()) {
        return { isValid: false, message: "Mô tả sự kiện không được để trống" };
    }

    const start = dayjs(event.startTime);
    if (!start.isValid()) {
        return { isValid: false, message: "Ngày bắt đầu không hợp lệ" };
    }

    if (event.endTime) {
        const end = dayjs(event.endTime);
        if (!end.isValid()) {
            return { isValid: false, message: "Ngày kết thúc không hợp lệ" };
        }
        if (end.isBefore(start)) {
            return { isValid: false, message: "Ngày kết thúc phải sau ngày bắt đầu" };
        }
    }

    if (event.type === EventType.LuckySpin) {
        if (!event.maxSpinTimePerDay || event.maxSpinTimePerDay < 1) {
            return { isValid: false, message: "Số lượt quay tối đa trong ngày phải >= 1" };
        }

        if (!Array.isArray(event.spinItems) || event.spinItems.length === 0) {
            return { isValid: false, message: "Lucky Spin cần ít nhất 1 vật phẩm" };
        }

        let sumRate = 0;
        for (const item of event.spinItems) {
            if (!item.reward || !item.reward.eventRewardId || item.reward.eventRewardId < 1) {
                return { isValid: false, message: "Vật phẩm Lucky Spin thiếu phần thưởng" };
            }
            if (item.reward.value === null || item.reward.value === undefined || item.reward.value < 0) {
                return { isValid: false, message: "Giá trị phần thưởng Lucky Spin không hợp lệ" };
            }
            if (item.rate === null || item.rate === undefined || item.rate <= 0) {
                return { isValid: false, message: "Tỉ lệ Lucky Spin phải > 0" };
            }
            sumRate += Number(item.rate);
        }

        if (Math.abs(sumRate - 100) > 0.01) {
            return { isValid: false, message: "Tổng tỉ lệ Lucky Spin phải bằng 100%" };
        }
    }

    if (event.type === EventType.QuizMilestoneChallenge) {
        if (!Array.isArray(event.thresholds) || event.thresholds.length === 0) {
            return { isValid: false, message: "Quiz Milestone cần ít nhất 1 ngưỡng" };
        }

        for (const threshold of event.thresholds) {
            if (!threshold.expScoreGained || threshold.expScoreGained < 1) {
                return { isValid: false, message: "Điểm kinh nghiệm cộng phải >= 1" };
            }
            if (!Array.isArray(threshold.rewards) || threshold.rewards.length === 0) {
                return { isValid: false, message: "Ngưỡng cần ít nhất 1 phần thưởng" };
            }
            for (const reward of threshold.rewards) {
                if (!reward.eventRewardId || reward.eventRewardId < 1) {
                    return { isValid: false, message: "Phần thưởng ngưỡng không hợp lệ" };
                }
                if (reward.value === null || reward.value === undefined || reward.value < 0) {
                    return { isValid: false, message: "Giá trị phần thưởng ngưỡng không hợp lệ" };
                }
            }
            if (!Array.isArray(threshold.challengeQuestionIds) || threshold.challengeQuestionIds.length === 0) {
                return { isValid: false, message: "Ngưỡng cần ít nhất 1 câu hỏi" };
            }
        }
    }

    return { isValid: true };
};

const normalizeEventNumbers = (event: any) => {
    if (!event) return;

    if (event.type === EventType.LuckySpin) {
        event.maxSpinTimePerDay = Number(event.maxSpinTimePerDay);
        if (Array.isArray(event.spinItems)) {
            event.spinItems.forEach((item: any) => {
                if (item.reward) {
                    item.reward.eventRewardId = Number(item.reward.eventRewardId);
                    item.reward.value = Number(item.reward.value);
                }
                item.rate = Number(item.rate);
            });
        }
    } else if (event.type === EventType.QuizMilestoneChallenge) {
        if (Array.isArray(event.thresholds)) {
            event.thresholds.forEach((threshold: any) => {
                threshold.expScoreGained = Number(threshold.expScoreGained);
                if (Array.isArray(threshold.rewards)) {
                    threshold.rewards.forEach((reward: any) => {
                        reward.eventRewardId = Number(reward.eventRewardId);
                        reward.value = Number(reward.value);
                    });
                }
                if (Array.isArray(threshold.challengeQuestionIds)) {
                    threshold.challengeQuestionIds = threshold.challengeQuestionIds.map((id: any) => Number(id));
                }
            });
        }
    }
};

const handleUpdate = async () => {
    if (!selectedEvent.value) return;

    const validation = validateEventForUpdate(selectedEvent.value);
    if (!validation.isValid) {
        message.error(validation.message || "Dữ liệu cập nhật không hợp lệ");
        return;
    }

    normalizeEventNumbers(selectedEvent.value);
    
    const hideLoading = message.loading("Đang xử lý ...");
    const request: UpdateEventRequest = {
        name: selectedEvent.value.name,
        desc: selectedEvent.value.desc,
        startTime: selectedEvent.value.startTime,
        endTime: selectedEvent.value.endTime,
        isLocked: selectedEvent.value.isLocked,
        eventConfigJsonData: ""
    }

    // Convert event config to json
    if (selectedEvent.value.type === EventType.QuizMilestoneChallenge) {

        request.eventConfigJsonData = JSON.stringify(selectedEvent.value.thresholds);

    } else if (selectedEvent.value.type === EventType.LuckySpin) {

        request.eventConfigJsonData = JSON.stringify({
            maxSpinTimePerDay: selectedEvent.value.maxSpinTimePerDay,
            spinItems: selectedEvent.value.spinItems
        });

    } 

    const res = await updateEvent(selectedEvent.value.id, request);
    hideLoading();
    if (res.isSuccess) {
        message.success("Cập nhật thành công");
        selectedEvent.value = null;
        showUpdateModal.value = false;
        handleLoadEvent();
    } else {
        message.error("Cập nhật thất bại: " + res.message);
    }
}

function handleCancel() {
    showUpdateModal.value = false;
    selectedEvent.value = null;
}

function getRemainingTime(targetDate: Date | string | number): string {
    const now = new Date().getTime();
    const target = new Date(targetDate).getTime();

    let diff = Math.floor((target - now) / 1000);
    if (diff <= 0) return "Đã kết thúc";
    const day = 86400;
    const hour = 3600;
    const minute = 60;

    if (diff >= day) {
        return `Kết thúc sau ${Math.floor(diff / day)}ng`;
    }

    if (diff >= hour) {
        return `Kết thúc sau ${Math.floor(diff / hour)}h`;
    }

    if (diff >= minute) {
        return `Kết thúc sau ${Math.floor(diff / minute)}m`;
    }

    return `Kết thúc sau ${diff}s`;
}

const handleLoadEvent = async () => {
    const hideLoading = message.loading("Đang tải event...", 0);
    const res = await getAllEvents();
    hideLoading();

    if (res.isSuccess) {
        // Convert from ISO string to Dayjs
        res.data!.forEach(e => {
            if (e.startTime) e.startTime = dayjs(e.startTime);
            if (e.endTime) e.endTime = dayjs(e.endTime);
        });

        events.value = (res.data ?? []).filter(e => e.type !== EventType.TournamentRewards);

        // selectedEvent.value = res.data![1];
    } else {
        message.error("Tải dữ liệu event thất bại: " + res.message);
    }
}

const handleLoadReward = async () => {
    const res = await getAllRewards();

    if (res.isSuccess) {
        rewards.value = res.data!;
    } else {
        message.error("Tải dữ liệu reward thất bại: " + res.message);
    }
}

const handleLoadQuestions = async () => {
    const res = await getAllQuestions({
        pageIndex: 1,
        pageSize: 999999,
        isAsc: true,
        level: null,
        status: null,
        stringContent: '',
        topicId: null,
        type: null,
    });

    if (res.isSuccess) {
        questions.value = res.data!.items;
    } else {
        message.error("Tải dữ liệu question thất bại: " + res.message);
    }
}

onMounted(() => {
    handleLoadEvent();
    handleLoadReward();
    handleLoadQuestions();
});

</script>

<style scoped>
.desc {
    text-overflow: ellipsis;
    overflow: hidden;
    font-size: 14px !important;
}
 
img {
    height: 100%;
    aspect-ratio: 1 / 1;
    border-radius: 4px;
    object-fit: cover;
}
:deep(.anticon) {
    display: inline-flex;
    align-items: center;
}
</style>