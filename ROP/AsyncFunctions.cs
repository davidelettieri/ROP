﻿using ROP.UnionType;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class AsyncFunctions
    {
        internal static Func<T, Task<Result<T, TError>>> ToSwitchTaskFunction<T, TError>(Func<T, Task> a)
        {
            return async t =>
            {
                await a(t);
                return t;
            };
        }
        internal static Func<T, Task<Result<S, TError>>> ToSwitchTaskFunction<T, S, TError>(Func<T, Task<S>> a)
        {
            return async t =>
            {
                var r = await a(t);
                return r;
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
        public async static Task<Result<SValue, TError>> Then<TValue, SValue, TError>(this Task<Result<TValue, TError>> tr, Func<TValue, Task<SValue>> a)
        {
            var r = await tr;
            return await Bind(ToSwitchTaskFunction<TValue, SValue, TError>(a))(r);
        }
        public async static Task<Result<S, TError>> Then<S, TValue, TError>(this Task<Result<TValue, TError>> tr, Func<TValue, Task<Result<S, TError>>> f)
        {
            var r = await tr;
            return await Bind(f)(r);
        }


        internal static Func<T, Task<Result<T>>> ToSwitchTaskFunction<T>(Func<T, Task> a)
        {
            return async t =>
            {
                await a(t);
                return t;
            };
        }
        internal static Func<T, Task<Result<S>>> ToSwitchTaskFunction<T, S>(Func<T, Task<S>> a)
        {
            return async t =>
            {
                var r = await a(t);
                return r;
            };
        }
        public static Func<Result<T>, Task<Result<TSuccess>>> Bind<T, TSuccess>(Func<T, Task<Result<TSuccess>>> f)
        {
            return r => r.Match(t => f(t), e => Task.FromResult(new Result<TSuccess>(e)));
        }
        public static Func<Result<T>, Result<TSuccess>> Bind<T, TSuccess>(Func<T, Result<TSuccess>> f)
        {
            return r => r.Match(t => f(t), e => new Result<TSuccess>(e));
        }
        public async static Task<Result<SValue>> Then<TValue, SValue>(this Task<Result<TValue>> tr, Func<TValue, Task<SValue>> a)
        {
            var r = await tr;
            return await Bind(ToSwitchTaskFunction(a))(r);
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
        public async static Task<Result<S>> Then<S, TValue>(this Result<TValue> r, Func<TValue, Task<Result<S>>> f)
        {
            return await Bind(f)(r);
        }
        public async static Task<Result<S>> Then<S, TValue>(this Task<Result<TValue>> tr, Func<TValue, Result<S>> f)
        {
            var r = await tr;
            return Bind(f)(r);
        }

    }


}
