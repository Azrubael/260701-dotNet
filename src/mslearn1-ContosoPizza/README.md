### 2026-07-29
--------------

**A webApi project with .Net 8**

# To install .Net 8.0 SDK
PS> ./dotnet-install.ps1 -Version 8.0.423
PS> dir "$env:LOCALAPPDATA\Microsoft\dotnet\sdk"

PS> dotnet --list-sdks

PS> dotnet --version

# An optional package to transform a datatime object
PS> dotnet add package Newtonsoft.Json

# To generate a web application
PS> dotnet new webapi -controllers -f net8.0

# To run an application with "http" profile
PS> dotnet run --launch-profile http

# To install a tool intended to work with .Net core REPL
PS> dotnet tool install -g Microsoft.dotnet-httprepl

PS > type Properties/launchSettings.json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:16212",
      "sslPort": 44338
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5078",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7061;http://localhost:5078",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

PS> curl http://localhost:5078/weatherforecast

StatusCode        : 200
StatusDescription : OK
Content           : [{"date":"2026-07-30","temperatureC":-16,"temperatureF":4,"summary":"Sweltering"},{"date":"2026-07-31","temperatureC":-13,"temperatureF
                    ":9,"summary":"Freezing"},{"date":"2026-08-01","temperatureC":-9,...
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Wed, 29 Jul 2026 16:08:55 GMT
                    Server: Kestrel

                    [{"date":"2026-07-30","temperatureC":-16,"temperatur...
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8], [Date, Wed, 29 Jul 2026 16:08:55 GMT], [Server, Kestrel
                    ]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 389


Alternatively, run the following command at any time while `HttpRepl` is running:
connect https://localhost:5078


PS D:\Project\code\dotNet> curl http://localhost:5078/pizza/1
StatusCode        : 200
StatusDescription : OK
Content           : {"id":1,"name":"Classic Italian","isGlutenFree":false}
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Sun, 02 Aug 2026 08:59:30 GMT
                    Server: Kestrel

                    {"id":1,"name":"Classic Italian","isGlutenFree":fals...
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8], [Date, Sun, 02 Aug 202
                    6 08:59:30 GMT], [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 54


PS D:\Project\code\dotNet> curl http://localhost:5078/pizza/2


StatusCode        : 200
StatusDescription : OK
Content           : {"id":2,"name":"Veggie","isGlutenFree":true}
RawContent        : HTTP/1.1 200 OK
                    Transfer-Encoding: chunked
                    Content-Type: application/json; charset=utf-8
                    Date: Sun, 02 Aug 2026 09:01:23 GMT
                    Server: Kestrel

                    {"id":2,"name":"Veggie","isGlutenFree":true}
Forms             : {}
Headers           : {[Transfer-Encoding, chunked], [Content-Type, application/json; charset=utf-8], [Date, Sun, 02 Aug 202
                    6 09:01:23 GMT], [Server, Kestrel]}
Images            : {}
InputFields       : {}
Links             : {}
ParsedHtml        : mshtml.HTMLDocumentClass
RawContentLength  : 44


# Create web APIs with ASP.NET Core -- Troubleshooting
https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0


**QUIZ**
Q1: What is the purpose of the [ApiController] attribute?
A1: This attribute includes several opinionated API-specific behaviors, such as automatic handling for bad HTTP requests. It other words [ApiController] enables opinionated behaviors that make it easier to build web APIs.

Q2: Suppose you need to update a product's name. Which HTTP action verb is the best fit for this request?
A2: The `PUT` verb is intended for use in modifying an existing product. Additionally, `PATCH` verb can be used to update individual properties.

Q3: In which scenario is it most appropriate to return an HTTP 404 status code, and how is it accomplished in ASP.Net Core?
A3: The `NotFound` method generates an HTTP 404 status code in the response. Using this status code is the best way to communicate that the request data doesn't exist.