import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { LoginSesssionDto } from '@/types/auth';

export const useLoginSessionStore = defineStore('userSessionDataStore', () => {
	const info = ref<LoginSesssionDto|null>(null);
	const setLoginInfo = (data: LoginSesssionDto) => info.value = data;
	const clearLoginInfo = () => info.value = null;

	return {
		info,
		setLoginInfo,
		clearLoginInfo
	}
});
