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