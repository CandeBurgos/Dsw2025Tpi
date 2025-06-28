using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Helpers
{
    public static class DbContextExtensions
    {
        public static void Seedwork<T>(this Dsw2025TpiContext context, string dataSource) where T : class
      {
        if (context.Set<T>().Any()) return;

        var path = Path.Combine(AppContext.BaseDirectory, dataSource);
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var entities = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // o null si no usás camelCase
        });

        if (entities == null || entities.Count == 0) return;

        context.Set<T>().AddRange(entities);
        context.SaveChanges();
    }

}

}

