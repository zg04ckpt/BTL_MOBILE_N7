<script lang="ts" setup>
    import { EditOutlined, DatabaseFilled, DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
    import { ref } from 'vue';

    const key = ref<string>('');
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

        <a-button type="primary">
            <PlusOutlined />
            Thêm chủ đề
        </a-button>
        
    </div>

    <div class="row g-3">

        <div class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button"/>
                        <DeleteOutlined class="icon-button ms-2"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">Tên chủ đề</div>
                        <small class="text-secondary">Mô tả chủ đề</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">156 câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>

                <div class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button"/>
                        <DeleteOutlined class="icon-button ms-2"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">Tên chủ đề</div>
                        <small class="text-secondary">Mô tả chủ đề</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">156 câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>

                <div class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button"/>
                        <DeleteOutlined class="icon-button ms-2"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">Tên chủ đề</div>
                        <small class="text-secondary">Mô tả chủ đề</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">156 câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>

                <div class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button"/>
                        <DeleteOutlined class="icon-button ms-2"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">Tên chủ đề</div>
                        <small class="text-secondary">Mô tả chủ đề</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">156 câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>

                <div class="col-3">
            <div class="card card-body p-0 overflow-hidden d-flex">
                <div class="w-100 box-header" style="height: 40px;"></div>
                <div class="p-3">
                    <div class="d-flex align-items-start">
                        <div class="header-icon d-flex justify-content-center align-items-center text-white">
                            <DatabaseFilled class="fs-4"/>
                        </div>
                        <div class="flex-fill"></div>
                        <EditOutlined class="icon-button"/>
                        <DeleteOutlined class="icon-button ms-2"/>

                    </div>
                    <div class="d-flex flex-column">
                        <div class="fw-bold mt-1">Tên chủ đề</div>
                        <small class="text-secondary">Mô tả chủ đề</small>
                    </div>
                    <div class="d-flex mt-1">
                        <div class="px-3 py-1 rounded-3 bg-secondary-subtle text-black">156 câu hỏi</div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <div class="status-tag ps-4 pe-3 py-1">Active</div>
                    </div>
                </div>
            </div>
        </div>

    </div>

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