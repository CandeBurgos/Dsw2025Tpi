using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;


namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService
    {
        private readonly IRepository _repository;

        public ProductsManagementService(IRepository repository)
        {
            _repository = repository;
        }

        // Métodos existentes (GET by ID y GET all)
        public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null || !product.IsActive) return null;

            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.Name,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive);
        }

        public async Task<IEnumerable<ProductModel.ProductResponse>> GetProducts()
        {
            var product = await _repository.GetAll<Product>();
            return product?
                .Where(p => p.IsActive)
                .Select(p => new ProductModel.ProductResponse(
                    p.Id,
                    p.Sku,
                    p.Name,
                    p.CurrentUnitPrice,
                    p.StockQuantity,
                    p.IsActive))
                ?? Enumerable.Empty<ProductModel.ProductResponse>();
        }

        public async Task<ProductModel.ProductResponse> AddProduct(ProductModel.ProductRequest request)
        {
            // Validaciones existentes (sin cambios)
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("El SKU es obligatorio");
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("El nombre es obligatorio");
            if (request.CurrentUnitPrice <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");
            if (request.StockQuantity < 0)
                throw new ArgumentException("El stock no puede ser negativo");

            // Verificar SKU único (Punto 1)
            var existingProduct = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (existingProduct != null) throw new DuplicateSkuException(request.Sku);

            var product = new Product(
                request.Sku,
                request.Name,
                request.CurrentUnitPrice,
                request.StockQuantity)
            {
                Description = request.Description,
                InternalCode = request.InternalCode
            };

            await _repository.Add(product);
            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.Name,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive);
        }

        public async Task<ProductModel.ProductResponse?> UpdateProduct(Guid id, ProductModel.ProductRequest request)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null) return null;

            // Validación de SKU único excluyendo el actual (Punto 4)
            var existingProduct = await _repository.First<Product>(p => p.Sku == request.Sku && p.Id != id);
            if (existingProduct != null) throw new DuplicateSkuException(request.Sku);

            // Resto de validaciones (igual que AddProduct)
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("El SKU es obligatorio");
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("El nombre es obligatorio");
            if (request.CurrentUnitPrice <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");
            if (request.StockQuantity < 0)
                throw new ArgumentException("El stock no puede ser negativo");

            product.Sku = request.Sku;
            product.Name = request.Name;
            product.Description = request.Description;
            product.InternalCode = request.InternalCode;
            product.CurrentUnitPrice = request.CurrentUnitPrice;
            product.StockQuantity = request.StockQuantity;

            await _repository.Update(product);
            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.Name,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive);
        }

        public async Task<bool> ToggleProductStatus(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null) return false;

            product.IsActive = !product.IsActive;
            await _repository.Update(product);
            return true;
        }
    }
}

