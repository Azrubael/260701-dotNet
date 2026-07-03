#!/usr/bin/env dotnet run
// The typical command to run this script
// PS d:\> dotnet run hello_world.cs

var a = 1;
var t = (a, b: 2, 3);
Console.WriteLine($"The 1st element is {t.Item1} (same as {t.a}).");
Console.WriteLine($"The 2nd element is {t.Item2} (same as {t.b}).");
Console.WriteLine($"The 3rd element is {t.Item3}.");
/* Output:
 * The 1st element is 1 (same as 1).
 * The 2nd element is 2 (same as 2).
 * The 3rd element is 3.
 */

/* Beginning with C# 12, you can specify an alias for a tuple type with a using directive
 * After declaring the alias, you can use the BandPass name as an alias for that tuple type
 * An alias doesn't introduce a new type, but only creates a synonym for an existing type
 */
global using BandPass = (int Min, int Max);
BandPass bracket = (40, 100);
Console.WriteLine($"The bandpass filter is {bracket.Min} to {bracket.Max}");
(int a , int b) = bracket;
Console.WriteLine($"The bracket is {a} to {b}");

int[] xs = new int[] { 4, 7, 9 };
var limits = FindMinMax(xs);
Console.WriteLine($"Limits of [{string.Join(" ", xs)}] are {limits.min} and {limits.max}");
// Output:
// Limits of [4 7 9] are 4 and 9

int[] ys = new int[] { -9, 0, 67, 100 };
var (minimum, maximum) = FindMinMax(ys);
Console.WriteLine($"Limits of [{string.Join(" ", ys)}] are {minimum} and {maximum}");
// Output:
// Limits of [-9 0 67 100] are -9 and 100

(int min, int max) FindMinMax(int[] input)
{
    if (input is null || input.Length == 0)
    {
        throw new ArgumentException("Cannot find minimum and maximum of a null or empty array.");
    }

    // Initialize min to MaxValue so every value in the input
    // is less than this initial value.
    var min = int.MaxValue;
    // Initialize max to MinValue so every value in the input
    // is greater than this initial value.
    var max = int.MinValue;
    foreach (var i in input)
    {
        if (i < min)
        {
            min = i;
        }
        if (i > max)
        {
            max = i;
        }
    }
    return (min, max);
}