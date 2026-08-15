// The simple flip coin app

bool quitOuter = false;
bool continueInner = true;
int roll;

Random dice = new();

do
{
  do
  {
    Console.Clear();
    Console.WriteLine("Do you want to flip a coin ? (y/n) :");
    char anyKey = Console.ReadKey(intercept: true).KeyChar;
    char c = char.ToLower(anyKey);
    if (c == 'n')
    {
      quitOuter = true;
      continueInner = false;
    }
    else
    {
      if (c == 'y')
      {
        continueInner = true;
        roll = dice.Next(2);
        Console.WriteLine(roll == 1 ? "Obverse!" : "Reverse!");
      }
      else
      {
        Console.WriteLine("\nPlease make the correct choise");
      }
      Console.Write("Press any key to continue.");
      Console.ReadKey();
    }
  } while (continueInner);

}
while (!quitOuter);