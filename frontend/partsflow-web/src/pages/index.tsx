import Head from 'next/head';
import Link from 'next/link';

const navigationItems = [
  { label: 'Dashboard', href: '/dashboard' },
  { label: 'Products', href: '/products' },
];

export default function Home() {
  return (
    <>
      <Head>
        <title>PartsFlow ERP</title>
        <meta name="description" content="Auto parts inventory management" />
      </Head>

      <main className="min-h-screen bg-slate-950 px-6 py-10 text-slate-100">
        <div className="mx-auto max-w-5xl">
          <header className="flex flex-col gap-6 border-b border-slate-800 pb-8 sm:flex-row sm:items-center sm:justify-between">
            <h1 className="text-3xl font-bold tracking-tight text-white">PartsFlow ERP</h1>
            <nav aria-label="Primary navigation">
              <ul className="flex flex-wrap gap-x-5 gap-y-2 text-sm text-slate-300">
                {navigationItems.map((item) => (
                  <li key={item.href}>
                    <Link href={item.href}>
                      <a className="transition hover:text-white">{item.label}</a>
                    </Link>
                  </li>
                ))}
              </ul>
            </nav>
          </header>

          <section className="py-20">
            <p className="text-sm font-medium uppercase tracking-[0.2em] text-cyan-400">
              Product CRUD MVP
            </p>
            <h2 className="mt-4 max-w-2xl text-4xl font-semibold tracking-tight text-white sm:text-5xl">
              Auto parts inventory management, built as a clean full-stack portfolio project.
            </h2>
            <div className="mt-8 flex flex-wrap gap-3">
              <Link href="/dashboard">
                <a className="rounded-lg bg-cyan-400 px-5 py-3 text-sm font-semibold text-slate-950 transition hover:bg-cyan-300">
                  View Dashboard
                </a>
              </Link>
              <Link href="/products">
                <a className="rounded-lg border border-slate-700 px-5 py-3 text-sm font-semibold text-white transition hover:border-cyan-400">
                  Manage Products
                </a>
              </Link>
            </div>
          </section>
        </div>
      </main>
    </>
  );
}
