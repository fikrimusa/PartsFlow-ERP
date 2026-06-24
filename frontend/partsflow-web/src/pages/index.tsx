import Head from 'next/head';

const navigationItems = [
  'Dashboard',
  'Products',
  'Stock Movements',
  'Sales Orders',
  'Login',
];

export default function Home() {
  return (
    <>
      <Head>
        <title>PartsFlow ERP</title>
        <meta name="description" content="Auto parts inventory and sales management" />
      </Head>

      <main className="min-h-screen bg-slate-950 px-6 py-10 text-slate-100">
        <div className="mx-auto max-w-5xl">
          <header className="flex flex-col gap-6 border-b border-slate-800 pb-8 sm:flex-row sm:items-center sm:justify-between">
            <h1 className="text-3xl font-bold tracking-tight text-white">PartsFlow ERP</h1>
            <nav aria-label="Primary navigation">
              <ul className="flex flex-wrap gap-x-5 gap-y-2 text-sm text-slate-300">
                {navigationItems.map((item) => (
                  <li key={item}>
                    <a className="transition hover:text-white" href="#">
                      {item}
                    </a>
                  </li>
                ))}
              </ul>
            </nav>
          </header>

          <section className="py-20">
            <p className="text-sm font-medium uppercase tracking-[0.2em] text-cyan-400">Project foundation</p>
            <h2 className="mt-4 max-w-2xl text-4xl font-semibold tracking-tight text-white sm:text-5xl">
              Auto parts inventory and sales management, built for growing distributors.
            </h2>
          </section>
        </div>
      </main>
    </>
  );
}
