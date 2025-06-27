using Dsw2025Tpi.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;


namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderManagementService _service;

        public OrdersController(OrderManagementService service)
        {
            _service = service;
        }

        // Punto 6: Endpoint para crear una orden
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
        {
            try
            {
                var order = await _service.CreateOrder(request);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                // Captura errores de validación (400 Bad Request)
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Error interno del servidor (500)
                return StatusCode(500, "Ocurrió un error al procesar la orden.");
            }
        }

        // Helper para CreatedAtAction
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _service.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}


