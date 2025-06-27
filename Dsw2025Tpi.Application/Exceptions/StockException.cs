using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public class StockException : BusinessException
    { public Guid ProductId { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public StockException(Guid productId, int requested, int available)
            : base($"Stock insuficiente. Producto: {productId}, Solicitado: {requested}, Disponible: {available}")
        {
            ProductId = productId;
            RequestedQuantity = requested;
            AvailableQuantity = available;
        }
    }
     }
   

   
    
