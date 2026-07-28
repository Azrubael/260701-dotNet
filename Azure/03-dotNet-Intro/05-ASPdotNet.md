### 2026-07-27
--------------


**REST: A common pattern for building APIs with HTTP**
Representational State Transfer (REST) is an architectural style for building web services. REST requests are made over HTTP. They use the same HTTP verbs that web browsers use to retrieve webpages and send data to servers.

`GET`: Retrieve data from the web service.
`POST`: Create a new item of data on the web service.
`PUT`: Update an item of data on the web service.
`PATCH`: Update an item of data on the web service by describing a set of instructions about how the item should be modified. The sample application in this module doesn't use this verb.
`DELETE`: Delete an item of data on the web service.

Web service APIs that adhere to REST are called *RESTful APIs*.
They're defined through:
- A base URI.
- HTTP methods, such as GET, POST, PUT, PATCH, or DELETE.
- A media type for the data, such as JavaScript Object Notation (JSON) or XML.

We use routing to map URIs (uniform resource identifiers) to logical divisions in our code to tha address: https://localhost:5000/

With ASP.NET, you can use the same framework and patterns to build both webpages and services. You can reuse model classes and validation logic. This approach has benefits:
- Simple serialization
- Authentication and authorization as industry-standard JSON Web Tokens (JWTs)
- Routing alongside your code
- HTTPS by default. ASP.NET provides support for HTTPS out of the box. It automatically generates a test certificate and easily imports it to enable local HTTPS
	
.NET HTTP `REPL` (read-evaluate-print loop) -- It's a simple and popular way to build interactive command-line environments instead of  view and test your work in a web browser.

**QUIZ**
Qiestion: Which of the following use cases is not a reason to build a web API by using ASP.NET Core?
Answer: To serve a traditional HTML-based web application.

Web API projects are secured with https by default. If you have problems, configure the ASP.NET Core HTTPS development certificate:
https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos

dotnet new webapi -controllers -f net10.0

PS D:\Project\code\dotNet\src\mslearnContosoPizza> type mslearnContosoPizza.csproj
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

PS D:\Project\code\dotNet\src\mslearnContosoPizza> tree
D:.
├───Controllers
├───obj
└───Properties

Controllers/ 			Contains classes with public methods exposed as HTTP endpoints.
Program.cs 				Configures services and the app's HTTP request pipeline, and contains the app's managed entry point.
ContosoPizza.csproj 	Contains configuration metadata for the project.
ContosoPizza.http 		Contains configuration to test REST APIs directly from Visual Studio Code.

