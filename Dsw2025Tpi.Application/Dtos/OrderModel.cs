using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain;


namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {

        public record OrderRequest(
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            List<OrderItemRequest> OrderItems,
            string? Notes = null);

        public record OrderItemRequest(
            Guid ProductId,
            int Quantity,
            string Name,
            string Description,
            decimal CurrentUnitPrice); // Precio al momento de la compra




        public record OrderResponse(
                Guid Id,
                DateTime Date,
                string ShippingAddress,
                string BillingAddress,
                decimal TotalAmount,
                OrderStatus Status,
                string CustomerName,
                List<OrderItemResponse> OrderItems,
                string? Notes = null);

        public record OrderItemResponse(
            Guid Id,
            string ProductName,
            string ProductDescription,
            int Quantity,
            decimal UnitPrice,
            decimal Subtotal);
    }
}
    

