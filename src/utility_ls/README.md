## Cкладання проекту в малий бінарник Windows
```csharp
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```

## Запуск проекту в режимі наладки з ключами
```csharp
dotnet run --configuration Debug -- -S -l
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

## 2026-07-21
-S -- sort by size
-r, --r -- use reverse sorting order
-h -- human readable sizes
-D -- calculate directories sizes recursively


1) **-T -- sort by time**
2) потрібно додати радок підсумкової інформації в розгорнутому вигляді.
3) Потрібно зробити сортировку директорій після визначення іхнього розміру