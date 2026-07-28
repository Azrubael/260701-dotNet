### 2026-07-28
--------------
**Tutorial: Create a Minimal API with ASP.NET Core**
https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0&tabs=visual-studio-code

API 					Description 				Request body 	Response body
GET /todoitems 			Get all to-do items 		None 			Array of to-do items
GET /todoitems/complete	Get completed to-do items 	None 			Array of to-do items
GET /todoitems/{id} 	Get an item by ID 			None 			To-do item
POST /todoitems 		Add a new item 				To-do item 		To-do item
PUT /todoitems/{id} 	Update an existing item   	To-do item 		None
PATCH /todoitems/{id} 	Partially update an item   	Partial to-do item 	None
DELETE /todoitems/{id} 	Delete an item     			None 			None

PS > dotnet new webapi -o TodoApi
PS > type Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

PS > dotnet add package Microsoft.EntityFrameworkCore.InMemory
