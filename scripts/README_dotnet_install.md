PS D:\Project\code\dotNet\scripts> ./dotnet-install.ps1 -Version 8.0.423

PS D:\Project\code\dotNet\scripts> dotnet --version
10.0.302

PS D:\Project\code\dotNet\scripts> dotnet --list-sdks
8.0.423 [C:\Users\User\AppData\Local\Microsoft\dotnet\sdk]
10.0.302 [C:\Users\User\AppData\Local\Microsoft\dotnet\sdk]

PS D:\Project\code\dotNet\src\mslearn-ContosoPizza> dotnet new webapi -controllers -f net8.0

PS D:\Project\code\dotNet\src\mslearn-ContosoPizza> dotnet run --launch-profile http

### Отриманий статус Ок
PS C:\Users\User> curl http://localhost:5078/swagger

Security Warning: Script Execution Risk
Invoke-WebRequest parses the content of the web page. Script code in the
web page might be run when the page is parsed.
      RECOMMENDED ACTION:
      Use the -UseBasicParsing switch to avoid script code execution.

      Do you want to continue?

[Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help
(default is "N"):A


StatusCode        : 200
StatusDescription : OK
Content           : <!-- HTML for static distribution bundle build -->
                    <!DOCTYPE html>
                    <html lang="en">
                    <head>
                        <meta charset="UTF-8">
                        <title>Swagger UI</title>
                        <link rel="stylesheet" type="text/css" href="./s...
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: text/html;charset=utf-8
                    Date: Tue, 28 Jul 2026 17:49:01 GMT
                    Server: Kestrel

                    <!-- HTML for static distribution bundle build -->
                    <!DOCTYPE...
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, text/html
                    ;charset=utf-8], [Date, Tue, 28 Jul 2026 17:49:01 GMT],
                     [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 4687


PS > type Program.css
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ===== If there is any problem with https profile!
// app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### Installation the addition package for Read-Eval-Print Loop (REPL) command-line tool
dotnet tool install -g Microsoft.dotnet-httprepl
###

D:\Project\code\dotNet>httprepl http://localhost:5078
Welcome to HttpRepl 8.0!
------------------------

Telemetry
---------
The .NET tools collect usage data in order to help us improve your experience. The data is collected by Microsoft and shared with the community. You can opt-out of telemetry by setting the DOTNET_HTTPREPL_TELEMETRY_OPTOUT environment variable to '1' or 'true' using your favorite shell.

Read more about HttpRepl telemetry: https://aka.ms/httprepl-telemetry
Read more about .NET CLI Tools telemetry: https://aka.ms/dotnet-cli-telemetry

(Disconnected)> connect http://localhost:5078
Using a base address of http://localhost:5078/
Using OpenAPI description at http://localhost:5078/swagger/v1/swagger.json
For detailed tool info, see https://aka.ms/http-repl-doc

http://localhost:5078/> ls
.                 []
WeatherForecast   [GET]

http://localhost:5078/> cd WeatherForecast
/WeatherForecast    [GET]

http://localhost:5078/WeatherForecast> get
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Tue, 28 Jul 2026 18:07:03 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "date": "2026-07-29",
    "temperatureC": 0,
    "temperatureF": 32,
    "summary": "Hot"
  },
  {
    "date": "2026-07-30",
    "temperatureC": 11,
    "temperatureF": 51,
    "summary": "Cool"
  },
  {
    "date": "2026-07-31",
    "temperatureC": 26,
    "temperatureF": 78,
    "summary": "Balmy"
  },
  {
    "date": "2026-08-01",
    "temperatureC": 39,
    "temperatureF": 102,
    "summary": "Warm"
  },
  {
    "date": "2026-08-02",
    "temperatureC": 10,
    "temperatureF": 49,
    "summary": "Cool"
  }
]


http://localhost:5078/WeatherForecast> exit