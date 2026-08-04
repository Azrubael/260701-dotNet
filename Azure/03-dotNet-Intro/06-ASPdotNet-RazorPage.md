### 2026-08-02
--------------

**The benefits of Razor Pages**

Razor syntax combines HTML and C# to define the dynamic rendering logic.
Razor Pages is a server-side, page-centric programming model for building web UIs with ASP.NET Core. Benefits include:
* Easy setup for dynamic web apps using HTML, CSS, and C#.
* Organized files by feature for easier maintenance.
* Combines markup with server-side C# code using Razor syntax.

`Razor Pages` utilize `Razor` for embedding server-based code into webpages.

Razor Pages обеспечивает разделение ответственности с помощью класса PageModel на C#, инкапсулирующего свойства данных и логические операции, относящиеся к конкретной странице Razor, а также определяющего обработчики страниц для HTTP-запросов.

Use Razor Pages to keep ASP.NET Core pages organized in a simpler way. All view (page) specific logic and page properties (view model) are kept in the same proximity.

```powershell
git clone https://github.com/MicrosoftDocs/mslearn-create-razor-pages-aspnet-core
mv "mslearn-create-razor-pages-aspnet-core" "mslearn-razor-aspnet-core"

```

# The following example is a simple Blazor counter component implemented in Razor. Every time the button is pressed the `IncrementCount` C# method is invoked, which increments the `currentCount` field, and then the component renders the updated value:
```Razor
<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

By default, Blazor components are `rendered statically from the server`, generating HTML in response to requests.
You can also configure server components to be `interactive`, so they can handle arbitrary UI events, maintain state across interactions, and render updates dynamically. Interactive server components handle UI interactions and updates over a WebSocket connection with the browser.
`Alternatively`, Blazor components can be rendered interactively from the client. The component is downloaded to the client and run from the browser via WebAssembly. Interactive WebAssembly components can access client resources through the web platform, like local storage and hardware, and can even function offline once downloaded.
You can choose to render different components from the server or the client within the same app.
To offload work from the server, interactive components are rendered on the client via WebAssembly.


**QUIZ**
--------
1. What do you need to install at a minimum to create and run Blazor apps?
.Net SDK

2. Blazor components are typically authored using what coding language?
Razor

3. Razor files are compiled int owhat C$ language feature?
A C# class

4. How are Blazor component  parameters defined?
By using properties and the [Parameter] attribute.

