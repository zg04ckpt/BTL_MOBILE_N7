import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '@/views/layouts/MainLayout.vue'
import LoginView from '@/views/LoginView.vue'
import Forbidden from '@/views/static/Forbidden.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/403',
      name: 'forbidden',
      component: Forbidden
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
        {
          path: '/matches',
          name: 'matches',
          component: () => import('@/views/MatchesView.vue')
        }
      ]
    }
  ]
})

export default router
