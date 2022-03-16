using ROP.UnionType;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ROP
{
    public class Result<TSuccess, TError> : Union<TSuccess, TError>
    {
        public Result([NotNull] TSuccess value) : base(value)
        {
        }

        public Result([NotNull] TError error) : base(error)
        {
        }

        public static implicit operator Result<TSuccess, TError>(TSuccess v) => new(v);
        public static implicit operator Result<TSuccess, TError>(TError e) => new(e);
    }

    public class Result<TSuccess> : Union<TSuccess, Failure>
    {
        public Result([NotNull] TSuccess value) : base(value)
        {
        }

        public Result([NotNull] Failure error) : base(error)
        {
        }

        public static implicit operator Result<TSuccess>([NotNull] TSuccess v) => new(v);
        public static implicit operator Result<TSuccess>([NotNull] Failure e) => new(e);
    }
}
