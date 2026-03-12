<script setup lang="ts">

import { ref } from 'vue'

interface Props {
  title?: string
}

const props = withDefaults(defineProps<Props>(), {
  title: 'Hello World'
})

const count = ref(0)
</script>

<template>
  <div class="example-component">
    <h2>{{ title }}</h2>
    <p>Count: {{ count }}</p>
    <button @click="count++">Increment</button>
  </div>
</template>

<style scoped>
.example-component {
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 8px;
}

button {
  padding: 0.5rem 1rem;
  background-color: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #35495e;
}
</style>
