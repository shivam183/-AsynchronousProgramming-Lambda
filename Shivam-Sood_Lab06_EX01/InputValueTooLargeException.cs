using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shivam_Sood_Lab06_EX01
{
    class InputValueTooLargeException:Exception
    {
        public InputValueTooLargeException() { }

        public InputValueTooLargeException(string msg):base(msg) { }

        public InputValueTooLargeException(string msg,Exception inner) : base(msg, inner) { }
    }
}
