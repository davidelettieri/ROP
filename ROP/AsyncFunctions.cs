using System;
using System.Threading.Tasks;

namespace ROP;

public static class AsyncFunctions
{
    internal static Func<TInput, Task<Result<TInput, TError>>> ToSwitchTaskFunction<TInput, TError>(
        Func<TInput, Task> a)
    {
        return async t =>
        {
            await a(t);
            return t;
        };
    }

    internal static Func<TInput, Task<Result<TOutput, TError>>> ToSwitchTaskFunction<TInput, TOutput, TError>(
        Func<TInput, Task<TOutput>> a)
    {
        return async t =>
        {
            var r = await a(t);
            return r;
        };
    }

    public static Func<Result<TInput, TError>, Task<Result<TOutput, TError>>> Bind<TInput, TOutput, TError>(
        Func<TInput, Task<Result<TOutput, TError>>> f)
    {
        return r => r.Match(f, e => Task.FromResult(new Result<TOutput, TError>(e)));
    }

    public static async Task<Result<TValue, TError>> Then<TValue, TError>(this Task<Result<TValue, TError>> tr,
        Func<TValue, Task> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction<TValue, TError>(a))(r);
    }

    public static async Task<Result<TOutput, TError>> Then<TInput, TOutput, TError>(
        this Task<Result<TInput, TError>> tr, Func<TInput, Task<TOutput>> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction<TInput, TOutput, TError>(a))(r);
    }

    public static async Task<Result<TOutput, TError>> Then<TInput, TOutput, TError>(
        this Task<Result<TInput, TError>> tr,
        Func<TInput, Task<Result<TOutput, TError>>> f)
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

    internal static Func<TInput, Task<Result<TOutput>>> ToSwitchTaskFunction<TInput, TOutput>(
        Func<TInput, Task<TOutput>> a)
    {
        return async t =>
        {
            var r = await a(t);
            return r;
        };
    }

    public static Func<Result<T>, Task<Result<TSuccess>>> Bind<T, TSuccess>(
        Func<T, Task<Result<TSuccess>>> f)
    {
        return r => r.Match(f, e => Task.FromResult(new Result<TSuccess>(e)));
    }

    public static Func<Result<T>, Result<TSuccess>> Bind<T, TSuccess>(Func<T, Result<TSuccess>> f)
    {
        return r => r.Match(f, e => new Result<TSuccess>(e));
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> tr,
        Func<TInput, Task<TOutput>> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction(a))(r);
    }

    public static async Task<Result<TValue>> Then<TValue>(this Task<Result<TValue>> tr,
        Func<TValue, Task> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction(a))(r);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> tr,
        Func<TInput, Task<Result<TOutput>>> f)
    {
        var r = await tr;
        return await Bind(f)(r);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Result<TInput> r,
        Func<TInput, Task<Result<TOutput>>> f)
    {
        return await Bind(f)(r);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Task<Result<TInput>> tr,
        Func<TInput, Result<TOutput>> f)
    {
        var r = await tr;
        return Bind(f)(r);
    }


    internal static Func<TInput, ValueTask<Result<TInput, TError>>> ToSwitchTaskFunction<TInput, TError>(
        Func<TInput, ValueTask> a)
    {
        return async t =>
        {
            await a(t);
            return t;
        };
    }

    internal static Func<TInput, ValueTask<Result<TOutput, TError>>> ToSwitchTaskFunction<TInput, TOutput, TError>(
        Func<TInput, ValueTask<TOutput>> a)
    {
        return async t =>
        {
            var r = await a(t);
            return r;
        };
    }

    public static Func<Result<TInput, TError>, ValueTask<Result<TOutput, TError>>> Bind<TInput, TOutput, TError>(
        Func<TInput, ValueTask<Result<TOutput, TError>>> f)
    {
        return r => r.Match(f, e => new ValueTask<Result<TOutput, TError>>(new Result<TOutput, TError>(e)));
    }

    public static async ValueTask<Result<TValue, TError>> Then<TValue, TError>(
        this ValueTask<Result<TValue, TError>> tr,
        Func<TValue, Task> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction<TValue, TError>(a))(r);
    }

    public static async ValueTask<Result<TOutput, TError>> Then<TInput, TOutput, TError>(
        this Task<Result<TInput, TError>> tr, Func<TInput, ValueTask<TOutput>> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction<TInput, TOutput, TError>(a))(r);
    }

    public static async Task<Result<TOutput, TError>> Then<TInput, TOutput, TError>(
        this ValueTask<Result<TInput, TError>> tr,
        Func<TInput, ValueTask<Result<TOutput, TError>>> f)
    {
        var r = await tr;
        return await Bind(f)(r);
    }


    internal static Func<T, ValueTask<Result<T>>> ToSwitchTaskFunction<T>(Func<T, ValueTask> a)
    {
        return async t =>
        {
            await a(t);
            return t;
        };
    }

    internal static Func<TInput, ValueTask<Result<TOutput>>> ToSwitchTaskFunction<TInput, TOutput>(
        Func<TInput, ValueTask<TOutput>> a)
    {
        return async t =>
        {
            var r = await a(t);
            return r;
        };
    }

    public static Func<Result<T>, ValueTask<Result<TSuccess>>> Bind<T, TSuccess>(
        Func<T, ValueTask<Result<TSuccess>>> f)
    {
        return r => r.Match(f, e => new ValueTask<Result<TSuccess>>(new Result<TSuccess>(e)));
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this ValueTask<Result<TInput>> tr,
        Func<TInput, ValueTask<TOutput>> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction(a))(r);
    }

    public static async Task<Result<TValue>> Then<TValue>(this ValueTask<Result<TValue>> tr,
        Func<TValue, ValueTask> a)
    {
        var r = await tr;
        return await Bind(ToSwitchTaskFunction(a))(r);
    }

    public static async ValueTask<Result<TOutput>> Then<TInput, TOutput>(this ValueTask<Result<TInput>> tr,
        Func<TInput, ValueTask<Result<TOutput>>> f)
    {
        var r = await tr;
        return await Bind(f)(r);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this Result<TInput> r,
        Func<TInput, ValueTask<Result<TOutput>>> f)
    {
        return await Bind(f)(r);
    }

    public static async Task<Result<TOutput>> Then<TInput, TOutput>(this ValueTask<Result<TInput>> tr,
        Func<TInput, Result<TOutput>> f)
    {
        var r = await tr;
        return Bind(f)(r);
    }
}