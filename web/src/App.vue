<script setup lang="ts">
import { onMounted } from 'vue';
import { RouterView } from 'vue-router';
import { getLoginInfo } from './api';
import { useLoginSessionStore } from './stores/counter';
import { message } from 'ant-design-vue';

const session = useLoginSessionStore();

// Check if not logged in => redirect to login page
onMounted(async () => {
    const res = await getLoginInfo();
    if (res.isSuccess) {
        session.setLoginInfo(res.data!);
    } else {
        message.error("Vui lòng đăng nhập để tiếp tục");
        if (window.location.pathname != "/login") {
            window.location.href = "/login";
        }
    }
});

</script>

<template>
    <div id="app">
        <main>
            <RouterView />
        </main>
    </div>
</template>

<style scoped>
:deep(.anticon) {
    display: inline-flex;
    align-items: center;
}
</style>
