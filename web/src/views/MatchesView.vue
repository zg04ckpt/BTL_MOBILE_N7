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
      @change="handleChangePageSize"
      class="text-semi-bold mt-5"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'time'">
          <div class="flex">
            <span>{{ record.startTime }}</span>
            <template v-if="record.endTime !== null">
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
import { reactive } from 'vue'
import { matchesData } from '@/mocks/matches'
import { EyeOutlined } from '@ant-design/icons-vue'

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80 },
  { title: 'Thời gian', dataIndex: 'time', key: 'time' },
  { title: 'Loại trận đấu', dataIndex: 'type', key: 'type' },
  { title: 'Số lượng người chơi', dataIndex: 'numberOfPlayers', key: 'numberOfPlayers' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Chủ đề', dataIndex: 'topic', key: 'topic' },
  { title: 'Hành động', key: 'action', align: 'center' }
]

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50']
})

const handleChangePageSize = (pagination: any): void => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
}
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
