<script lang="ts" setup>
    import { createTopic, deleteTopic, getAllTopic, updateTopic } from '@/api';
import { CreateTopicRequest, TopicListItemDto } from '@/types';
import { EditOutlined, DatabaseFilled, DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { message, Modal } from 'ant-design-vue';
    import { onMounted, reactive, ref } from 'vue';

    const key = ref<string>('');
    const topics = ref<TopicListItemDto[]>([]);
    const selectedTopic = ref<TopicListItemDto>({
        id: 0,
        name: '',
        questionCount: 0,
        slug: ''
    });

    const modal = reactive<{
        show: boolean;
        name: string;
        title: 'Tạo topic mới' | 'Cập nhật topic';
        mode: 'create' | 'update';
    }>({
        show: false,
        name: '',
        title: 'Tạo topic mới',
        mode: 'create'
    });

    const handleSearchTopic = async () => {
        const hideLoading = message.loading("Đang tải danh sách topic ...", 0);
        const res = await getAllTopic();
        hideLoading();

        if (res.isSuccess) {
            topics.value = res.data!;
        } else {
            message.error(res.message);
        }
    };

    const handleSave = async () => {
        if (modal.mode == 'create') {
            const hideLoading = message.loading("Đang xử lý ...", 0);
            const res = await createTopic({ name: modal.name });
            hideLoading();
            if (res.isSuccess) {
                message.success("Tạo topic thành công");
                handleSearchTopic();
                modal.show = false;
            } else {
                message.error("Tạo topic thất bại: " + res.message);
            }
        } else {
            const hideLoading = message.loading("Đang xử lý ...", 0);
            const res = await updateTopic(selectedTopic.value.id, { name: modal.name });
            hideLoading();
            if (res.isSuccess) {
                message.success("Cập nhật topic thành công");
                handleSearchTopic();
                modal.show = false;
            } else {
                message.error("Cập nhật topic thất bại: " + res.message);
            }
        }

    }     
    
    const handleRemove = (topic: TopicListItemDto) => {
        Modal.confirm({
            title: "Xác nhận xóa",
            content: "Bạn có muốn xóa topic " + topic.name + " không ?",
            okText: "Xóa ngay",
            cancelText: "Giữ lại",
            onOk: async () => {
                const hideLoading = message.loading("Đang xử lý ...", 0);
                const res = await deleteTopic(topic.id);
                hideLoading();
                if (res.isSuccess) {
                    message.success("Xóa topic thành công");
                    handleSearchTopic();
                } else {
                    message.error("Xóa topic thất bại: " + res.message);
                }
            }
        })
    }

    onMounted(() => {
        handleSearchTopic();
    });
</script>

<template >
    <!-- Title + Action -->
    <div class="d-flex align-items-center mb-3">
        <h2>Quản lý chủ đề</h2>
        <div class="flex-fill"></div>

        <a-input-search
            class="w-25 me-3"
            v-model:value="key"
            placeholder="Nhập tên chủ đề"
            loading:value="false"
            enter-button/>

        <a-button type="primary" @click="() => {
            modal.mode = 'create';
            modal.title = 'Tạo topic mới';
            modal.name = '';
            modal.show = true;
        }">
            <PlusOutlined />
            Thêm chủ đề
        </a-button>
    </div>

    <!-- Topic -->
    <div class="row g-3">
        <div v-for="topic in topics" class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button" @click="() => {
                            selectedTopic = topic;
                            modal.name = topic.name;
                            modal.title = 'Cập nhật topic';
                            modal.mode = 'update';
                            modal.show = true;
                        }"/>
                        <DeleteOutlined class="icon-button ms-2" @click="handleRemove(topic)"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">{{ topic.name }}</div>
                        <small class="text-secondary">Chưa có mô tả</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">{{ topic.questionCount }} câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Model -->
    <a-modal v-model:open="modal.show" :title="modal.title" @ok="handleSave">
        <a-input v-model:value.lazy="modal.name" autofocus placeholder="Nhập tên topic" />
    </a-modal>
</template>

<style scoped>
.status-tag {
    background-color: #F0FDF4;
    color: #1BA54E;
    font-weight: 600;
    border-radius: 25px;
    border: 2px solid #DCFCE7;
    position: relative;
}
.status-tag::before {
    position: absolute;
    content: "";
    width: 6px;
    height: 6px;
    border-radius: 50%;
    background-color: #0dda58;
    top: 50%;
    left: 15px;
    transform: translate(-50%, -50%);
}

.box-header {
    background-color: #2F56ED;
}
.header-icon {
    width: 50px;
    aspect-ratio: 1 / 1;
    box-sizing: border-box;
    padding: 10px;
    background-color: #3B82F6;
    border-radius: 5px;
}
.icon-button:hover {
    cursor: pointer;
    opacity: 0.5;
}
</style>