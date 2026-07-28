### 2026-07-27
--------------

PS D:\Project\code\dotNet\src> cd mslearnContosoPizza/
PS D:\Project\code\dotNet\src\mslearnContosoPizza> dotnet new webapi -controllers...
Restore succeeded with 1 warning(s) in 3,5s
    D:\Project\code\dotNet\src\mslearnContosoPizza\mslearnContosoPizza.csproj : warning NU1903: Package 'Microsoft.OpenApi' 2.0.0 has a known high severity `vulnerability`, https://github.com/advisories/GHSA-v5pm-xwqc-g5wc
Restore succeeded.

D:\Project\code\dotNet\src\mslearnContosoPizza> dotnet list mslearnContosoPizza.csproj package --include-transitive                                                            
Restore succeeded with 1 warning(s) in 0,3s
    D:\Project\code\dotNet\src\mslearnContosoPizza\mslearnContosoPizza.csproj : warning NU1903: Package 'Microsoft.OpenApi' 2.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-v5pm-xwqc-g5wc

`Build succeeded` with 1 warning(s) in 0,6s
Project 'mslearnContosoPizza' has the following package references
   [net10.0]: 
   Top-level Package                   Requested   Resolved
   > Microsoft.AspNetCore.OpenApi      10.0.9      10.0.9  

   Transitive Package       Resolved

PS D:\Project\code\dotNet\src\mslearnContosoPizza> type mslearnContosoPizza.csproj
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="11.0.0-preview.6.26359.118" />
  </ItemGroup>

PS D:\Project\code\dotNet\src\mslearnContosoPizza> dotnet list .\mslearnContosoPizza.csproj package --include-transitive                         
Restore complete (0,3s)

Build succeeded in 0,6s
Project 'mslearnContosoPizza' has the following package references
   [net10.0]: 
   Top-level Package                   Requested                    Resolved                  
   > Microsoft.AspNetCore.OpenApi      11.0.0-preview.6.26359.118   11.0.0-preview.6.26359.118
```
   
PS D:\Project\code\dotNet\src\mslearnContosoPizza> ll -lhD

d-----             785 B  27.07.2026 19:51  Controllers/
d-----           64,19Ki  27.07.2026 20:07  obj/
d-----             644 B  27.07.2026 19:51  Properties/
-a----             127 B  27.07.2026 19:51  appsettings.Development.json
-a----             151 B  27.07.2026 19:51  appsettings.json
-a----             355 B  27.07.2026 20:07  mslearnContosoPizza.csproj
-a----             151 B  27.07.2026 19:51  mslearnContosoPizza.http
-a----             468 B  27.07.2026 19:51  Program.cs
-a----             268 B  27.07.2026 19:51  WeatherForecast.cs

The total size of the current directory is: 67,07Ki

```
Controllers/ 				Contains classes with public methods exposed as HTTP endpoints.
Program.cs 					Configures services and the app's HTTP request pipeline, and contains the app's managed entry point.
mslearnContosoPizza.csproj 	Contains configuration metadata for the project.
mslearnContosoPizza.http 	Contains configuration to test REST APIs
```


PS D:\Project\code\dotNet\src\mslearn-ContosoPizza> dotnet new webapi -controllers -f net8.0

PS D:\Project\code\dotNet\src\mslearn-ContosoPizza> dotnet run --launch-profile http

### Отриманий статус Ок
PS C:\Users\User> curl http://localhost:5078/swagger
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

PS C:\Users\User> curl http://localhost:5078/weatherforecast
StatusCode        : 200
StatusDescription : OK
Content           : [{"date":"2026-07-29","temperatureC":40,"temperatureF":103,"summary":"Balmy"},{
                    "date":"2026-07-30","temperatureC":-3,"temperatureF":27,"summary":"Sweltering"}
                    ,{"date":"2026-07-31","temperatureC":4,"te...
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Tue, 28 Jul 2026 18:00:23 GMT
                    Server: Kestrel

                    [{"date":"2026-07-29","temperatureC":40,"temperature...
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8],
                     [Date, Tue, 28 Jul 2026 18:00:23 GMT], [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 392

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