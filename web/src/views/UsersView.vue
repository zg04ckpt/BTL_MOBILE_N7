<template>
  <div class="p-4 container-fluid">
    <div class="row g-4 mb-5">
      <div class="col-md-3">
        <a-input
          v-model:value="filter.keyword"
          placeholder="Nhập tên đăng nhập, email, số điện thoại"
        />
      </div>
      <div class="col-md-2 d-flex justify-content-center">
        <a-select class="w-100" v-model:value="filter.status" placeholder="Chọn trạng thái">
          <a-option value="Hoạt động">Hoạt động</a-option>
          <a-option value="Ngừng hoạt động">Ngừng hoạt động</a-option>
          <a-option value="Khoá">Khoá</a-option>
        </a-select>
      </div>
      <div class="col-md-2 d-flex justify-content-center">
        <a-date-picker class="w-100" v-model:value="filter.startDate" placeholder="Từ ngày" />
      </div>
      <div class="col-md-2 d-flex justify-content-center">
        <a-date-picker class="w-100" v-model:value="filter.endDate" placeholder="Đến ngày" />
      </div>
      <div class="col-md-3 d-flex justify-content-around gap-4">
        <a-button class="w-50" type="primary">Tìm kiếm</a-button>
        <a-button class="w-50" @click="resetFilter">Đặt lại</a-button>
      </div>
    </div>

    <a-table
      :columns="columns"
      :data-source="userData"
      :pagination="paginationConfig"
      :bordered="true"
      @change="handleChangePageSize"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'username'">
          <div class="d-flex align-items-center gap-2">
            <a-avatar :src="record.avatar" :size="32" />
            <span>{{ record.name }}</span>
          </div>
        </template>

        <template v-if="column.key === 'level'">
          <span>{{ `Lv.${record.level}` }}</span>
        </template>

        <template v-if="column.key === 'status'">
          <a-tag :color="getStatusColor(record.status)">{{ record.status }}</a-tag>
        </template>

        <template v-if="column.key === 'action'">
          <div class="d-flex text-primary gap-4 fs-5 justify-content-center">
            <eye-outlined class="cursor-pointer" />
            <edit-outlined class="cursor-pointer" />
            <lock-outlined class="cursor-pointer" />
          </div>
        </template>
      </template>
    </a-table>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { userData } from '@/mocks/user'
import { getStatusColor } from '@/utils/helper'
import { EyeOutlined, EditOutlined, LockOutlined } from '@ant-design/icons-vue'

const filter = reactive({
  keyword: '',
  status: null,
  startDate: null,
  endDate: null
})

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80 },
  { title: 'Tên đăng nhập', dataIndex: 'username', key: 'username' },
  { title: 'Số điện thoại', dataIndex: 'phone', key: 'phone' },
  { title: 'Cấp độ', dataIndex: 'level', key: 'level' },
  { title: 'Ngày đăng ký', dataIndex: 'createdAt', key: 'createdAt' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Hành động', key: 'action', align: 'center' }
]

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50']
})

const resetFilter = () => {
  Object.assign(filter, { keyword: '', status: null, startDate: null, endDate: null })
}

const handleChangePageSize = (pagination: any): void => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
}
</script>

<style scoped>
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
