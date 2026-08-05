### 2026-08-05
--------------

There are two hosting models for code in Blazor apps:

[1] Blazor Server: In this model, the app is executed on the web server within an ASP.NET Core app. On the client side, UI updates, events, and JavaScript calls, are sent through a SignalR connection between the client and the server. In this module, we discuss and code for this model.
[2] Blazor WebAssembly: In this model, the Blazor app, its dependencies, and the .NET runtime are downloaded and run on the browser.


*NB*: `Razor` syntax is used for embedding .NET code into webpages. You can use it in ASP.NET MVC (Model-View-Controller) applications, where files have a .cshtml extension. Razor syntax is used in Blazor to write components. These components have the .razor extension instead, and there's no strict separation between controllers and views.


PS> dotnet new blazor -o IteractiveBlazor

# The default components include the Index.razor home page and the Counter.razor demo component. Both of these components are placed in the Pages folder. 
# To add a new component to an existing web app, use this command:

PS> mv IteractiveBlazor mslearn5-IteractiveBlazor
PS> cd mslearn5-IteractiveBlazor
PS> dotnet new razorcomponent -n PizzaBrowser -o Pages
PS> vim Pages/PizzaBrowser.razor
```Razor
@page "/index"

<h1>Welcome to Blazing Pizza</h1>

<p>@welcomeMessage</p>

@code {
  private string welcomeMessage = "However you like your pizzas, we can deliver them fast!";
}
```

http://localhost:5285/index

PS> dotnet --list-sdks
8.0.423 [C:\Program Files\dotnet\sdk]
10.0.301 [C:\Program Files\dotnet\sdk]

