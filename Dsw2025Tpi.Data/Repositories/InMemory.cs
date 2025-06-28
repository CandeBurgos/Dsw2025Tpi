using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Domain;

namespace Dsw2025Tpi.Data.Repositories
{
   
    public class InMemory : IRepository
    {
        private List<Product> _products = new();
        private List<Order> _orders = new();

        public InMemory()
        {
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            // Cargar productos desde JSON
            var productsJson = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\products.json"));
            _products = JsonSerializer.Deserialize<List<Product>>(productsJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            }) ?? new List<Product>();

            // Orden de ejemplo (para pruebas)
            _orders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(), // Simulado según documento
                Date = DateTime.UtcNow.AddDays(-1),
                Status = OrderStatus.Pending,
                ShippingAddress = "Calle Ejemplo 123",
                BillingAddress = "Calle Ejemplo 123",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = _products.First().Id,
                        Quantity = 1,
                        UnitPrice = _products.First().CurrentUnitPrice,
                        ProductName = _products.First().Name,
                        ProductDescription = _products.First().Description
                    }
                }
            }
        };
        }

        private List<T> GetList<T>() where T : EntityBase
        {
            return typeof(T).Name switch
            {
                nameof(Product) => _products.Cast<T>().ToList(),
                nameof(Order) => _orders.Cast<T>().ToList(),
                nameof(OrderItem) => _orders.SelectMany(o => o.OrderItems).Cast<T>().ToList(),
                _ => throw new NotSupportedException($"Tipo {typeof(T).Name} no soportado")
            };
        }
        public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
        {
            var entity = GetList<T>()?.FirstOrDefault(e => e.Id == id);

            // Manejo básico de includes para Order -> OrderItems
            if (entity is Order order && include.Contains(nameof(Order.OrderItems)))
            {
                // En memoria ya están cargadas las relaciones
            }

            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
        {
            return await Task.FromResult(GetList<T>());
        }

        public async Task<T> Add<T>(T entity) where T : EntityBase
        {
            GetList<T>().Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<T> Update<T>(T entity) where T : EntityBase
        {
            var list = GetList<T>();
            var existing = list.FirstOrDefault(e => e.Id == entity.Id);
            if (existing != null)
            {
                list.Remove(existing);
                list.Add(entity);
            }
            return await Task.FromResult(entity);
        }

        public async Task<T> Delete<T>(T entity) where T : EntityBase
        {
            GetList<T>().Remove(entity);
            return await Task.FromResult(entity);
        }

        public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            return await Task.FromResult(GetList<T>()?.FirstOrDefault(predicate.Compile()));
        }

        public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            return await Task.FromResult(GetList<T>()?.Where(predicate.Compile()));
        }

        // Métodos específicos para gestión de stock (requeridos para punto 6)
        public async Task<bool> CheckStockAsync(Guid productId, int quantity)
        {
            var product = await GetById<Product>(productId);
            return product?.StockQuantity >= quantity;
        }

        public async Task UpdateStockAsync(Guid productId, int quantity)
        {
            var product = await GetById<Product>(productId);
            if (product != null)
            {
                product.StockQuantity += quantity;
                await Update(product);
            }
        }
    }
}

