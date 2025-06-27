
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Application.Services;

namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
         // Agregar servicios al contenedor
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();

        // Base de datos con SQL Server y Entity Framework Core
        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Si usás una estrategia de carga desde JSON, agregala aquí (opcional)
            // Ejemplo:
            // options.UseSeeding((context, type) => {
            //     ((Dsw2025TpiContext)context).Seedwork<Product>("Sources/products.json");
            //     ((Dsw2025TpiContext)context).Seedwork<Customer>("Sources/customers.json");
            // });
        });

        // Inyección de dependencias
        builder.Services.AddScoped<IRepository, EfRepository>();
        builder.Services.AddScoped<ProductsManagementService>();
        builder.Services.AddScoped<OrderManagementService>();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        var app = builder.Build();
        

        // Configuración del pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/health-check");

        app.Run();

    }
}