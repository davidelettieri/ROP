using System;
using System.Collections.Generic;
using System.Text;

namespace ROP.UnionType
{
    public interface IUnion<T0, T1>
    {
        TResult Match<TResult>(Func<T0, TResult> ft0, Func<T1, TResult> ft1);
    }
}
