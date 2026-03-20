<template>
    <div class="background">
        <div class="position-absolute start-50 top-50 translate-middle text-center text-light">
            <div class="error">403</div>
            <div class="message fst-italic">Bạn không có quyền thực hiện hành động này</div>
        </div>
    </div>
</template>

<style scoped>
.error {
    font-size: 300px;
    font-weight: 700;
    line-height: 1.1;
}
.background {
    background: linear-gradient(0deg, #0094a8, #014a4e);
    width: 100vw;
    height: 100vh;
    display: block;
}
</style>