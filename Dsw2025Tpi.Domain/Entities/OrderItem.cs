using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dsw2025Tpi.Domain.Entities
{       
    public class OrderItem : EntityBase
    {
        public OrderItem()
        {
        }

        public OrderItem(Guid productId, int quantity, decimal unitPrice,
                       string productName, string productDescription)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductName = productName;
            ProductDescription = productDescription;
        }

        public Guid OrderId { get; set; }           // Relación con Order
        public Guid ProductId { get; set; }         // Relación con Product
        public int Quantity { get; set; }           // Cantidad del producto
        public decimal UnitPrice { get; set; }      // Precio unitario al momento de la compra
        public string ProductName { get; set; }     // Nombre del producto (snapshot)
        public string ProductDescription { get; set; } // Descripción (snapshot)

        // Propiedad calculada (no se persiste)
        public decimal Subtotal => Quantity * UnitPrice;

        // Relaciones de navegación
        public Order Order { get; set; }            // Orden a la que pertenece
        public Product Product { get; set; }        // Producto relacionado
    }
}


