import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/HomePage.vue'
import AuthPage from "../pages/AuthPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";
import GroupReviewView from "../pages/GroupReviewView.vue";
import {apiGet} from "../services/api.ts";
import MyReviewsView from "../pages/MyReviewsView.vue";

const routes = [
    {
        path: '/',
        name: 'homepage',
        component: HomePage,
        meta: { requiresAuth: true, requiresLayout: true }
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
        meta: { requiresAuth: true, requiresLayout: true }
    },
    {
        path: '/myreviews',
        name: 'MyReviews',
        component: MyReviewsView,
        meta: { requiresAuth: true, requiresLayout: true }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

let isUserValidated = false

router.beforeEach(async (to, _, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (isUserValidated) {
            return next()
        }

        try {
            await apiGet('/api/user/validate')
            isUserValidated = true
            next()
        } catch (error) {
            isUserValidated = false
            next('/login')
        }
    }
})
export default router