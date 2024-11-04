using System;
using System.Diagnostics.CodeAnalysis;

namespace ROP.UnionType;

public class Union<T0, T1> : IUnion<T0, T1>
{
    private readonly T0? _t0;
    private readonly T1? _t1;
    private readonly UnionType _type;

    public Union([NotNull] T0 value)
    {
        _t0 = value ?? throw new ArgumentNullException(nameof(value));
        _type = UnionType.T0;
    }

    public Union([NotNull] T1 value)
    {
        _t1 = value ?? throw new ArgumentNullException(nameof(value));
        _type = UnionType.T1;
    }

    public void Switch([NotNull] Action<T0> actionT0, [NotNull] Action<T1> actionT1)
    {
        if (actionT0 is null)
            throw new ArgumentNullException(nameof(actionT0));
        if (actionT1 is null)
            throw new ArgumentNullException(nameof(actionT1));

        switch (_type)
        {
            case UnionType.T0 when _t0 != null:
                actionT0(_t0);
                break;
            case UnionType.T1 when _t1 != null:
                actionT1(_t1);
                break;
        }
    }

    public TResult Match<TResult>([NotNull] Func<T0, TResult> funcT0, [NotNull] Func<T1, TResult> funcT1)
    {
        if (funcT0 is null)
            throw new ArgumentNullException(nameof(funcT0));
        if (funcT1 is null)
            throw new ArgumentNullException(nameof(funcT1));

        return _type switch
        {
            UnionType.T0 when _t0 != null => funcT0(_t0),
            UnionType.T1 when _t1 != null => funcT1(_t1),
            _ => throw new InvalidOperationException()
        };
    }

    enum UnionType
    {
        T0,
        T1
    }

    public static implicit operator Union<T0, T1>([NotNull] T0 t0) => new(t0);
    public static implicit operator Union<T0, T1>([NotNull] T1 t1) => new(t1);
}