using System;

namespace ROP.UnionType
{
    public class Union<T0, T1> : IUnion<T0, T1>
    {
        private readonly T0 _t0;
        private readonly T1 _t1;
        private readonly UnionType _type;

        public Union(T0 value)
        {
            _t0 = value;
            _type = UnionType.T0;
        }

        public Union(T1 value)
        {
            _t1 = value;
            _type = UnionType.T1;
        }

        public void Switch(Action<T0> actionT0, Action<T1> actionT1)
        {
            if (actionT0 is null)
                throw new ArgumentNullException(nameof(actionT0));
            if (actionT1 is null)
                throw new ArgumentNullException(nameof(actionT1));

            switch (_type)
            {
                case UnionType.T0:
                    actionT0(_t0);
                    break;
                case UnionType.T1:
                    actionT1(_t1);
                    break;
            }
        }

        public TResult Match<TResult>(Func<T0, TResult> funcT0, Func<T1, TResult> functT1)
        {
            if (funcT0 is null)
                throw new ArgumentNullException(nameof(funcT0));
            if (functT1 is null)
                throw new ArgumentNullException(nameof(functT1));

            switch (_type)
            {
                case UnionType.T0:
                    return funcT0(_t0);
                case UnionType.T1:
                    return functT1(_t1);
            }

            throw new InvalidOperationException();
        }

        enum UnionType
        {
            T0,
            T1
        }

        public static implicit operator Union<T0, T1>(T0 t0) => new Union<T0, T1>(t0);
        public static implicit operator Union<T0, T1>(T1 t1) => new Union<T0, T1>(t1);
    }
}
