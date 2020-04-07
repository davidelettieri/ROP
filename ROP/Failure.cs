using System;
using System.Collections.Generic;
using System.Text;

namespace ROP
{
    public class Failure
    {
        public string ReasonId { get; }
        public string Message { get; }

        protected Failure(string reasonId, string message)
        {
            ReasonId = reasonId;
            Message = message;
        }
    }
}
