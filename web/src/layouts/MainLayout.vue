<script lang="ts" setup >

</script>

<template>
    <!-- Nav bar -->
    <header>
      <nav>
        <RouterLink to="/">Trang chủ</RouterLink>
        <RouterLink to="/about">Giới thiệu</RouterLink>
      </nav>
    </header>

    <!-- Sidebar -->
    <aside>
        
    </aside>

    <!-- Content -->
    <div class="content">
        <RouterView />
    </div>
</template>