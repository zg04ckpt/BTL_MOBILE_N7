<template>
  <div class="px-4">
    <section class="d-flex align-items-center justify-content-between mb-4">
      <a-typography-title :level="2">Quản lý câu hỏi</a-typography-title>
      <div class="d-flex gap-2">
        <a-button>Duyệt câu hỏi (1)</a-button>
        <a-button>Import Excel</a-button>
        <a-button class="d-flex gap-1 align-items-center" type="primary">
          <plus-outlined /> Thêm câu hỏi
        </a-button>
      </div>
    </section>
    <div class="container-fluid border p-3 rounded mb-3">
      <div class="row g-5">
        <div class="col-md-5">
          <a-input v-model:value="filter.keyword" placeholder="Tìm kiếm nội dung câu hỏi..." />
        </div>
        <div class="col-md-2">
          <a-select class="w-100" v-model:value="filter.topic" placeholder="Chọn chủ đề">
            <a-option value="Tất cả chủ đề">Tất cả chủ đề</a-option>
            <a-option value="Lịch sử">Lịch sử</a-option>
            <a-option value="Toán học">Toán học</a-option>
            <a-option value="Tin học">Tin học</a-option>
          </a-select>
        </div>
        <div class="col-md-2">
          <a-select
            class="w-100"
            :dropdownMatchSelectWidth="false"
            v-model:value="filter.level"
            placeholder="Độ khó"
          >
            <a-option value="Dễ">Dễ</a-option>
            <a-option value="Trung bình">Trung bình</a-option>
            <a-option value="Khó">Khó</a-option>
            <a-option value="Rất khó">Tin học</a-option>
          </a-select>
        </div>
        <div class="col-md-3">
          <a-select
            class="w-100"
            :dropdownMatchSelectWidth="false"
            v-model:value="filter.status"
            placeholder="Trạng thái"
          >
            <a-option value="Hoạt động">Hoạt động</a-option>
            <a-option value="Chờ duyệt">Chờ duyệt</a-option>
            <a-option value="Ngừng hoạt động">Ngừng hoạt động</a-option>
          </a-select>
        </div>
      </div>
    </div>

    <a-table
      :columns="columns"
      :data-source="questionData"
      :pagination="paginationConfig"
      :bordered="true"
      @change="handleChangePageSize"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'status'">
          <a-tag :color="getStatusColor(record.status)">{{ record.status }}</a-tag>
        </template>

        <template v-if="column.key === 'action'">
          <div class="d-flex text-primary gap-4 fs-5 justify-content-center">
            <template v-if="record.status === 'Chờ duyệt'">
              <check-circle-outlined class="text-success cursor-pointer" />
              <close-circle-outlined class="text-danger cursor-pointer" />
            </template>
            <template v-else>
              <edit-outlined class="text-primary cursor-pointer" />
              <eye-invisible-outlined class="text-secondary cursor-pointer" />
              <delete-outlined class="text-danger cursor-pointer" />
            </template>
          </div>
        </template>
      </template>
    </a-table>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import {
  PlusOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  EditOutlined,
  EyeInvisibleOutlined,
  DeleteOutlined
} from '@ant-design/icons-vue'
import { questionData } from '@/mocks/question'
import { getStatusColor } from '@/utils/helper'
import { QuestionItem } from '@/types/question'

const filter = reactive({
  keyword: '',
  topic: 'Tất cả chủ đề',
  level: null,
  status: null
})

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 70 },
  { title: 'Nội dung câu hỏi', dataIndex: 'content', key: 'content' },
  { title: 'Chủ đề', dataIndex: 'topic', key: 'topic' },
  { title: 'Độ khó', dataIndex: 'level', key: 'level' },
  {
    title: 'Đáp án đúng',
    key: 'correctAnswer',
    customRender: ({ record }: { record: QuestionItem }) => {
      const correct = record.answers.find((ans) => ans.isCorrect)
      return correct ? correct.content : 'Chưa có đáp án'
    }
  },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Hành động', key: 'action', align: 'center', width: 150 }
]

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20']
})

const handleChangePageSize = (pagination: any): void => {
  paginationConfig.current = pagination.current
  paginationConfig.pageSize = pagination.pageSize
}
</script>

<style scoped>
:deep(.ant-table-thead > tr > th) {
  background-color: #d9d9d9;
}

.cursor-pointer {
  cursor: pointer;
  transition: opacity 0.2s;
}

.cursor-pointer:hover {
  opacity: 0.7;
}
</style>
