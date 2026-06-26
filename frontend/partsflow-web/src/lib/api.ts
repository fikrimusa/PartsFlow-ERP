export const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:5000';

export const ALLOW_REGISTRATION =
  process.env.NEXT_PUBLIC_ALLOW_REGISTRATION === 'true';

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

export type AuthUser = {
  id: number;
  fullName: string;
  email: string;
};

export type AuthResponse = {
  token: string;
  expiresAt: string;
  user: AuthUser;
};

export type RegisterInput = {
  fullName: string;
  email: string;
  password: string;
};

export type LoginInput = {
  email: string;
  password: string;
};

const AUTH_STORAGE_KEY = 'partsflow_auth';

export function getStoredAuth(): AuthResponse | null {
  if (typeof window === 'undefined') {
    return null;
  }

  const value = window.localStorage.getItem(AUTH_STORAGE_KEY);

  if (!value) {
    return null;
  }

  try {
    const auth = JSON.parse(value) as AuthResponse;

    if (new Date(auth.expiresAt).getTime() <= Date.now()) {
      window.localStorage.removeItem(AUTH_STORAGE_KEY);
      return null;
    }

    return auth;
  } catch {
    window.localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

export function saveAuth(auth: AuthResponse) {
  window.localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(auth));
}

export function clearAuth() {
  window.localStorage.removeItem(AUTH_STORAGE_KEY);
}

function getAuthToken() {
  return getStoredAuth()?.token;
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const token = getAuthToken();

  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options?.headers,
    },
    ...options,
  });

  if (!response.ok) {
    const errorText = await response.text();
    let message = errorText || `Request failed with status ${response.status}`;

    try {
      const parsed = JSON.parse(errorText) as { message?: string; title?: string };
      message = parsed.message || parsed.title || message;
    } catch {
      // Keep raw error text.
    }

    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export const authApi = {
  register: (input: RegisterInput) =>
    request<AuthResponse>('/api/auth/register', {
      method: 'POST',
      body: JSON.stringify(input),
    }),
  login: (input: LoginInput) =>
    request<AuthResponse>('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify(input),
    }),
};

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
