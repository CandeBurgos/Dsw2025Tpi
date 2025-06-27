using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
   
    public class InvalidProductDataException : BusinessException
    {
        public string FieldName { get; }

        public InvalidProductDataException(string fieldName)
            : base($"Dato inválido en el campo {fieldName}")
        {
            FieldName = fieldName;
        }
    }

    public class DuplicateSkuException : BusinessException
    {
        public string Sku { get; }

        public DuplicateSkuException(string sku)
            : base($"El SKU {sku} ya está en uso")
        {
            Sku = sku;
        }
    }

    

    public class DuplicatedEntityException : Exception
    {
        public DuplicatedEntityException(string message) : base(message) { }
    }

    public class InsufficientStockException : BusinessException
    {
        public Guid ProductId { get; }
        public int Requested { get; }
        public int Available { get; }

        public InsufficientStockException(Guid productId, int requested, int available)
            : base($"Stock insuficiente para el producto {productId}")
        {
            ProductId = productId;
            Requested = requested;
            Available = available;
        }
    }
}

