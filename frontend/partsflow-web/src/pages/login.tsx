import Head from 'next/head';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { FormEvent, useEffect, useState } from 'react';
import { ALLOW_REGISTRATION, authApi, getStoredAuth, saveAuth } from '../lib/api';

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (getStoredAuth()) {
      router.replace('/');
    }
  }, [router]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError('');
    setLoading(true);

    try {
      const auth = await authApi.login({ email, password });
      saveAuth(auth);
      router.push('/');
    } catch (apiError) {
      setError((apiError as Error).message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <Head>
        <title>Login | PartsFlow ERP</title>
      </Head>

      <main className="flex min-h-screen items-center justify-center bg-slate-100 px-6 py-10 text-slate-900">
        <section className="w-full max-w-md rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-blue-600">
            PartsFlow ERP
          </p>
          <h1 className="mt-3 text-3xl font-bold text-slate-950">Login</h1>
          <p className="mt-2 text-sm text-slate-500">
            Sign in to manage inventory products.
          </p>

          {error && (
            <div className="mt-5 rounded-lg border border-red-200 bg-red-50 p-3 text-sm text-red-700">
              {error}
            </div>
          )}

          <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
            <label className="block">
              <span className="mb-1 block text-sm font-medium text-slate-700">Email</span>
              <input
                className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                required
              />
            </label>

            <label className="block">
              <span className="mb-1 block text-sm font-medium text-slate-700">Password</span>
              <input
                className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                required
              />
            </label>

            <button
              className="w-full rounded-lg bg-blue-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:opacity-60"
              disabled={loading}
              type="submit"
            >
              {loading ? 'Logging in...' : 'Login'}
            </button>
          </form>

          {ALLOW_REGISTRATION ? (
            <p className="mt-5 text-center text-sm text-slate-500">
              No account yet?{' '}
              <Link href="/register">
                <a className="font-semibold text-blue-600 hover:text-blue-700">Register</a>
              </Link>
            </p>
          ) : (
            <p className="mt-5 rounded-lg bg-slate-50 p-3 text-center text-sm text-slate-500">
              Public registration is disabled. Use the demo account provided by the owner.
            </p>
          )}
        </section>
      </main>
    </>
  );
}
