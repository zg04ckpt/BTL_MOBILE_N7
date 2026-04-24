<script lang="ts" setup>
    import { EditTwoTone, DeleteTwoTone } from '@ant-design/icons-vue';
</script>

<template>
    <div class="d-flex px-2">
        <EditTwoTone class="me-2 item-hover" title="Chỉnh sửa"/>
        <DeleteTwoTone class="me-2 item-hover" title="Xóa"/>
    </div>
</template>

<style lang="css" scoped>
    .item-hover:hover {
        cursor: pointer;
        opacity: 0.5;
    }
</style>