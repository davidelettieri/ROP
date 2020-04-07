using ROP.UnionType;
using System.Collections.Generic;
using System.Text;

namespace ROP
{
    public class Result<TSuccess, TError> : Union<TSuccess, TError>
    {
        public Result(TSuccess value) : base(value)
        {
        }

        public Result(TError error) : base(error)
        {
        }

        public static implicit operator Result<TSuccess, TError>(TSuccess v) => new Result<TSuccess, TError>(v);
        public static implicit operator Result<TSuccess, TError>(TError e) => new Result<TSuccess, TError>(e);
    }

    public class Result<TSuccess> : Union<TSuccess, Failure>
    {
        public Result(TSuccess value) : base(value)
        {
        }

        public Result(Failure error) : base(error)
        {
        }

        public static implicit operator Result<TSuccess>(TSuccess v) => new Result<TSuccess>(v);
        public static implicit operator Result<TSuccess>(Failure e) => new Result<TSuccess>(e);
    }
}
