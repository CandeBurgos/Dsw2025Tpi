using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain
{
    public enum OrderStatus
    {
        Pending, // Estado inicial (automático)
        Processing,   // En preparación
        Shipped,      // Enviada
        Delivered,    // Entregada
        Cancelled     // Cancelada
    }
}

