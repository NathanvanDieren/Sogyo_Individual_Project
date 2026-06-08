import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/HomePage.vue'
import AuthPage from "../pages/AuthPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";
import GroupReviewView from "../pages/GroupReviewView.vue";
import {apiGet} from "../services/api.ts";

const routes = [
    {
        path: '/',
        name: 'homepage',
        component: HomePage,
        meta: { requiresAuth: true }
    },
    {
        path: '/login',
        name: 'login',
        component: AuthPage
    },
    {
        path: '/register',
        name: 'register',
        component: RegisterPage
    },
    {
        path: '/group/:id',
        name: 'GroupReviews',
        component: GroupReviewView,
        props: true,
        meta: { requiresAuth: true }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach(async (to, _, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        try {

            await apiGet('/api/user/validate')
            next()
        } catch (error) {
            next('/login')
        }
    } else {
        next()
    }
})

export default router