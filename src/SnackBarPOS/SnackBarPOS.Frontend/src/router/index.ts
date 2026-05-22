import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: () => import('@/pages/DashboardPage.vue'),
      meta: { title: 'Dashboard' }
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/pages/RegisterPage.vue'),
      meta: { title: 'Kassa' }
    },
    {
      path: '/orders',
      name: 'orders',
      component: () => import('@/pages/OrdersPage.vue'),
      meta: { title: 'Bestellingen' }
    },
    {
      path: '/orders/:id',
      name: 'order-detail',
      component: () => import('@/pages/OrderDetailPage.vue'),
      meta: { title: 'Bestelling Details' },
      props: true
    },
    {
      path: '/products',
      name: 'products',
      component: () => import('@/pages/ProductsPage.vue'),
      meta: { title: 'Producten' }
    },
    {
      path: '/products/new',
      name: 'product-new',
      component: () => import('@/pages/ProductFormPage.vue'),
      meta: { title: 'Nieuw Product' }
    },
    {
      path: '/products/:id/edit',
      name: 'product-edit',
      component: () => import('@/pages/ProductFormPage.vue'),
      meta: { title: 'Product Bewerken' },
      props: true
    },
    {
      path: '/categories',
      name: 'categories',
      component: () => import('@/pages/CategoriesPage.vue'),
      meta: { title: 'Categorieën' }
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/pages/NotFoundPage.vue'),
      meta: { title: 'Niet gevonden' }
    }
  ]
})

router.afterEach((to) => {
  const title = to.meta?.title as string | undefined
  document.title = title ? `${title} | SnackBar POS` : 'SnackBar POS'
})

export default router
