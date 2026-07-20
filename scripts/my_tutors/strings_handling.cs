#!/usr/bin/env dotnet run

string word = "Hello", exclam = "!!!";

Console.WriteLine(word.Length);
string word111 = string.Concat(word, exclam);
Console.WriteLine(string.Compare(word, word111));

string people = "June,Jane,Jack,George";
string[] names = people.Split([',']);
people = string.Join(" + ", names);
Console.WriteLine("=== {0} ===", people);

// Print without the first two letters
Console.WriteLine(people[2..]);

// Print without the last two letters
Console.WriteLine(people[0..(people.Length-2)]);
