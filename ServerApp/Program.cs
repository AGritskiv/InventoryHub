// Author: Andrey Gritskiv
// Project: InventoryHub - Back-End API with In-Memory Caching

using Microsoft.Extensions.Caching.Memory;
using ServerApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

// Configure CORS policy to allow Blazor WebAssembly client and custom headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("InventoryHubCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Cache-Status", "X-Developer");
    });
});

var app = builder.Build();

app.UseCors("InventoryHubCorsPolicy");

app.MapGet("/api/productlist", (IMemoryCache cache, HttpContext context, bool refresh = false) =>
{
    const string cacheKey = "ag_inventory_products_v1";
    context.Response.Headers.Append("X-Developer", "Andrey Gritskiv");

    if (refresh)
    {
        cache.Remove(cacheKey);
    }

    if (!cache.TryGetValue(cacheKey, out List<Product>? products))
    {
        products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Sku = "AG-TECH-001",
                Name = "UltraBook Pro 15",
                Price = 1349.99,
                Stock = 18,
                Category = new Category { Id = 101, Name = "Laptops" }
            },
            new Product
            {
                Id = 2,
                Sku = "AG-AUD-014",
                Name = "Noise-Canceling Studio Headphones",
                Price = 199.50,
                Stock = 7,
                Category = new Category { Id = 102, Name = "Audio" }
            },
            new Product
            {
                Id = 3,
                Sku = "AG-PER-089",
                Name = "Wireless Mechanical Keyboard",
                Price = 115.00,
                Stock = 34,
                Category = new Category { Id = 103, Name = "Peripherals" }
            },
            new Product
            {
                Id = 4,
                Sku = "AG-MON-022",
                Name = "27-inch 4K IPS Monitor",
                Price = 420.00,
                Stock = 0,
                Category = new Category { Id = 104, Name = "Displays" }
            },
            new Product
            {
                Id = 5,
                Sku = "AG-PER-105",
                Name = "Ergonomic Vertical Mouse",
                Price = 64.99,
                Stock = 9,
                Category = new Category { Id = 103, Name = "Peripherals" }
            }
        };

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

        cache.Set(cacheKey, products, cacheOptions);
        context.Response.Headers.Append("X-Cache-Status", "MISS");
    }
    else
    {
        context.Response.Headers.Append("X-Cache-Status", "HIT");
    }

    return Results.Ok(products);
});

app.Run();
