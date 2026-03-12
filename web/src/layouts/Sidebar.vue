<script setup lang="ts">
import { HomeOutlined } from '@ant-design/icons-vue'
</script>

<template>
  <aside>
    <div class="d-flex flex-column w-100 p-2">
      <router-link to="/" class="item">
        <HomeOutlined />
        <div class="ms-2">Thống kê</div>
      </router-link>

      <router-link to="/users" class="item active">
        <HomeOutlined />
        <div class="ms-2">Quản lý người dùng</div>
      </router-link>

      <router-link to="/questions" class="item">
        <HomeOutlined />
        <div class="ms-2">Quản lý câu hỏi</div>
      </router-link>

      <router-link to="/settings" class="item">
        <HomeOutlined />
        <div class="ms-2">Quản lý cấu hình</div>
      </router-link>

      <router-link to="topics" class="item">
        <HomeOutlined />
        <div class="ms-2">Quản lý chủ đề</div>
      </router-link>

      <router-link to="events" class="item">
        <HomeOutlined />
        <div class="ms-2">Quản lý sự kiện</div>
      </router-link>

      <router-link to="matches" class="item">
        <HomeOutlined />
        <div class="ms-2">Quản lý trận đấu</div>
      </router-link>
    </div>
  </aside>
</template>

<style scoped>
.item {
  display: flex;
  flex-direction: row;
  height: 40px;
  align-items: center;
  padding: 10px;
  border-bottom: 2px solid #021df3;
  color: black;
  text-decoration: none;
}

.router-link-exact-active {
  color: white;
  background-color: #021df3;
}

.item:hover:not(.item.active) {
  cursor: pointer;
  background-color: #021ef372;
}

aside {
  position: fixed;
  top: 50px;
  bottom: 0;
  left: 0;
  width: 250px;
  border-right: 2px solid #021df3;
}
</style>
