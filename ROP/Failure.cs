using System;
using System.Collections.Generic;
using System.Text;

namespace ROP
{
    public class Failure
    {
        public string ReasonId { get; }
        public string Message { get; }

        public Failure(string reasonId, string message)
        {
            ReasonId = reasonId;
            Message = message;
        }
    }
}
