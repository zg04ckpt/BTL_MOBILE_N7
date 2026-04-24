<template>
  <div class="px-4">
    <div class="container-fluid mt-3">
      <div class="row gap-5 justify-content-center">
        <div class="col-md-2 rounded-3 border px-3 py-4 text-center">
          <div class="text-primary fs-5 text-semi-bold">Trận đang diễn ra</div>
          <div class="text-primary fs-1 text-semi-bold">12</div>
        </div>
        <div class="col-md-2 rounded-3 border px-3 py-4 text-center">
          <div class="text-primary fs-5 text-semi-bold">Trận trong ngày</div>
          <div class="text-primary fs-1 text-semi-bold">145</div>
        </div>
        <div class="col-md-2 rounded-3 border px-3 py-4 text-center">
          <div class="text-primary fs-5 text-semi-bold">Người chơi đồng thời</div>
          <div class="text-primary fs-1 text-semi-bold">4</div>
        </div>
      </div>
    </div>

    <a-table
      :columns="columns"
      :pagination="paginationConfig"
      :data-source="matchesData"
      :loading="isLoading"
      @change="handleChangePageSize"
      class="text-semi-bold mt-5"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'time'">
          <div class="flex">
            <span>{{ record.startTime }}</span>
            <template v-if="record.endTime">
              <span>{{ ` - ${record.endTime}` }}</span>
            </template>
          </div>
        </template>

        <template v-if="column.key === 'action'">
          <div class="text-primary fs-5">
            <eye-outlined class="cursor-pointer" />
          </div>
        </template>
      </template>
    </a-table>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue'
import { getAllMatches } from '@/api'
import { EyeOutlined } from '@ant-design/icons-vue'
import { MatchListItemDto, SearchMatchRequest } from '@/types'

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80 },
  { title: 'Thời gian', dataIndex: 'time', key: 'time' },
  { title: 'Loại trận đấu', dataIndex: 'type', key: 'type' },
  { title: 'Số lượng người chơi', dataIndex: 'numberOfPlayers', key: 'numberOfPlayers' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Chủ đề', dataIndex: 'topic', key: 'topic' },
  { title: 'Hành động', key: 'action', align: 'center' }
]

interface MatchTableItem {
  id: number;
  startTime: string;
  endTime: string | null;
  type: string;
  numberOfPlayers: number;
  status: string;
  topic: string;
}

const matchesData = ref<MatchTableItem[]>([])
const isLoading = ref(false)

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  total: 0,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50']
})

const formatLocalDateTime = (dateValue?: string | null): string | null => {
  if (!dateValue) return null
  const date = new Date(dateValue)
  if (Number.isNaN(date.getTime())) return null
  return date.toLocaleString('vi-VN')
}

const mapToTableItem = (item: MatchListItemDto): MatchTableItem => ({
  id: item.id,
  startTime: formatLocalDateTime(item.from) || '-',
  endTime: formatLocalDateTime(item.to),
  type: item.battleType,
  numberOfPlayers: item.numberOfPlayers,
  status: item.battleStatus,
  topic: item.topicName || 'Tất cả'
})

const loadMatches = async (): Promise<void> => {
  const request: SearchMatchRequest = {
    pageIndex: paginationConfig.current,
    pageSize: paginationConfig.pageSize,
    isAsc: false,
    orderBy: 'CreatedAt'
  }

  isLoading.value = true
  const res = await getAllMatches(request)
  isLoading.value = false

  if (!res.isSuccess || !res.data) {
    matchesData.value = []
    paginationConfig.total = 0
    return
  }

  matchesData.value = res.data.items.map(mapToTableItem)
  paginationConfig.total = res.data.totalItems
}

const handleChangePageSize = async (pagination: any): Promise<void> => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
  await loadMatches()
}

onMounted(async () => {
  await loadMatches()
})
</script>
<style scoped>
.text-semi-bold {
  font-weight: 500;
}

.cursor-pointer {
  cursor: pointer;
  transition: opacity 0.2s;
}

.cursor-pointer:hover {
  opacity: 0.7;
}

:deep(.ant-table-thead > tr > th) {
  background-color: #2f56ed;
  color: #fff;
}
</style>
