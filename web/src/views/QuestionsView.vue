<script lang="ts" setup>
    import TableItemActionComponent from '@/components/TableItemActionComponent.vue';
    import { UploadOutlined, DeleteOutlined } from '@ant-design/icons-vue';
    import { computed, h, ref } from 'vue';

    const key = ref<string>('');
    const topic = ref<string>('');
    const level = ref<string>('');
    const status = ref<string>('');
    const selectedRowKeys = ref<number[]>([]);
    const isSelectRowStatus = ref<boolean>(false);

    const columns = [
        {
            title: 'ID',
            dataIndex: 'id',
            key: 'id',
            width: 50
        },
        {
            title: 'Nội dung câu hỏi',
            dataIndex: 'content',
            key: 'content',
            width: 500
        },
        {
            title: 'Chủ đề',
            dataIndex: 'topic',
            key: 'topic',
            width: 180
        },
        {
            title: 'Level',
            dataIndex: 'level',
            key: 'level',
            width: 180
        },
        {
            title: 'Đáp án đúng',
            dataIndex: 'correctAnswer',
            key: 'correctAnswer',
            width: 200
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            width: 180,
        },
        {
            title: 'Hành động',
            dataIndex: 'action',
            key: 'action',
            width: 80,
            fixed: 'right',
            customRender: () => h(TableItemActionComponent)
        },
    ];
    const data = [
        {
            id: 1,
            content: "2 + 2 bằng bao nhiêu?",
            topic: "Toán học",
            level: "Dễ",
            correctAnswer: "4",
            status: "Hoạt động",
            action: ""
        },
        {
            id: 2,
            content: "Thủ đô của Việt Nam là gì?",
            topic: "Địa lý",
            level: "Dễ",
            correctAnswer: "Hà Nội",
            status: "Hoạt động",
            action: ""
        },
        {
            id: 3,
            content: "Ai là người phát minh ra bóng đèn?",
            topic: "Khoa học",
            level: "Trung bình",
            correctAnswer: "Thomas Edison",
            status: "Hoạt động",
            action: ""
        },
        {
            id: 4,
            content: "HTML viết tắt của cụm từ nào?",
            topic: "Lập trình",
            level: "Trung bình",
            correctAnswer: "HyperText Markup Language",
            status: "Hoạt động",
            action: ""
        },
        {
            id: 5,
            content: "CPU là viết tắt của gì?",
            topic: "Công nghệ",
            level: "Khó",
            correctAnswer: "Central Processing Unit",
            status: "Tạm khóa",
            action: ""
        }
    ];
    const rowSelection = computed(() => ({
        selectedRowKeys: selectedRowKeys.value,
        onChange: (keys: number[]) => {
            selectedRowKeys.value = keys
            if (keys.length > 0) {
                isSelectRowStatus.value = true;
            } else {
                isSelectRowStatus.value = false;
            }
        }
    }));
</script>

<template >
    <!-- Title + Action -->
    <div class="d-flex align-items-center mb-3">
        <h2>Quản lý câu hỏi</h2>
        <div class="flex-fill"></div>
        <a-button type="primary">
            <UploadOutlined />
            Thêm câu hỏi bằng Excel
        </a-button>
        
    </div>

    <!-- Search + Filter -->
    <div v-if="!isSelectRowStatus" class="d-flex align-items-center mb-3" style="height: 40px;">
        <a-input-search
            class="w-25"
            v-model:value="key"
            placeholder="Nhập nội dung câu hỏi"
            loading:value="false"
            enter-button/>
        
        <div class="flex-fill"></div>
        <a-select
            class="me-2"
            ref="select"
            v-model:value="topic"
            style="width: 120px"
            @focus="() => {}"
            @change="() => {}">
            <a-select-option value="" selected>Mọi chủ đề</a-select-option>
        </a-select>
        <a-select
            class="me-2"
            ref="select"
            v-model:value="level"
            style="width: 120px"
            @focus="() => {}"
            @change="() => {}">
            <a-select-option value="">Mọi cấp độ</a-select-option>
            <a-select-option value="Normal">Dễ</a-select-option>
            <a-select-option value="Medium">Trung bình</a-select-option>
            <a-select-option value="Hard">Khó</a-select-option>
        </a-select>
        <a-select
            class=""
            ref="select"
            v-model:value="status"
            style="width: 140px"
            @focus="() => {}"
            @change="() => {}">
            <a-select-option value="">Mọi trạng thái</a-select-option>
            <a-select-option value="Draft">Đang chỉnh sửa</a-select-option>
            <a-select-option value="Pending">Đang đợi duyệt</a-select-option>
            <a-select-option value="Approved">Đã duyệt</a-select-option>
            <a-select-option value="Rejected">Đã từ chối</a-select-option>
        </a-select>
    </div>

    <!-- Action for selected rows -->
    <div v-if="isSelectRowStatus" class="d-flex align-items-center mb-3" style="height: 40px;">
        <div class="align-content-center fw-bold">Đã chọn {{ selectedRowKeys.length }}</div>
        <a-button type="primary" class="ms-3">
            <DeleteOutlined />
            Xóa
        </a-button>
    </div>

    <!-- Table data -->
    <div class="card card-body p-2">
        <a-table 
            rowKey="id"
            size="small" 
            :row-selection="rowSelection" 
            :columns="columns" 
            :scroll="{ x: 'max-content' }"
            :pagination="{
                pageSize: 50,
                showSizeChanger: true
            }"
            :data-source="data" />
    </div>
</template>

<style scoped>
</style>
