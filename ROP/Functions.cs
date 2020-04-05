using System;
using System.Collections.Generic;
using System.Text;

namespace ROP
{
    public static class Functions
    {
        public static Func<Result<T, TError>, Result<TSuccess, TError>> Bind<T, TSuccess, TError>(Func<T, Result<TSuccess, TError>> f)
        {
            return r => r.Match(t => f(t), e => new Result<TSuccess, TError>(e));
        }

        public static Func<T, Result<S, TError>> ToSwitchFunction<T, S, TError>(Func<T, S> a) => (t => a(t));
        public static Func<T, Result<T, TError>> ToSwitchFunction<T, TError>(Action<T> a)
        {
            return t =>
            {
                a(t);
                return t;
            };
        }
        public static Result<SValue, TError> Then<TValue, SValue, TError>(this Result<TValue, TError> r, Func<TValue, SValue> a)
            => Bind(ToSwitchFunction<TValue, SValue, TError>(a))(r);
        public static Result<TValue, TError> Then<TValue, TError>(this Result<TValue, TError> r, Action<TValue> a)
            => Bind(ToSwitchFunction<TValue, TError>(a))(r);
        public static Result<S, TError> Then<S, TValue, TError>(this Result<TValue, TError> r, Func<TValue, Result<S, TError>> f)
            => Bind(f)(r);

    }
}
