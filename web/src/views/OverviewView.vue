<template>
  <div class="px-4">
    <section class="d-flex align-items-center justify-content-between">
      <a-typography-title :level="2" class="m-0 fs-1">Báo cáo & thống kê</a-typography-title>
      <a-space size="middle">
        <a-select v-model:value="filterDate" class="w-150px">
          <a-select-option value="7">7 ngày qua</a-select-option>
          <a-select-option value="30">30 ngày qua</a-select-option>
        </a-select>
        <a-range-picker v-model:value="dateRange" />
        <a-button @click="handleExport">
          <template #icon><DownloadOutlined /></template> Export
        </a-button>
        <a-button type="primary">Lên lịch</a-button>
      </a-space>
    </section>

    <a-tabs class="mb-3">
      <a-tab-pane key="user" tab="Người dùng" />
      <a-tab-pane key="question" tab="Câu hỏi" />
      <a-tab-pane key="match" tab="Trận đấu" />
    </a-tabs>

    <a-row :gutter="[20, 20]">
      <a-col :xs="24" :sm="12" :lg="6" v-for="stat in summaryData" :key="stat.title">
        <a-card hoverable class="rounded shadow-sm">
          <a-typography-text type="primary" class="fs-5 fw-bold">{{
            stat.title
          }}</a-typography-text>
          <div class="d-flex justify-content-between align-items-center">
            <span class="fs-2 fw-bold">{{ stat.value }}</span>
            <span class="fs-4" :class="[stat.isUp ? 'up' : 'down']"
              >{{ stat.isUp ? '+ ' : '- ' }}{{ stat.trend }}%</span
            >
          </div>
        </a-card>
      </a-col>
    </a-row>

    <a-row :gutter="[20, 20]" class="my-5">
      <a-col :lg="16" , :xs="24">
        <a-card title="Xu hướng người dùng mới" :bordered="true" class="h-100 rounded shadow-sm">
          <apexchart
            type="area"
            height="300"
            :options="chartConfigs.lineOptions"
            :series="lineSeries"
          />
        </a-card>
      </a-col>
      <a-col :lg="8" :xs="24">
        <a-card title="Tỷ lệ tài khoản" :bordered="true" class="h-100 rounded shadow-sm">
          <apexchart
            type="donut"
            height="300"
            :options="chartConfigs.donutOptions"
            :series="donutSeries"
          />
        </a-card>
      </a-col>

      <a-card
        title="Chi tiết người dùng gần đây"
        :bordered="true"
        class="w-100 mt-4 rounded shadow-sm"
      >
        <a-table
          :columns="columns"
          :data-source="userData"
          :pagination="paginationConfig"
          @change="handleChangePageSize"
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'status'">
              <a-tag :color="getStatusColor(record.status)">{{ record.status }}</a-tag>
            </template>
          </template>
        </a-table>
      </a-card>
    </a-row>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { DownloadOutlined } from '@ant-design/icons-vue'
import { summaryData } from '@/mocks/statistic'
import { userData } from '@/mocks/user'
import { getStatusColor } from '@/utils/helper'

const filterDate = ref<string>('7')
const dateRange = ref<any>([])
const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50']
})

const handleExport = (): void => {
  console.log('Đang xuất dữ liệu cho khoảng ngày:', dateRange.value)
}

const handleChangePageSize = (pagination: any): void => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
}

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id' },
  { title: 'Tên hiển thị', dataIndex: 'name', key: 'name' },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  { title: 'Ngày đăng ký', dataIndex: 'createdAt', key: 'createdAt' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Tổng số trận', dataIndex: 'matches', key: 'matches' }
]

const lineSeries = [{ name: 'Người dùng mới', data: [31, 40, 28, 51, 42, 109, 100] }]
const donutSeries = [44, 55, 13]

const chartConfigs = reactive({
  lineOptions: {
    chart: { toolbar: { show: false }, fontFamily: 'Inter, sans-serif' },
    stroke: { curve: 'smooth', width: 3 },
    colors: ['#1890ff'],
    xaxis: { categories: ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'] },
    fill: { type: 'gradient', gradient: { shadeIntensity: 1, opacityFrom: 0.3, opacityTo: 0.05 } }
  },
  donutOptions: {
    labels: ['Hoạt động', 'Khóa', 'Ngừng'],
    colors: ['#52c41a', '#1890ff', '#faad14'],
    legend: { position: 'bottom' },
    plotOptions: { pie: { donut: { size: '70%' } } }
  }
})
</script>

<style scoped>
.w-150px {
  width: 150px !important;
}

.up {
  color: #52c41a;
}
.down {
  color: #ff4d4f;
}

:deep(.ant-table-tbody > tr:nth-child(even)),
:deep(.ant-table-thead > tr > th) {
  background-color: #e8e8e8;
}
</style>
