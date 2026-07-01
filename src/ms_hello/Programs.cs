using System;
namespace TourOfCsharp;

class Program
{
    static void Main()
    {
        Console.Write("Enter Your name: ");
        var name = Console.ReadLine();
        Console.WriteLine($"Hello fom MicroSoft, {name}!\nDo You like out platform .Net?");
        _ = Console.ReadLine();
    }
}