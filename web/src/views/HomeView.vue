<script setup lang="ts">
import { ref } from 'vue'

const message = ref('Chào mừng đến với Vue 3!')
</script>

<template>
  <div class="home">
    <h1>{{ message }}</h1>
    <p>Đây là trang chủ của ứng dụng Vue.</p>
  </div>
</template>

<style scoped>
.home {
  text-align: center;
}

h1 {
  color: #42b883;
  margin-bottom: 1rem;
}

p {
  color: #666;
}
</style>
