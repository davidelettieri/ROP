using System;
using System.Collections.Generic;
using System.Text;

namespace ROP
{
    public class EmailSent
    {
        public Customer Customer { get; set; }
        public bool EventEmitted { get; set; }
    }
}
