import axios from 'axios'
import type {
  Category, Product, Order, DailySummary,
  CreateCategoryRequest, UpdateCategoryRequest,
  CreateProductRequest, UpdateProductRequest,
  AddOrderItemRequest, PayOrderRequest
} from '@/types'

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' }
})

// Categories
export const categoryApi = {
  getAll: () => api.get<Category[]>('/categories').then(r => r.data),
  getById: (id: number) => api.get<Category>(`/categories/${id}`).then(r => r.data),
  create: (data: CreateCategoryRequest) => api.post<Category>('/categories', data).then(r => r.data),
  update: (id: number, data: UpdateCategoryRequest) => api.put<Category>(`/categories/${id}`, data).then(r => r.data),
  delete: (id: number) => api.delete(`/categories/${id}`)
}

// Products
export const productApi = {
  getAll: (includeInactive = false, categoryId?: number) => {
    const params: Record<string, unknown> = {}
    if (includeInactive) params.includeInactive = true
    if (categoryId) params.categoryId = categoryId
    return api.get<Product[]>('/products', { params }).then(r => r.data)
  },
  getById: (id: number) => api.get<Product>(`/products/${id}`).then(r => r.data),
  create: (data: CreateProductRequest) => api.post<Product>('/products', data).then(r => r.data),
  update: (id: number, data: UpdateProductRequest) => api.put<Product>(`/products/${id}`, data).then(r => r.data),
  delete: (id: number) => api.delete(`/products/${id}`)
}

// Orders
export const orderApi = {
  getAll: (from?: string, to?: string) => {
    const params: Record<string, string> = {}
    if (from) params.from = from
    if (to) params.to = to
    return api.get<Order[]>('/orders', { params }).then(r => r.data)
  },
  getById: (id: number) => api.get<Order>(`/orders/${id}`).then(r => r.data),
  getCurrent: () => api.get<Order>('/orders/current').then(r => r.data),
  getTodaySummary: () => api.get<DailySummary>('/orders/summary/today').then(r => r.data),
  create: () => api.post<Order>('/orders').then(r => r.data),
  addItem: (id: number, data: AddOrderItemRequest) =>
    api.post<Order>(`/orders/${id}/items`, data).then(r => r.data),
  updateItem: (id: number, itemId: number, quantity: number) =>
    api.patch<Order>(`/orders/${id}/items/${itemId}`, { quantity }).then(r => r.data),
  removeItem: (id: number, itemId: number) =>
    api.delete<Order>(`/orders/${id}/items/${itemId}`).then(r => r.data),
  pay: (id: number, data: PayOrderRequest) =>
    api.post<Order>(`/orders/${id}/pay`, data).then(r => r.data),
  cancel: (id: number) => api.post<Order>(`/orders/${id}/cancel`).then(r => r.data)
}

export default api
