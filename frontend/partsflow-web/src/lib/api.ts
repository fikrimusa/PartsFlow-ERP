export const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:5000';

export type Product = {
  id: number;
  sku: string;
  name: string;
  brand: string;
  category: string;
  description?: string | null;
  quantity: number;
  minimumStockLevel: number;
  costPrice: number;
  sellingPrice: number;
  createdAt: string;
  updatedAt: string;
};

export type ProductInput = {
  sku: string;
  name: string;
  brand: string;
  category: string;
  description?: string;
  quantity: number;
  minimumStockLevel: number;
  costPrice: number;
  sellingPrice: number;
};

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
    ...options,
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Request failed with status ${response.status}`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export const productApi = {
  getAll: () => request<Product[]>('/api/products'),
  getLowStock: () => request<Product[]>('/api/products/low-stock'),
  create: (product: ProductInput) =>
    request<Product>('/api/products', {
      method: 'POST',
      body: JSON.stringify(product),
    }),
  update: (id: number, product: ProductInput) =>
    request<void>(`/api/products/${id}`, {
      method: 'PUT',
      body: JSON.stringify(product),
    }),
  delete: (id: number) =>
    request<void>(`/api/products/${id}`, {
      method: 'DELETE',
    }),
};
