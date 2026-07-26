### 2026-07-25
--------------

If debugging is the process of removing bugs, then programming must be the process of putting them in.

A debugger is a software tool you can use to observe and control the execution flow of your program with an analytical approach. Its design goal is to help find the root cause of a bug and help you resolve it. 

Every debugger has its own set of features. The two most important ones:
`Controlling your program execution`: You can pause your program and run it step by step, which allows you to see which code is executed and how it affects your program's state.
`Observing your program's state`: For example, you can look at the value of your variables and function parameters at any point during your code execution.


To handle terminal input while debugging, you can use the integrated terminal (one of the Visual Studio Code windows) or an external terminal. For this tutorial, you use the integrated terminal.

Open .vscode/launch.json.
Change the console setting to integratedTerminal from:
    
```JSON
    "console": "internalConsole",
```
    
```JSON
    "console": "integratedTerminal",
```

You can use `System.Diagnostics.Debug` and `System.Diagnostics.Trace` in addition to `System.Console`. Both Debug and Trace are part of System.Diagnostics and will only write to logs when an appropriate listener is attached.

```csharp
Console.WriteLine("This message is readable by the end user.");
Trace.WriteLine("This is a trace message when tracing the app.");
Debug.WriteLine("This is a debug message just for developers.");
```

**Define TRACE and DEBUG constants**
By default, when an application is running under debug, the `DEBUG` constant is defined. You can control this by adding a `DefineConstants` entry in the project file in a property group. Here's an example of turning on `TRACE` for both `Debug` and `Release` configurations in addition to `DEBUG` for `Debug` configurations.
```xml
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DefineConstants>DEBUG;TRACE</DefineConstants>
</PropertyGroup>
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <DefineConstants>TRACE</DefineConstants>
</PropertyGroup>
```
When you use `Trace` not attached to the debugger, you'll need to configure a trace listener such as `dotnet-trace` (https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-trace).

In addition to simple Write and WriteLine methods, there's also the capability to add conditions with `WriteIf` and `WriteLineIf`. As an example, the following logic checks if the count is zero and then writes a debug message:
```csharp
bool errorFlag = false;  
System.Diagnostics.Trace.WriteIf(errorFlag, "Error in AppendData procedure.");  
System.Diagnostics.Debug.WriteIf(errorFlag, "Transaction abandoned.");  
System.Diagnostics.Trace.Write("Invalid value for data request");
```

Use the `System.Diagnostics.Debug.Assert` method freely to test conditions that should hold true if your code is correct. owever, the comparison isn't made in the release version, so there's no additional overhead.
```csharp
int IntegerDivide(int dividend, int divisor)
{
    Debug.Assert(divisor != 0, $"{nameof(divisor)} is 0 and will cause an exception.");

    return dividend / divisor;
}
```

When you use `System.Diagnostics.Debug.Assert`, make sure that any code inside `Assert` doesn't change the results of the program if `Assert` is removed. Otherwise, you might accidentally introduce a bug that only shows up in the release version of your program. Be especially careful about asserts that contain function or procedure calls.

