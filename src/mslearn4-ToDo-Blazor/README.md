### 2026-08-04
--------------

**Create the to-do list page as a Blazor Web App**

## Have to be installed .Net 10.0
PS> dotnet --list-sdks
8.0.423 [C:\Program Files\dotnet\sdk]
10.0.301 [C:\Program Files\dotnet\sdk]

PS> dotnet --version
10.0.301

# To generate a web application
PS> dotnet new blazor

# To run an application with "http" profile
PS> dotnet run --launch-profile http
# or
PS> dotnet watch

# Add a Todo.razor file to the Components/Pages folder
PS> dotnet new razorcomponent -n Todo -o Components/Pages

Open the Todo component and add an @page Razor directive to the top of the file with a relative URL of /todo, and set the render mode to InteractiveServer so the component can handle UI events.
```cshtml
@page "/todo"
@rendermode InteractiveServer

<h3>Todo</h3>

@code {

}
```
# Try to see
http://localhost:5198/todo

Open Components/Layout/NavMenu.razor.
Find the nav element in the NavMenu component and add the following div element below the existing nav item for the weather page.
```html
<div class="nav-item px-3">
    <NavLink class="nav-link" href="todo">
        <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Todo
    </NavLink>
</div>
```

