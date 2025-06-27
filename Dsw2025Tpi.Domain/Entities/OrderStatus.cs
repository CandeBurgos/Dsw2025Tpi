using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain
{
    public enum OrderStatus
    {
        PENDING,      // Estado inicial (automático)
        PROCESSING,   // En preparación
        SHIPPED,      // Enviada
        DELIVERED,    // Entregada
        CANCELLED     // Cancelada
    }
}

