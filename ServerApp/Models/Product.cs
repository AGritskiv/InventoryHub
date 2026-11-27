// Author: Andrey Gritskiv
// Project: InventoryHub - Back-End Data Models

namespace ServerApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; } = new Category();

        public string StockStatus => Stock switch
        {
            0 => "Out of Stock",
            <= 10 => "Low Stock",
            _ => "In Stock"
        };
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
