/*
Here are the FizzBuzz rules that you need to implement in your code project:
  Output values from 1 to 100, one number per line, inside the code block of an iteration statement.
  When the current value is divisible by 3, print the term Fizz next to the number.
  When the current value is divisible by 5, print the term Buzz next to the number.
  When the current value is divisible by both 3 and 5, print the term FizzBuzz next to the number.
*/

for (int i = 1; i <= 101; i++)
{
  string slogan = "";
  if (i % 3 == 0) slogan += "Fizz";
  if (i % 5 == 0) slogan += "Buzz";
  if (slogan == "") Console.WriteLine(i);
  else Console.WriteLine($"{i} -- {slogan}");
}


Console.WriteLine("-----------------------------------------");
Random random = new();
int current;

do
{
    current = random.Next(1, 11);
    Console.WriteLine(current);
} while (current != 7);

Console.WriteLine("-----------------------------------------");

do
{
    current = random.Next(1, 11);

    if (current >= 8) continue;

    Console.WriteLine(current);
} while (current != 7);

Console.WriteLine("-----------------------------------------");

string? readResult;
bool validEntry = false;
Console.WriteLine("Enter a string containing at least three characters:");
do
{
    readResult = Console.ReadLine();
    if (readResult != null)
    {
        if (readResult.Length >= 3)
        {
            validEntry = true;
        }
        else
        {
            Console.WriteLine("Your input is invalid, please try again.");
        }
    }
} while (validEntry == false);