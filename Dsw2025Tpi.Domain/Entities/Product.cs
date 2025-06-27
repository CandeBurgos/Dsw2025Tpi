using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dsw2025Tpi.Domain.Entities
{

public class Product : EntityBase
{
    public Product()
    {
    }

    public Product(string sku, string name, decimal currentUnitPrice, int stockQuantity)
    {
        Sku = sku;
        Name = name;
        CurrentUnitPrice = currentUnitPrice;
        StockQuantity = stockQuantity;
        IsActive = true;
    }

    public string Sku { get; set; }
    public string InternalCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
}
    
}
