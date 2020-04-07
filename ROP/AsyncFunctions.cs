using ROP.UnionType;
using System;
using System.Runtime.CompilerServices;
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

        public static Func<T, Task<Result<T>>> ToSwitchTaskFunction<T>(Func<T, Task> a)
        {
            return async t =>
            {
                await a(t);
                return t;
            };
        }

        public static Func<Result<T>, Task<Result<TSuccess>>> Bind<T, TSuccess>(Func<T, Task<Result<TSuccess>>> f)
        {
            return r => r.Match(t => f(t), e => Task.FromResult(new Result<TSuccess>(e)));
        }

        public async static Task<Result<TValue>> Then<TValue>(this Task<Result<TValue>> tr, Func<TValue, Task> a)
        {
            var r = await tr;
            return await Bind(ToSwitchTaskFunction(a))(r);
        }

        public async static Task<Result<S>> Then<S, TValue>(this Task<Result<TValue>> tr, Func<TValue, Task<Result<S>>> f)
        {
            var r = await tr;
            return await Bind(f)(r);
        }

        //internal static Func<T, ValueTask<Result<T, TError>>> ToSwitchTaskFunction<T, TError>(Func<T, ValueTask> a)
        //{
        //    return async t =>
        //    {
        //        await a(t);
        //        return t;
        //    };
        //}
        //internal static Func<IUnion<T, TError>, ValueTask<Result<TSuccess, TError>>> Bind<T, TSuccess, TError>(Func<T, ValueTask<Result<TSuccess, TError>>> f)
        //{
        //    return r => r.Match(t => f(t), e => new ValueTask<Result<TSuccess, TError>>((new Result<TSuccess, TError>(e))));
        //}

        //public async static ValueTask<IUnion<TValue, TError>> Then<TUnion, TValue, TError>(this ValueTask<TUnion> tr, Func<TValue, ValueTask> a)
        //    where TUnion : IUnion<TValue, TError>
        //{
        //    var r = await tr;
        //    return await Bind(ToSwitchTaskFunction<TValue, TError>(a))(r);
        //}

        //public async static ValueTask<IUnion<S, TError>> Then<TUnion, S, TValue, TError>(this ValueTask<TUnion> tr, Func<TValue, ValueTask<Result<S, TError>>> f)
        //    where TUnion : IUnion<TValue, TError>
        //{
        //    var r = await tr;
        //    return await Bind(f)(r);
        //}

    }

    
}
