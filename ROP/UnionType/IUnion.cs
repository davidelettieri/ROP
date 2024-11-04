using System;
using System.Collections.Generic;
using System.Text;

namespace ROP.UnionType;

public interface IUnion<out T0, out T1>
{
    TResult Match<TResult>(Func<T0, TResult> funcT0, Func<T1, TResult> funcT1);
}