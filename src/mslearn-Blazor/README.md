### 2026-08-03
--------------

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

http://localhost:5204

PS C:\Users\User> curl  http://localhost:5204

Security Warning: Script Execution Risk
Invoke-WebRequest parses the content of the web page. Script code in the
web page might be run when the page is parsed.
      RECOMMENDED ACTION:
      Use the -UseBasicParsing switch to avoid script code execution.

      Do you want to continue?

[Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help
(default is "N"):a
StatusCode        : 200
StatusDescription : OK


PS> dotnet watch