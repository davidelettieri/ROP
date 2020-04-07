using System;

namespace ROP.UnionType
{
    public class Union<TOne, TOther> : IUnion<TOne, TOther>
    {
        private readonly TOne _toneValue;
        private readonly TOther _totherValue;
        private readonly UnionType _type;

        public Union(TOne value)
        {
            _toneValue = value;
            _type = UnionType.TOne;
        }

        public Union(TOther value)
        {
            _totherValue = value;
            _type = UnionType.TOther;
        }

        public void Switch(Action<TOne> actionTOne, Action<TOther> actionTOther)
        {
            if (actionTOne is null)
                throw new ArgumentNullException(nameof(actionTOne));
            if (actionTOther is null)
                throw new ArgumentNullException(nameof(actionTOther));

            switch (_type)
            {
                case UnionType.TOne:
                    actionTOne(_toneValue);
                    break;
                case UnionType.TOther:
                    actionTOther(_totherValue);
                    break;
            }
        }

        public TResult Match<TResult>(Func<TOne, TResult> funcTOne, Func<TOther, TResult> funcTOther)
        {
            if (funcTOne is null)
                throw new ArgumentNullException(nameof(funcTOne));
            if (funcTOther is null)
                throw new ArgumentNullException(nameof(funcTOther));

            switch (_type)
            {
                case UnionType.TOne:
                    return funcTOne(_toneValue);
                case UnionType.TOther:
                    return funcTOther(_totherValue);
            }

            throw new InvalidOperationException();
        }

        enum UnionType
        {
            TOne,
            TOther
        }
    }
}
