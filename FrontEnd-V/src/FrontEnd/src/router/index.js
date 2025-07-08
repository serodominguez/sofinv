import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '@/views/DashboardView.vue'
import AboutView from '@/views/AboutView.vue'
import BrandsView from '@/views/BrandsView.vue'
import CategoriesView from '@/views/CategoriesView.vue'
import CustomersView from '@/views/CustomersView.vue'
import GoodsIssueView from '@/views/GoodsIssue.vue'
import GoodsReceiptView from '@/views/GoodsReceipt.vue'
import LoginView from '@/views/LoginView.vue'
import ProductsView from '@/views/ProductsView.vue'
import RolesView from '@/views/RolesView.vue'
import StoresView from '@/views/StoresView.vue'
import SuppliersView from '@/views/SuppliersView.vue'
import UsersView from '@/views/UsersView.vue'
import store from '@/store' 

const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: DashboardView,
    meta: {
      administrator: true,
      grocer: true,
      operator: true
    }
  },
  {
    path: '/about',
    name: 'about',
    component: AboutView,
    meta: {
      administrator: true,
      grocer: true,
      operator: true
    }
  },
  {
    path: '/brands',
    name: 'brands',
    component: BrandsView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/categories',
    name: 'categories',
    component: CategoriesView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/customers',
    name: 'customers',
    component: CustomersView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/issues',
    name: 'issues',
    component: GoodsIssueView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/receipts',
    name: 'receipts',
    component: GoodsReceiptView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView,
    meta: {
      free: true,
    }
  },
  {
    path: '/roles',
    name: 'roles',
    component: RolesView,
    meta: {
      administrator: true,
    }
  },
  {
    path: '/products',
    name: 'products',
    component: ProductsView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/stores',
    name: 'stores',
    component: StoresView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/suppliers',
    name: 'suppliers',
    component: SuppliersView,
    meta: {
      administrator: true,
      grocer: true,
    }
  },
  {
    path: '/users',
    name: 'users',
    component: UsersView,
    meta: {
      administrator: true,
    }
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  if(to.matched.some(record => record.meta.free)){
    next()
  } else if (store.state.currentUser && store.state.currentUser.role == 'ADMINISTRADORES'){
    if (to.matched.some(record => record.meta.administrator)){
        next()
    }
  } else if (store.state.currentUser && store.state.currentUser.role == 'ALMACENEROS'){
    if (to.matched.some(record => record.meta.grocer)){
        next()
    }
  } else if (store.state.currentUser && store.state.currentUser.role == 'OPERARIOS'){
    if (to.matched.some(record => record.meta.operator)){
        next()
    }
  } else {
    next({
      name: 'login'
    })
  }
})

export default router
