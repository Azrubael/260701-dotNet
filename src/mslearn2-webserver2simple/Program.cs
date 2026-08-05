using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var items = new List<string> { "Apple", "Banana", "Cherry" };

app.MapGet("/", () => Results.Content("""
<h1>Home</h1>
<ul>
  <li><a href="/about">About</a></li>
  <li><a href="/items">Items</a></li>
  <li><a href="/contact">Contact</a></li>
</ul>
""", "text/html"));

app.MapGet("/about", () => Results.Content("""
<h1>About</h1>
<p>This is a simple ASP.NET Core app.</p>
<p><a href="/">Home</a></p>
""", "text/html"));

app.MapGet("/contact", () => Results.Content("""
<h1>Contact</h1>
<form method="post" action="/contact">
  <input name="message" placeholder="Your message" />
  <button type="submit">Send</button>
</form>
<p><a href="/">Home</a></p>
""", "text/html"));

app.MapGet("/items", () =>
{
    var list = string.Join("", items.Select((item, i) =>
        $"<li>{item} <form method='post' action='/items/{i}/delete' style='display:inline'><button type='submit'>Delete</button></form></li>"));

    return Results.Content($$"""
<h1>Items</h1>
<ul>
  {{list}}
</ul>

<form method="post" action="/items">
  <input name="name" placeholder="New item" />
  <button type="submit">Add</button>
</form>

<p><a href="/">Home</a></p>
""", "text/html");
});

app.MapPost("/items", ([FromForm] string name) =>
{
    if (!string.IsNullOrWhiteSpace(name))
        items.Add(name);

    return Results.Redirect("/items");
});

app.MapDelete("/items/{index:int}", (int index) =>
{
    if (index >= 0 && index < items.Count)
        items.RemoveAt(index);

    return Results.Ok();
});

app.MapPost("/items/{index:int}/delete", (int index) =>
{
    if (index >= 0 && index < items.Count)
        items.RemoveAt(index);

    return Results.Redirect("/items");
});

app.MapPost("/contact", ([FromForm] string message) =>
{
    return Results.Content($"""
<h1>Thanks</h1>
<p>Received: {message}</p>
<p><a href="/">Home</a></p>
""", "text/html");
});

app.Run();
