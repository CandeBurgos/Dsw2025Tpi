using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
      public class OrderValidationException : Exception
        {
            public OrderValidationException(string message) : base(message) { }
        }


    }

