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
