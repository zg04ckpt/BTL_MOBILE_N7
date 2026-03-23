<template>
    <header>
        <nav class="d-flex flex-row align-items-center h-100">
            <div class="fs-3 mx-3 text-light">QB Management</div>
            <div class="flex-fill"></div>
            <BellOutlined style="font-size: 24px" class="text-white me-2"/>
            <div v-if="session.info != null" class="text-white">Xin chào {{session.info.name}}!</div>
            <a-dropdown :trigger="['click']">
                <a-avatar v-if="session.info != null" class="mx-3 border-2 border border-info" :size="40" :src="session.info!.avatarUrl"></a-avatar>
                <template #overlay>
                    <a-menu>
                        <a-menu-item key="0" @click="handleLogout">
                            Đăng xuất
                        </a-menu-item>
                    </a-menu>
                </template>
            </a-dropdown>
        </nav>
    </header>
</template>

<script setup lang="ts">
    import { logout } from '@/api';
import router from '@/router';
import { useLoginSessionStore } from '@/stores/loginSesssion';
    import { BellOutlined } from '@ant-design/icons-vue';
    import { message, Modal } from 'ant-design-vue';

    const session = useLoginSessionStore();

    const handleLogout = () => {
        Modal.confirm({
            title: "Xác nhận đăng xuất?",
            content: "Sau khi đăng xuất, phiên đăng nhập của bạn sẽ kết thúc ngay lập tức",
            okText: "Đăng xuất ngay",
            cancelText: "Ở lại",
            onOk: async () => {
                const res = await logout();
                if (res.isSuccess) {
                    message.success("Đăng xuất thành công!");
                }
                router.replace("/login");
            }
        });
    }
</script>

<style scoped>
header {
    position: fixed;
    top: 0;
    right: 0;
    left: 0;
    height: 50px;
    background-color: #2F56ED;
}
</style>