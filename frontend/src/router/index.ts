import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/HomePage.vue'
import AuthPage from "../pages/AuthPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";

const routes = [
    {
        path: '/',
        name: 'login',
        component: AuthPage
    },
    {
        path: '/homepage',
        name: 'homepage',
        component: HomePage
    },
    {
        path: '/register',
        name: 'register',
        component: RegisterPage
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router