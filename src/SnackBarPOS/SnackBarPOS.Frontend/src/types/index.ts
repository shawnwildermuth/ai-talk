export interface Category {
  id: number
  name: string
  description?: string
  iconEmoji?: string
  sortOrder: number
  isActive: boolean
  productCount: number
}

export interface Product {
  id: number
  name: string
  description?: string
  price: number
  categoryId: number
  categoryName: string
  categoryEmoji?: string
  imageUrl?: string
  isActive: boolean
  isAvailable: boolean
}

export interface OrderItem {
  id: number
  productId: number
  productName: string
  unitPrice: number
  quantity: number
  lineTotal: number
}

export type OrderStatus = 'Open' | 'Paid' | 'Cancelled'
export type PaymentMethod = 'Cash' | 'Pin' | 'Contactless'

export interface Order {
  id: number
  orderNumber: string
  status: OrderStatus
  totalAmount: number
  paymentMethod?: PaymentMethod
  amountTendered?: number
  change?: number
  notes?: string
  createdAt: string
  paidAt?: string
  cancelledAt?: string
  items: OrderItem[]
}

export interface DailySummary {
  totalOrders: number
  totalRevenue: number
  totalItems: number
  revenueByCategory: Record<string, number>
}

// Requests
export interface CreateCategoryRequest {
  name: string
  description?: string
  iconEmoji?: string
  sortOrder: number
}

export interface UpdateCategoryRequest extends CreateCategoryRequest {
  isActive: boolean
}

export interface CreateProductRequest {
  name: string
  description?: string
  price: number
  categoryId: number
  imageUrl?: string
}

export interface UpdateProductRequest extends CreateProductRequest {
  isActive: boolean
  isAvailable: boolean
}

export interface AddOrderItemRequest {
  productId: number
  quantity: number
}

export interface PayOrderRequest {
  paymentMethod: PaymentMethod
  amountTendered?: number
  notes?: string
}
