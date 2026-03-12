<script setup lang="ts">
import { RouterView } from 'vue-router'
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
