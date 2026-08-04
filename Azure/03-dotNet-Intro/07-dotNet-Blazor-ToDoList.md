### 2026-08-03
--------------

**QUIZ**
Q1: How do you make a Blazor component interactive so that it can handle UI events?
A1: Apply an interactive render mode using @rendermode directive.

Q2: What happens to a component immediately after it handles a UI event?
A2: The component is rendered.

Q3: What is data binding used for in Blazor?
A3: Tp bind the value of a UI element to evaluate in code.

Q4: Which of ht following examples isn't a valid Razor directive?
@currentCount


*Build a to-do list with Blazor*

When you want to render the value of a C# expression in Razor, you use a leading @ character. For example, a Counter component can render the value of its currentCount field like this:
```Razor
<p role="status">Current count: @(currentCount)</p>
	@if (currentCount > 3)
	{
		<p>You win!</p>
	}
<ul>
    @foreach (var item in items)
    {
        <li>@item.Name</li>
    }
</ul>
<button class="btn btn-primary" @onclick="() => currentCount++">Click me</button>
<input @onchange="InputChanged" />
<p>@message</p>

@code {
    string message = "";

    void InputChanged(ChangeEventArgs e)
    {
        message = (string)e.Value;
    }
}
```
After an event handler runs, Blazor will automatically render the component with its new state, so the message is displayed after the input changes.

Blazor's data binding support makes it easy to set up two-way data bindingЖ
```Razor
<input @bind="text" />
<button @onclick="() => text = string.Empty">Clear</button>
<p>@text</p>

@code {
    string text = "";
}
```
When you change the value of the input, the text field is updated with the new value. And when you change the value of the text field by clicking the Clear button, the value of the input is also cleared.