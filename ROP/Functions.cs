using System;
using System.Collections.Generic;
using System.Text;
using ROP.UnionType;

namespace ROP;

public static class Functions
{
    internal static Func<IUnion<TInput, TError>, Result<TOutput, TError>> Bind<TInput, TOutput, TError>(Func<TInput, Result<TOutput, TError>> f)
    {
        return r => r.Match(f, e => new Result<TOutput, TError>(e));
    }
    internal static Func<TInput, Result<TOutput, TError>> ToSwitchFunction<TInput, TOutput, TError>(Func<TInput, TOutput> a) => t => a(t);
    internal static Func<TInput, Result<TInput, TError>> ToSwitchFunction<TInput, TError>(Action<TInput> a)
    {
        return t =>
        {
            a(t);
            return t;
        };
    }
    public static Result<TOutput, TError> Then<TInput, TOutput, TError>(this IUnion<TInput, TError> r, Func<TInput, TOutput> a)
        => Bind(ToSwitchFunction<TInput, TOutput, TError>(a))(r);
    public static Result<TInput, TError> Then<TInput, TError>(this IUnion<TInput, TError> r, Action<TInput> a)
        => Bind(ToSwitchFunction<TInput, TError>(a))(r);
    public static Result<TOutput, TError> Then<TOutput, TInput, TError>(this IUnion<TInput, TError> r, Func<TInput, Result<TOutput, TError>> f)
        => Bind(f)(r);
}