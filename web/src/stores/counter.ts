import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { LoginSesssionDto } from '@/types/auth';

export const useCounterStore = defineStore('counter', () => {
	const count = ref(0)
	const doubleCount = computed(() => count.value * 2)

	function increment() {
		count.value++
	}

	function decrement() {
		count.value--
	}

	return { count, doubleCount, increment, decrement };
});

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
