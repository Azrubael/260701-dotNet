{ // The first lesson about methods
  Random dice = new();
  int roll1 = dice.Next();
  int roll2 = dice.Next(101);
  int roll3 = dice.Next(50, 101);

  Console.WriteLine($"First roll: {roll1}");
  Console.WriteLine($"Second roll: {roll2}");
  Console.WriteLine($"Third roll: {roll3}");

  int firstValue = Max(roll1, roll2);
  int secondValue = Max(roll2, roll3);
  int largerValue = Max(firstValue, secondValue);

  firstValue = Math.Min(roll1, roll2);
  secondValue = Math.Min(roll2, roll3);
  var smallerValue = Math.Min(firstValue, secondValue);

  Console.WriteLine($"The maximum: {largerValue}");
  Console.WriteLine($"The minimum: {smallerValue}");
  Console.WriteLine("====================");

  static T Max<T>(T a, T b) where T : IComparable<T>
      => a.CompareTo(b) <= 0 ? b : a;
}

{ // The second lesson about if/else
  Random dice2 = new();

  int roll11 = dice2.Next(1, 7);
  int roll12 = dice2.Next(1, 7);
  int roll13 = dice2.Next(1, 7);

  int total = roll11 + roll12 + roll13;


  if ((roll11 == roll12) || (roll12 == roll13) || (roll11 == roll13))
  {

    if ((roll11 == roll12) && (roll12 == roll13))
    {
      Console.WriteLine("You rolled triples! +6 bonus to total!");
      total += 6;
    }
    else
    {
      Console.WriteLine("You rolled doubles! +2 bonus to total!");
      total += 2;
    }
  }


  Console.WriteLine($"Dice2 roll: {roll11} + {roll12} + {roll13} = {total}");
  if (total >= 15) Console.WriteLine("You win!");
  else Console.WriteLine("Sorry, you lose.");
  Console.WriteLine("====================");
}

{ // The therd lesson "if / else / else if"
  Random random = new();
int daysUntilExpiration = random.Next(12);
int discountPercentage = 0;

if (daysUntilExpiration == 0)
{
    Console.WriteLine("Your subscription has expired.");
}
else if (daysUntilExpiration == 1)
{
    Console.WriteLine("Your subscription expires within a day!");
    discountPercentage = 20;
}
else if (daysUntilExpiration <= 5)
{
    Console.WriteLine($"Your subscription expires in {daysUntilExpiration} days.");
    discountPercentage = 10;
}
else if (daysUntilExpiration <= 10)
{
    Console.WriteLine("Your subscription will expire soon. Renew now!");
}

if (discountPercentage > 0)
{
    Console.WriteLine($"Renew now and save {discountPercentage}%.");
}
}