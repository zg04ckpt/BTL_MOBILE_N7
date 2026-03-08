<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div id="app">
    <header>
      <nav>
        <RouterLink to="/">Trang chủ</RouterLink>
        <RouterLink to="/about">Giới thiệu</RouterLink>
      </nav>
    </header>

    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
header {
  padding: 1rem;
  background-color: #f5f5f5;
}

nav {
  display: flex;
  gap: 1rem;
}

nav a {
  text-decoration: none;
  color: #42b883;
  font-weight: bold;
}

nav a:hover {
  color: #35495e;
}

nav a.router-link-active {
  color: #35495e;
}

main {
  padding: 2rem;
}
</style>
