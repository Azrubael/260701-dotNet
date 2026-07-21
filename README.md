#### 2026-07-01
The tutorial repository that I've created to study .Net technologies.
Today my main goal is to get acquainted with tools for working with the Azure cloud enviroment.
Main project types: Console / ASP.NET / WebAPI / Blazor

#### 2026-07-18
```pwsh
cd ./dotNet
dotnet run --project src/az_planner --test0
dotnet run --project src/hello -v d
dotnet list package
```

## Cкладання проекту в малий бінарник Windows
```csharp
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```

# Пояснення параметрів:
    -c Release — збірка у режимі релізу (оптимізовано для швидкості та розміру).
    -r win-x64 — збірка для 64-бітної Windows.
    --self-contained true — включає всі необхідні бібліотеки у вихідний файл (не потрібно встановлювати .NET на цільовій машині).
    -p:PublishSingleFile=true — збирає усе у один .exe файл.
    -p:PublishTrimmed=true — видаляє зайвий код для зменшення розміру.


PS D:\src\ls_utility> dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
Restore complete (1,8s)
  ls_utility net10.0 win-x64 succeeded (6,2s) → bin\Release\net10.0\win-x64\publish\

Build succeeded in 8,4s

NuGet — это менеджер пакетов для .NET. Он позволяет разработчику подключать в проект готовые библиотеки (пакеты) из “репозитория” (обычно nuget.org) и автоматически подтягивать нужные зависимости, версии и обновления.

### 1) Подключение пакета
Вариант A (через файл проекта `*.csproj`):
```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```

Вариант B (через команду):
```powershell
dotnet add package Newtonsoft.Json --version 13.0.3
```
После этого пакет становится доступен в коде через `using ...` (если нужно пространство имён из библиотеки).

### 2) Установка через restore
Когда вы собираете проект, выполняется “restore” пакетов (скачивание/обновление зависимостей) — обычно автоматически:
```powershell
dotnet restore
dotnet build
```

### 3) Обновление версий
```powershell
dotnet list package
dotnet outdated
```
(последняя команда может требовать установленного расширения/обновление tooling, чтоб увидеть устаревшие пакеты)

### 4) Сборка и использование
В зависимости от типа проекта вы добавляете/удаляете зависимости в `csproj`, а затем:
- `dotnet build` компилирует с учётом NuGet-зависимостей
- `dotnet run` запускает собранное приложение
- `dotnet test` запускает тесты с нужными пакетами

## Что хранится в проекте
Обычно вы увидите:
- `*.csproj` — список `PackageReference`
- `packages.lock.json` (если включали lock-файлы)
- `bin/` и `obj/` — сгенерированные файлы/кэш сборки
- глобальный кэш пакетов у пользователя (не храните пакеты вручную)

## Локальные/частные репозитории
Иногда пакеты не из nuget.org, а из вашего корпоративного feed:
- добавляется как `NuGet.config`
- или указывается source при `dotnet restore`
