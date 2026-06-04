using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ForbiddenException :AppException
    { 
        public ForbiddenException(string message = "Acceso denegado.")
            : base(message, 403)
        {
        }
    }
}
