<script setup lang="ts">
    import { BellOutlined } from '@ant-design/icons-vue';
</script>

<template>
    <header>
        <nav class="container d-flex flex-row align-items-center h-100">
            <div class="flex-fill"></div>
            <BellOutlined style="font-size: 24px" class="text-white me-2"/>
            <div class="text-white">Xin chào admin!</div>
            <a-avatar class="mx-3 border-2 border border-info" :size="40" src="/src/assets/images/test-avatar.jpg"></a-avatar>
        </nav>
    </header>
</template>

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