using System;
using System.Threading.Tasks;

namespace ROP
{
    public static class AsyncFunctions
    {
        public static Func<T, Task<Result<T, TError>>> ToSwitchTaskFunction<T, TError>(Func<T, Task> a)
        {
            return async t =>
            {
                await a(t);
                return t;
            };
        }

        public static Func<Result<T, TError>, Task<Result<TSuccess, TError>>> Bind<T, TSuccess, TError>(Func<T, Task<Result<TSuccess, TError>>> f)
        {
            return r => r.Match(t => f(t), e => Task.FromResult(new Result<TSuccess, TError>(e)));
        }

        public async static Task<Result<TValue, TError>> Then<TValue, TError>(this Task<Result<TValue, TError>> tr, Func<TValue, Task> a)
        {
            var r = await tr;
            return await Bind(ToSwitchTaskFunction<TValue, TError>(a))(r);
        }

        public async static Task<Result<S, TError>> Then<S, TValue, TError>(this Task<Result<TValue, TError>> tr, Func<TValue, Task<Result<S, TError>>> f)
        {
            var r = await tr;
            return await Bind(f)(r);
        }

    }
}
