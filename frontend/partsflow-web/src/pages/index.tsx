import Head from 'next/head';
import Link from 'next/link';
import { useEffect, useMemo, useState } from 'react';
import { Product, productApi } from '../lib/api';

export default function DashboardPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    productApi
      .getAll()
      .then(setProducts)
      .catch((apiError: Error) => setError(apiError.message))
      .finally(() => setLoading(false));
  }, []);

  const summary = useMemo(() => {
    return {
      totalProducts: products.length,
      lowStockProducts: products.filter(
        (product) => product.quantity <= product.minimumStockLevel
      ).length,
      totalInventoryQuantity: products.reduce(
        (total, product) => total + product.quantity,
        0
      ),
    };
  }, [products]);

  return (
    <>
      <Head>
        <title>Dashboard | PartsFlow ERP</title>
        <meta name="description" content="PartsFlow ERP inventory dashboard" />
      </Head>

      <main className="min-h-screen bg-slate-100 px-6 py-8 text-slate-900">
        <div className="mx-auto max-w-6xl">
          <header className="mb-8 rounded-2xl border border-slate-200 bg-white px-6 py-5 shadow-sm sm:flex sm:items-center sm:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.2em] text-blue-600">
                Dashboard
              </p>
              <h1 className="mt-2 text-3xl font-bold text-slate-950">PartsFlow ERP</h1>
              <p className="mt-2 text-sm text-slate-500">
                Simple auto parts inventory management.
              </p>
            </div>
            <nav className="mt-4 flex gap-2 text-sm font-medium sm:mt-0">
              <Link href="/">
                <a className="rounded-lg bg-blue-50 px-4 py-2 text-blue-700">Dashboard</a>
              </Link>
              <Link href="/products">
                <a className="rounded-lg px-4 py-2 text-slate-600 hover:bg-slate-100 hover:text-slate-950">
                  Products
                </a>
              </Link>
            </nav>
          </header>

          {error && (
            <div className="mb-6 rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-700">
              {error}
            </div>
          )}

          <section className="grid gap-4 md:grid-cols-3">
            <SummaryCard label="Total Products" value={loading ? '...' : summary.totalProducts} />
            <SummaryCard
              label="Low Stock Products"
              value={loading ? '...' : summary.lowStockProducts}
            />
            <SummaryCard
              label="Total Inventory Quantity"
              value={loading ? '...' : summary.totalInventoryQuantity}
            />
          </section>
        </div>
      </main>
    </>
  );
}

function SummaryCard({ label, value }: { label: string; value: number | string }) {
  return (
    <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
      <p className="text-sm font-medium text-slate-500">{label}</p>
      <p className="mt-3 text-3xl font-bold text-slate-950">{value}</p>
    </div>
  );
}
