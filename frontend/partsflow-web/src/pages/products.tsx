import Head from 'next/head';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { FormEvent, useEffect, useMemo, useState } from 'react';
import { AuthUser, clearAuth, getStoredAuth, Product, ProductInput, productApi } from '../lib/api';

type ProductFormState = {
  sku: string;
  name: string;
  brand: string;
  category: string;
  description: string;
  quantity: string;
  minimumStockLevel: string;
  costPrice: string;
  sellingPrice: string;
};

const emptyForm: ProductFormState = {
  sku: '',
  name: '',
  brand: '',
  category: '',
  description: '',
  quantity: '0',
  minimumStockLevel: '0',
  costPrice: '0',
  sellingPrice: '0',
};

export default function ProductsPage() {
  const router = useRouter();
  const [user, setUser] = useState<AuthUser | null>(null);
  const [products, setProducts] = useState<Product[]>([]);
  const [form, setForm] = useState<ProductFormState>(emptyForm);
  const [editingProductId, setEditingProductId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  const lowStockProducts = useMemo(
    () => products.filter((product) => product.quantity <= product.minimumStockLevel).length,
    [products]
  );

  async function loadProducts() {
    setLoading(true);
    setError('');

    try {
      setProducts(await productApi.getAll());
    } catch (apiError) {
      setError((apiError as Error).message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    const auth = getStoredAuth();

    if (!auth) {
      router.replace('/login');
      return;
    }

    setUser(auth.user);
    loadProducts();
  }, [router]);

  function logout() {
    clearAuth();
    router.push('/login');
  }

  function updateField(field: keyof ProductFormState, value: string) {
    setForm((currentForm) => ({ ...currentForm, [field]: value }));
  }

  function startEdit(product: Product) {
    setEditingProductId(product.id);
    setForm({
      sku: product.sku,
      name: product.name,
      brand: product.brand,
      category: product.category,
      description: product.description || '',
      quantity: String(product.quantity),
      minimumStockLevel: String(product.minimumStockLevel),
      costPrice: String(product.costPrice),
      sellingPrice: String(product.sellingPrice),
    });
  }

  function resetForm() {
    setEditingProductId(null);
    setForm(emptyForm);
  }

  function toProductInput(): ProductInput {
    return {
      sku: form.sku,
      name: form.name,
      brand: form.brand,
      category: form.category,
      description: form.description,
      quantity: Number(form.quantity),
      minimumStockLevel: Number(form.minimumStockLevel),
      costPrice: Number(form.costPrice),
      sellingPrice: Number(form.sellingPrice),
    };
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setSaving(true);
    setError('');

    try {
      const payload = toProductInput();

      if (editingProductId) {
        await productApi.update(editingProductId, payload);
      } else {
        await productApi.create(payload);
      }

      resetForm();
      await loadProducts();
    } catch (apiError) {
      setError((apiError as Error).message);
    } finally {
      setSaving(false);
    }
  }

  async function deleteProduct(product: Product) {
    if (!window.confirm(`Delete ${product.name}?`)) {
      return;
    }

    setError('');

    try {
      await productApi.delete(product.id);
      await loadProducts();
    } catch (apiError) {
      setError((apiError as Error).message);
    }
  }

  return (
    <>
      <Head>
        <title>Products | PartsFlow ERP</title>
      </Head>

      <main className="min-h-screen bg-slate-100 px-6 py-8 text-slate-900">
        <div className="mx-auto max-w-7xl">
          <header className="mb-8 rounded-2xl border border-slate-200 bg-white px-6 py-5 shadow-sm sm:flex sm:items-center sm:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.2em] text-blue-600">
                Inventory
              </p>
              <h1 className="mt-2 text-3xl font-bold text-slate-950">Products</h1>
              <p className="mt-2 text-sm text-slate-500">
                {products.length} products · {lowStockProducts} low stock
                {user ? ` · Logged in as ${user.fullName}` : ''}
              </p>
            </div>
            <nav className="mt-4 flex flex-wrap gap-2 text-sm font-medium sm:mt-0">
              <Link href="/">
                <a className="rounded-lg px-4 py-2 text-slate-600 hover:bg-slate-100 hover:text-slate-950">
                  Dashboard
                </a>
              </Link>
              <Link href="/products">
                <a className="rounded-lg bg-blue-50 px-4 py-2 text-blue-700">Products</a>
              </Link>
              <button
                className="rounded-lg px-4 py-2 text-slate-600 hover:bg-slate-100 hover:text-slate-950"
                type="button"
                onClick={logout}
              >
                Logout
              </button>
            </nav>
          </header>

          {error && (
            <div className="mb-6 rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-700">
              {error}
            </div>
          )}

          <section className="mb-8 rounded-2xl border border-slate-200 bg-white p-5 shadow-sm">
            <h2 className="mb-4 text-lg font-semibold text-slate-950">
              {editingProductId ? 'Edit Product' : 'Create Product'}
            </h2>
            <form className="grid gap-4 md:grid-cols-4" onSubmit={handleSubmit}>
              <TextInput label="SKU" value={form.sku} onChange={(value) => updateField('sku', value)} />
              <TextInput label="Name" value={form.name} onChange={(value) => updateField('name', value)} />
              <TextInput label="Brand" value={form.brand} onChange={(value) => updateField('brand', value)} />
              <TextInput
                label="Category"
                value={form.category}
                onChange={(value) => updateField('category', value)}
              />
              <TextInput
                label="Quantity"
                type="number"
                value={form.quantity}
                onChange={(value) => updateField('quantity', value)}
              />
              <TextInput
                label="Minimum Stock"
                type="number"
                value={form.minimumStockLevel}
                onChange={(value) => updateField('minimumStockLevel', value)}
              />
              <TextInput
                label="Cost Price"
                type="number"
                value={form.costPrice}
                onChange={(value) => updateField('costPrice', value)}
              />
              <TextInput
                label="Selling Price"
                type="number"
                value={form.sellingPrice}
                onChange={(value) => updateField('sellingPrice', value)}
              />
              <label className="md:col-span-4">
                <span className="mb-1 block text-sm font-medium text-slate-700">Description</span>
                <textarea
                  className="min-h-[80px] w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                  value={form.description}
                  onChange={(event) => updateField('description', event.target.value)}
                />
              </label>
              <div className="flex gap-3 md:col-span-4">
                <button
                  className="rounded-lg bg-blue-600 px-5 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:opacity-60"
                  disabled={saving}
                  type="submit"
                >
                  {saving ? 'Saving...' : editingProductId ? 'Update Product' : 'Create Product'}
                </button>
                {editingProductId && (
                  <button
                    className="rounded-lg border border-slate-300 px-5 py-2 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
                    type="button"
                    onClick={resetForm}
                  >
                    Cancel
                  </button>
                )}
              </div>
            </form>
          </section>

          <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-slate-200 text-sm">
                <thead className="bg-slate-50 text-left text-xs uppercase tracking-wide text-slate-500">
                  <tr>
                    <th className="px-4 py-3">SKU</th>
                    <th className="px-4 py-3">Name</th>
                    <th className="px-4 py-3">Brand</th>
                    <th className="px-4 py-3">Category</th>
                    <th className="px-4 py-3">Quantity</th>
                    <th className="px-4 py-3">Minimum Stock</th>
                    <th className="px-4 py-3">Selling Price</th>
                    <th className="px-4 py-3">Status</th>
                    <th className="px-4 py-3">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-200">
                  {loading ? (
                    <tr>
                      <td className="px-4 py-6 text-slate-500" colSpan={9}>
                        Loading products...
                      </td>
                    </tr>
                  ) : products.length === 0 ? (
                    <tr>
                      <td className="px-4 py-6 text-slate-500" colSpan={9}>
                        No products found.
                      </td>
                    </tr>
                  ) : (
                    products.map((product) => {
                      const isLowStock = product.quantity <= product.minimumStockLevel;

                      return (
                        <tr key={product.id} className="text-slate-700 hover:bg-slate-50">
                          <td className="px-4 py-3 font-mono text-xs text-slate-600">{product.sku}</td>
                          <td className="px-4 py-3 font-medium text-slate-950">{product.name}</td>
                          <td className="px-4 py-3">{product.brand}</td>
                          <td className="px-4 py-3">{product.category}</td>
                          <td className="px-4 py-3">{product.quantity}</td>
                          <td className="px-4 py-3">{product.minimumStockLevel}</td>
                          <td className="px-4 py-3">RM {product.sellingPrice.toFixed(2)}</td>
                          <td className="px-4 py-3">
                            <span
                              className={`rounded-full px-3 py-1 text-xs font-semibold ${
                                isLowStock
                                  ? 'bg-amber-100 text-amber-700'
                                  : 'bg-emerald-100 text-emerald-700'
                              }`}
                            >
                              {isLowStock ? 'Low Stock' : 'In Stock'}
                            </span>
                          </td>
                          <td className="px-4 py-3">
                            <div className="flex gap-3">
                              <button
                                className="font-medium text-blue-600 hover:text-blue-800"
                                type="button"
                                onClick={() => startEdit(product)}
                              >
                                Edit
                              </button>
                              <button
                                className="font-medium text-red-600 hover:text-red-800"
                                type="button"
                                onClick={() => deleteProduct(product)}
                              >
                                Delete
                              </button>
                            </div>
                          </td>
                        </tr>
                      );
                    })
                  )}
                </tbody>
              </table>
            </div>
          </section>
        </div>
      </main>
    </>
  );
}

function TextInput({
  label,
  value,
  onChange,
  type = 'text',
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  type?: string;
}) {
  return (
    <label>
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        min={type === 'number' ? '0' : undefined}
        step={type === 'number' ? '0.01' : undefined}
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
      />
    </label>
  );
}
