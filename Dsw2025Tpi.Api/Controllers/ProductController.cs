using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsManagementService _service;

        public ProductsController(ProductsManagementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetProducts();
            if (products == null || !products.Any()) return NoContent();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModel.ProductRequest request)
        {
            try
            {
                var product = await _service.AddProduct(request);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductModel.ProductRequest request)
        {
            try
            {
                var updatedProduct = await _service.UpdateProduct(id, request);
                if (updatedProduct == null) return NotFound();
                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var result = await _service.ToggleProductStatus(id);
            if (!result) return NotFound();
            return NoContent(); // 204 (Punto 5)
        }
    }
}