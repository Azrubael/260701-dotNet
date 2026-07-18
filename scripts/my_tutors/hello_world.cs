#!/usr/bin/env dotnet run

// The typical command to run this script
// PS d:\> dotnet run hello_world.cs

Console.Write("Enter Your name: ");
var name = Console.ReadLine();
Console.WriteLine($"Hello, {name}!\nDo You like .Net?");
_ = Console.ReadLine();

