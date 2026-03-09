<script lang="ts" setup >
    import Header from './Header.vue';
    import Sidebar from './Sidebar.vue';
</script>

<template>
    <!-- Header -->
    <Header/>

    <!-- Sidebar -->
    <Sidebar />

    <!-- Content -->
    <div class="content m-3">
        <RouterView />
    </div>
</template>

<style lang="css" scoped>
    .content {
        position: fixed;
        top: 50px;
        left: 250px;
        bottom: 0;
        right: 0;
    }
</style>