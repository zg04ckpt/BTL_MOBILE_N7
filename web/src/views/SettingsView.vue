<template>
  <div class="px-4">
    <a-typography-title :level="2">Quản lý cấu hình</a-typography-title>
    <div class="d-flex align-items-center justify-content-between gap-5 mt-5">
      <div class="w-50 border rounded-3 shadow-sm">
        <div
          class="d-flex align-items-center text-primary fs-4 p-3 rounded-3 gap-3 fw-bold border shadow-sm"
        >
          <global-outlined class="ms-4" /> Cấu hình chung
        </div>
        <div class="px-4 pb-5">
          <div
            class="maintenance-box d-flex align-items-center justify-content-between p-4 rounded-3 mt-4"
          >
            <div class="d-flex flex-column">
              <div class="fs-4 fw-bold text-brown">Chế độ bảo trì (Maintenance)</div>
              <div class="fs-6 text-orange">Người dùng sẽ không thể truy cập hệ thống khi bật.</div>
            </div>
            <a-switch v-model:checked="config.isMaintenance" />
          </div>
          <div class="mt-3 fs-5 fw-bold ms-4">Whitelist IP (Truy cập ưu tiên)</div>
          <a-input
            v-model:value="config.whitelistIp"
            class="border rounded-3 shadow-sm p-4 fs-5 mt-2 pb-5 text-semi-bold"
          />
          <div class="mt-3 fs-5 fw-bold ms-4">Phiên bản ứng dụng bắt buộc</div>
          <a-input
            v-model:value="config.appVersionRequired"
            class="border rounded-3 shadow-sm px-4 py-2 fs-5 mt-2 text-semi-bold"
          />
        </div>
      </div>
      <div class="w-50 border rounded-3 shadow-sm">
        <div
          class="d-flex align-items-center text-primary fs-4 p-3 rounded-3 gap-3 fw-bold border shadow-sm"
        >
          <customer-service-outlined class="ms-4" /> Cấu hình game
        </div>
        <div class="px-5 pb-5 pt-4">
          <div class="d-flex gap-5 justify-content-between">
            <div class="w-50">
              <div>Thời gian trả lời (s)</div>
              <a-input
                class="border rounded-3 shadow-sm px-4 py-3 mt-2 w-100 fs-5"
                v-model:value="config.answerTime"
              />
            </div>
            <div class="w-50">
              <div>Số câu hỏi mỗi trận</div>
              <a-input
                class="border rounded-3 shadow-sm px-4 py-3 mt-2 w-100 fs-5"
                v-model:value="config.questionsPerMatch"
              />
            </div>
          </div>
          <div class="d-flex gap-5 mt-5 justify-content-between">
            <div class="w-50">
              <div>Điểm thắng cơ bản</div>
              <a-input
                class="border rounded-3 shadow-sm px-4 py-3 mt-2 w-100 fs-5"
                v-model:value="config.baseWinScore"
              />
            </div>
            <div class="w-50">
              <div>Điểm thua cơ bản</div>
              <a-input
                class="border rounded-3 shadow-sm px-4 py-3 mt-2 w-100 fs-5"
                v-model:value="config.baseLoseScore"
              />
            </div>
          </div>
          <div class="d-flex gap-5 mt-5 justify-content-between">
            <div class="w-100">
              <div>Hệ số ELO K-factor</div>
              <a-input
                class="border rounded-3 fs-5 shadow-sm px-4 py-3 mt-2 w-100 text-semi-bold"
                v-model:value="config.eloKFactor"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="d-flex justify-content-center mt-5">
      <a-button type="primary" size="large" class="px-5 h-75px w-190px fs-4">Lưu</a-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { GlobalOutlined, CustomerServiceOutlined } from '@ant-design/icons-vue'

const config = reactive({
  isMaintenance: false,
  whitelistIp: '127.0.0.1, 192.168.1.1',
  appVersionRequired: '2.4.5',
  answerTime: 15,
  questionsPerMatch: 20,
  baseWinScore: 20,
  baseLoseScore: 20,
  eloKFactor: 32
})
</script>

<style scoped>
.maintenance-box {
  background-color: #fff7ed;
}

.text-brown {
  color: #802d12;
}

.text-orange {
  color: #ce5128;
}

.text-semi-bold {
  font-weight: 500;
}

.h-75px {
  height: 75px;
}

.w-190px {
  width: 190px;
}
</style>
