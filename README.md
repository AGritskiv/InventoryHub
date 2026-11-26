# InventoryHub — Full-Stack Integration Project

**Author:** Andrey Gritskiv  
**Tech Stack:** ASP.NET Core Minimal API (.NET) & Blazor WebAssembly  

## Overview
**InventoryHub** is a full-stack inventory dashboard designed and built by **Andrey Gritskiv**. It connects a Blazor WebAssembly front-end (`ClientApp`) with an ASP.NET Core Web API back-end (`ServerApp`), featuring structured JSON payloads with SKU tracking, real-time client-side filtering, resilient error handling, and server-side memory caching.

## Key Features
- **Custom JSON Schema:** Includes nested `Category` objects, unique `SKU` codes, and computed availability statuses (`In Stock`, `Low Stock`, `Out of Stock`).
- **Performance Optimization:** Uses `IMemoryCache` on the server with custom `X-Cache-Status` (`HIT`/`MISS`) response headers and zero-latency client-side search/filtering.
- **Resilient API Integration:** Implements `CancellationTokenSource` timeouts, CORS policies, and granular exception handling.

## How to Run Locally
1. Start the back-end API (`http://localhost:5000`):
   ```bash
   cd ServerApp
   dotnet run
   ```
2. Open a second terminal and start the Blazor client:
   ```bash
   cd ClientApp
   dotnet run
   ```
3. Open the local URL shown in the `ClientApp` terminal.

---

## Reflective Summary: How Microsoft Copilot Assisted

### 1. Generating and Refining Integration Code
Microsoft Copilot helped scaffold the `HttpClient` dependency injection in `ClientApp/Program.cs` and structure the asynchronous data-fetching workflow in `Home.razor`. With Copilot's suggestions, I extended the basic fetch logic to support a manual cache-refresh toggle (`?refresh=true`) and custom response header inspection (`X-Cache-Status`).

### 2. Debugging and Resolving Integration Issues
When connecting the Blazor front-end to `ServerApp`, browser CORS restrictions initially blocked requests and custom headers. Copilot helped configure a dedicated `InventoryHubCorsPolicy` with `.WithExposedHeaders(...)` and guided the implementation of `try-catch` blocks for `HttpRequestException`, `TaskCanceledException`, and `JsonException`.

### 3. Structuring JSON Responses
Working with Copilot, I designed a clean, strongly typed JSON contract shared across both projects (`Product` with nested `Category`, `Sku`, and computed `StockStatus`). Enabling `PropertyNameCaseInsensitive = true` eliminated camelCase/PascalCase deserialization mismatches.

### 4. Optimizing Performance
Copilot assisted in implementing a two-tier optimization strategy:
- **Server-Side:** Configured `IMemoryCache` with sliding and absolute expiration policies so repeated API calls return cached data (`X-Cache-Status: HIT`).
- **Client-Side:** Implemented LINQ-based search and category filtering directly on the loaded array in memory, eliminating unnecessary round-trip HTTP requests while typing.