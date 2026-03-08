import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'
import LoginView from '@/views/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/',
      name: 'home',
      component: MainLayout,
      children: [
        {
          path: '/',
          name: 'overview',
          component: () => import('@/views/OverviewView.vue')
        },
        {
          path: '/users',
          name: 'users',
          component: () => import('@/views/UsersView.vue')
        },
        {
          path: '/topics',
          name: 'topics',
          component: () => import('@/views/TopicsView.vue')
        },
        {
          path: '/questions',
          name: 'questions',
          component: () => import('@/views/QuestionsView.vue')
        },
        {
          path: '/settings',
          name: 'settings',
          component: () => import('@/views/SettingsView.vue')
        },
      ]
    }
  ]
})

export default router
