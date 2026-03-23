<template >
    <div class="login-background">
        <div class="card card-body login-dialog">
            <div class="fw-bold mb-3">Đăng nhập vào QB Management</div>
            <a-form :model="formData" autocomplete="off">
                <!-- Email -->
                <a-form-item
                    name="email"
                    class="mb-4"
                    :rules="[{ required: true, message: 'Vui lòng nhập email' }]">
                    <a-input 
                        v-model:value="formData.email"
                        placeholder="Email" />
                </a-form-item>
                <!-- Mật khẩu -->
                <a-form-item
                    name="pass"
                    :rules="[{ required: true, message: 'Vui lòng nhập mật khẩu' }]">
                    <a-input-password 
                        type="password"
                        v-model:value="formData.pass"
                        placeholder="Mật khẩu" />
                </a-form-item>
            </a-form>
            <!-- Nút đăng nhập -->
            <a-button type="primary" 
                html-type="button"
                :loading="loading" class="float-end" @click.prevent="handleLoginAsync">Đăng nhập</a-button>
        </div>
    </div>
</template>

<script setup lang="ts">

import { login } from '@/api';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
import { reactive, ref } from 'vue';
import { useLoginSessionStore } from '@/stores/loginSesssion';

const loginInfoStore = useLoginSessionStore();
const router = useRouter();
const loading = ref(false);
const formData = reactive({
    email: "",
    pass: ""
});

const handleLoginAsync = async () => {
    try {
        loading.value = true;
        const hideLoadingAlert = message.loading("Đang đăng nhập...", 0);
    
        const res = await login(formData.email, formData.pass);
    
        hideLoadingAlert();
        loading.value = false;
    
        if (res.isSuccess) {
            message.success("Đăng nhập thành công, xin chào " + res.data!.name + "!");
            loginInfoStore.setLoginInfo(res.data!);
            await router.replace("/");
        } else {
            message.error("Đăng nhập thất bại, " + res.message);
        }
    } catch (err: any) {
        console.log("Lỗi", err);
    }
}

</script>

<style scoped>
.login-dialog {
    position: absolute;
    top: 100px;
    left: 50%;
    transform: translateX(-50%);
    width: 500px;
}

.login-background {
    background: linear-gradient(0deg, #0094a8, #014a4e);
    width: 100vw;
    height: 100vh;
    display: block;
}
</style>