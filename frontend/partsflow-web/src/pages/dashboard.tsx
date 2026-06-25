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
      </Head>

      <main className="min-h-screen bg-slate-950 px-6 py-8 text-slate-100">
        <div className="mx-auto max-w-6xl">
          <header className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <p className="text-sm font-medium uppercase tracking-[0.2em] text-cyan-400">
                Dashboard
              </p>
              <h1 className="mt-2 text-3xl font-bold text-white">PartsFlow ERP</h1>
            </div>
            <nav className="flex gap-4 text-sm text-slate-300">
              <Link href="/">
                <a className="hover:text-white">Home</a>
              </Link>
              <Link href="/products">
                <a className="hover:text-white">Products</a>
              </Link>
            </nav>
          </header>

          {error && (
            <div className="mb-6 rounded-lg border border-red-500/40 bg-red-500/10 p-4 text-sm text-red-200">
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
    <div className="rounded-xl border border-slate-800 bg-slate-900 p-6 shadow-lg">
      <p className="text-sm text-slate-400">{label}</p>
      <p className="mt-3 text-3xl font-bold text-white">{value}</p>
    </div>
  );
}
