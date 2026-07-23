## Cкладання проекту в малий бінарник Windows
```csharp
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```

## Запуск проекту в режимі наладки з ключами
```csharp
dotnet run --configuration Debug -- -S -l
```

# Пояснення параметрів:
    Usage: ls [OPTION]... [FILE]...
    List information about the FILEs (the current directory by default).

    Options:
      -a, --all       do not ignore entries starting with '.'
      -D              recursively count directories' sizes (works only with '-l')
      -l, --long      use a long listing format
      -h              human readable sizes
      -r, --r         use a reverse sorting order
      -S              sort by size
      -T              sort by time
      -?, --help      display this help and exit


PS D:\src\utility_ls> dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
Restore complete (1,1s)
  utility_ls net10.0 win-x64 succeeded (5,3s) → bin\Release\net10.0\win-x64\publish\

Build succeeded in 6,8s


