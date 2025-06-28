
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Application.Services;
using System;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Data.Helpers;
using System.Text.Json.Serialization;

namespace Dsw2025Tpi.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        

        
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        
        // Agregar servicios al contenedor
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();

        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Carga datos desde JSON si la tabla está vacía
            options.UseSeeding((context, _) =>
            {
                ((Dsw2025TpiContext)context).Seedwork<Product>("Fuentes/products.json");
                ((Dsw2025TpiContext)context).Seedwork<Order>("Fuentes/orders.json");
            });
        });


        //    builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Inyección de dependencias
        builder.Services.AddScoped<IRepository, EfRepository>();
        builder.Services.AddScoped<ProductsManagementService>();
        builder.Services.AddScoped<OrderManagementService>();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        var app = builder.Build();

        //using (var scope = app.Services.CreateScope())
        //{
        //    var context = scope.ServiceProvider.GetRequiredService<Dsw2025TpiContext>();
        //    context.Seedwork<Product>("Sources/products.json");
        //    context.Seedwork<Order>("Sources/orders.json");
        //}
         // si es necesario

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Dsw2025TpiContext>();
            db.Database.Migrate(); // si usás migraciones

            db.Seedwork<Product>("Fuentes/products.json");
            db.Seedwork<Order>("Fuentes/orders.json");
        }

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