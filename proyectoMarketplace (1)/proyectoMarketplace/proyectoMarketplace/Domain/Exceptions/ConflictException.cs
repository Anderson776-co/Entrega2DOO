using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message = "Conflicto de datos.")
            : base(message, 409)
        {
        }
    }
}
