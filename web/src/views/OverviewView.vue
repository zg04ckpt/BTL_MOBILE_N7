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
import { computed, ref, reactive, onMounted } from 'vue'
import { DownloadOutlined } from '@ant-design/icons-vue'
import dayjs from 'dayjs'
import { getRecentUsersAnalytics, getSystemAnalytics } from '@/api'
import type { RecentUserDto } from '@/types'
import { message } from 'ant-design-vue'

const filterDate = ref<string>('7')
const dateRange = ref<any>([])
const analytics = ref<any>(null)
const userData = ref<RecentUserDto[]>([])

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  total: 0,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50']
})

const buildFilter = () => {
  const range = dateRange.value as any[]
  if (Array.isArray(range) && range.length === 2 && range[0] && range[1]) {
    return {
      startDate: dayjs(range[0]).toDate(),
      endDate: dayjs(range[1]).toDate(),
      lastDays: null
    }
  }
  return {
    startDate: null,
    endDate: null,
    lastDays: Number(filterDate.value)
  }
}

const summaryData = computed(() => {
  const ov = analytics.value?.overview
  if (!ov) return []
  return [
    { title: 'Người dùng mới', value: ov.newUsers, trend: Math.abs(Math.round(ov.newUsersChangePercent)), isUp: ov.newUsersChangePercent >= 0 },
    { title: 'Tổng người dùng', value: ov.totalUsers, trend: Math.abs(Math.round(ov.totalUsersChangePercent)), isUp: ov.totalUsersChangePercent >= 0 },
    { title: 'Tổng đăng ký', value: ov.totalRegistrations, trend: Math.abs(Math.round(ov.totalRegistrationsChangePercent)), isUp: ov.totalRegistrationsChangePercent >= 0 },
    { title: 'Peak CCU', value: ov.peakCCU, trend: Math.abs(Math.round(ov.peakCCUChangePercent)), isUp: ov.peakCCUChangePercent >= 0 }
  ]
})

const lineSeries = computed(() => [
  {
    name: 'Người dùng mới',
    data: (analytics.value?.userTrend ?? []).map((x: any) => x.newUsers)
  }
])

const donutSeries = computed(() => {
  const d = analytics.value?.accountStatusDistribution
  if (!d) return [0, 0, 0]
  return [d.active, d.banned, d.inactive]
})

const handleExport = (): void => {
  const filter = buildFilter()
  const query = new URLSearchParams()
  if (filter.startDate) query.append('startDate', filter.startDate.toISOString())
  if (filter.endDate) query.append('endDate', filter.endDate.toISOString())
  if (filter.lastDays) query.append('lastDays', String(filter.lastDays))
  window.open(`/api/analytics/export?${query.toString()}`, '_blank')
}

const handleChangePageSize = async (pagination: any): Promise<void> => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
  await loadRecentUsers()
}

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id' },
  { title: 'Tên hiển thị', dataIndex: 'name', key: 'name' },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  { title: 'Ngày đăng ký', dataIndex: 'createdAt', key: 'createdAt' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Tổng số trận', dataIndex: 'matches', key: 'matches' }
]

const chartConfigs = reactive({
  lineOptions: {
    chart: { toolbar: { show: false }, fontFamily: 'Inter, sans-serif' },
    stroke: { curve: 'smooth', width: 3 },
    colors: ['#1890ff'],
    xaxis: { categories: [] as string[] },
    fill: { type: 'gradient', gradient: { shadeIntensity: 1, opacityFrom: 0.3, opacityTo: 0.05 } }
  },
  donutOptions: {
    labels: ['Hoạt động', 'Khóa', 'Ngừng'],
    colors: ['#52c41a', '#1890ff', '#faad14'],
    legend: { position: 'bottom' },
    plotOptions: { pie: { donut: { size: '70%' } } }
  }
})

const loadOverview = async () => {
  const res = await getSystemAnalytics(buildFilter())
  if (!res.isSuccess || !res.data) {
    message.error('Không thể tải thống kê tổng quan')
    return
  }
  analytics.value = res.data
  chartConfigs.lineOptions = {
    ...chartConfigs.lineOptions,
    xaxis: {
      categories: res.data.userTrend.map((x: any) => dayjs(x.date).format('DD/MM'))
    }
  }
}

const loadRecentUsers = async () => {
  const res = await getRecentUsersAnalytics({
    pageIndex: paginationConfig.current,
    pageSize: paginationConfig.pageSize,
    isAsc: false,
    orderBy: 'CreatedAt',
    ...buildFilter()
  })

  if (!res.isSuccess || !res.data) {
    message.error('Không thể tải danh sách người dùng gần đây')
    return
  }

  userData.value = res.data.items
  paginationConfig.total = res.data.totalItems
}

const getStatusColor = (status: string): string => {
  if (status === 'Active') return 'success'
  if (status === 'Banned') return 'error'
  if (status === 'Inactive') return 'warning'
  return 'default'
}

onMounted(async () => {
  await loadOverview()
  await loadRecentUsers()
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
