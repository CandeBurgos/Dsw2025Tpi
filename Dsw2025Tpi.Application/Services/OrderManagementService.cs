using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain;
using Dsw2025Tpi.Application.Exceptions;

namespace Dsw2025Tpi.Application.Services
{
    

    public class OrderManagementService
    {
        private readonly IRepository _repository;

        public OrderManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
        {
            var order = await _repository.GetById<Order>(id, nameof(Order.OrderItems));
            if (order == null) return null;

            return new OrderModel.OrderResponse(
                order.Id,
                order.Date,
                order.ShippingAddress,
                order.BillingAddress,
                order.TotalAmount,
                order.Status,
                order.CustomerId.ToString(), // Mostramos el ID directamente
                order.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
                    oi.Id,
                    oi.ProductName,
                    oi.ProductDescription,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.Subtotal)).ToList(),
                order.Notes);
        }

            public async Task<OrderModel.OrderResponse> CreateOrder(OrderModel.OrderRequest request)
            {
                //var customerExists = await _repository.GetById<Order>(request.CustomerId) != null;
                //if (!customerExists) throw new NotFoundException("Cliente", request.CustomerId);
            // Validaciones básicas (direcciones y items)
            if (string.IsNullOrWhiteSpace(request.ShippingAddress))
                    throw new ArgumentException("La dirección de envío es obligatoria");
                if (string.IsNullOrWhiteSpace(request.BillingAddress))
                    throw new ArgumentException("La dirección de facturación es obligatoria");
                if (request.OrderItems == null || !request.OrderItems.Any())
                    throw new ArgumentException("La orden debe contener al menos un item");

                // Verificar stock y calcular TotalAmount (Punto 6)
                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();
                foreach (var item in request.OrderItems)
                {
                    var product = await _repository.GetById<Product>(item.ProductId);
                    if (product == null || product.StockQuantity < item.Quantity)
                        throw new InsufficientStockException(item.ProductId, item.Quantity, product?.StockQuantity ?? 0);

                    var orderItem = new OrderItem(
                        item.ProductId,
                        item.Quantity,
                        item.CurrentUnitPrice, // Snapshot del precio
                        item.Name,
                        item.Description);

                    totalAmount += orderItem.Subtotal;
                    orderItems.Add(orderItem);
                }

                // Crear orden
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    ShippingAddress = request.ShippingAddress,
                    BillingAddress = request.BillingAddress,
                    Notes = request.Notes,
                    Status = OrderStatus.Pending,
                    TotalAmount = totalAmount, // Calculado
                    OrderItems = orderItems
                };

                await _repository.Add(order);

                // Actualizar stock
                foreach (var item in request.OrderItems)
                {
                    var product = await _repository.GetById<Product>(item.ProductId);
                    product.StockQuantity -= item.Quantity;
                    await _repository.Update(product);
                }

                return await GetOrderById(order.Id);
            }

        }
    }

