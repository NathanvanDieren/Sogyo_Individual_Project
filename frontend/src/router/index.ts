import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../pages/HomePage.vue'
import AuthPage from "../pages/AuthPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";
import GroupReviewView from "../pages/GroupReviewView.vue";
import { apiGet } from "../services/api.ts";
import MyReviewsView from "../pages/MyReviewsView.vue";


const routes = [
    {
        path: '/',
        name: 'homepage',
        component: HomePage,
        meta: { title: 'Home - TasteBuds',  requiresAuth: true, requiresLayout: true }
    },
    {
        path: '/login',
        name: 'login',
        component: AuthPage,
        meta: { title: 'Login - TasteBuds'}
    },
    {
        path: '/register',
        name: 'register',
        component: RegisterPage,
        meta: { title: 'Registreer - TasteBuds'}
    },
    {
        path: '/group/:id',
        name: 'groupreviews',
        component: GroupReviewView,
        props: true,
        meta: { title: 'Groep - TasteBuds', requiresAuth: true, requiresLayout: true }
    },
    {
        path: '/myreviews',
        name: 'myreviews',
        component: MyReviewsView,
        meta: { title: 'Mijn Reviews - TasteBuds', requiresAuth: true, requiresLayout: true }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

let isUserValidated = false

router.beforeEach(async (to, _, next) => {
    document.title = (to.meta.title as string) || 'Standaard Titel';
    if (to.matched.some(record => record.meta.requiresAuth)) {

        if (isUserValidated) {
            return next()
        }

        try {
            await apiGet('/api/user/validate')
            isUserValidated = true
            return next()
        } catch (error) {
            isUserValidated = false
            if (to.name === 'login') {
                return next()
            }
            return next({ name: 'login' })
        }
    }

    next()
})

export default router