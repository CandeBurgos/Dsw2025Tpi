using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entityName, Guid id)
            : base($"La entidad {entityName} con ID {id} no fue encontrada") { }
    }
}

