export interface BakeryItem {
  id: number
  name: string
  description: string
  price: number
  category: string
  isAvailable: boolean
  stockQuantity: number
}

export interface OrderItem {
  bakeryItemId: number
  itemName: string
  quantity: number
  price: number
}

export interface Order {
  id?: number
  customerName: string
  orderDate?: string
  items: OrderItem[]
  totalAmount?: number
  status?: string
}
