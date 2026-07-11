#r "nuget: Giraffe"
#r "nuget: Microsoft.AspNetCore.Hosting"
#r "nuget: Microsoft.AspNetCore.Builder"
#r "nuget: Microsoft.Extensions.Hosting"

open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting

// Обробник запиту
let webApp =
    choose [
        route "/" >=> text "Привіт, світ з F#!"
        route "/hello" >=> text "Це приклад веб-сервісу на F#"
    ]

// Запуск сервера
[<EntryPoint>]
let main _ =
    printfn "Привіт, це F# скріпт для веб застосунку на базі Giraffe!"
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHost ->
            webHost
                .UseUrls("http://localhost:5000") // задаємо порт
                .Configure(fun app ->
                app.UseGiraffe webApp))
        .Build()
        .Run()
    0

(* Giraffe використовується для маршрутизації HTTP‑запитів.
 * При зверненні до / повертається текст "Привіт, світ з F#!".
 * При зверненні до /hello повертається інший текст.
 * Код запускає простий веб‑сервер на ASP.NET Core, як вказано нижче:
 * PS d:\ dotnet new web -lang F# -n WebApp0
 * PS d:\ cd WebApp0
 * PS d:\ dotnet add package Giraffe
 * PS d:\ dotnet run
 НО!!! Если все-таки хочется максимально гладкого старта и большого числа готовых решений/примеров, то чаще выбирают обычный ASP.NET Core или более высокоуровневые F#-альтернативы вроде Saturn/Falco/Oxpecker. 
 *) 
