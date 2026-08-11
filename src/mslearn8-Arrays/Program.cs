
string[] fraudulentOrderIDs = ["A123", "B456", "C789", "D000"];

Console.WriteLine($"There are {fraudulentOrderIDs.Length} fraudulent orders to process.");

foreach (string el in fraudulentOrderIDs)
{
    Console.Write(el + "\t");
}
Console.WriteLine();

int[] inventory = [200, 450, 700, 175, 250];
int totalSum = inventory.Sum(item => item);

int sum = 0;
foreach (var el in inventory)
{
  Console.Write(el + " + ");
  sum += el;
}

Console.WriteLine($"= {totalSum} or {sum}");

Console.WriteLine("-------------------------------------------------");
string[] orderIds = [ "B123", "C234", "A345", "C15", "B177", "G3003", "C235", "B179" ];

var potentiallyFraudulent = orderIds
    .Where(id => id.StartsWith('B'))
    .ToArray();

Console.WriteLine(string.Join(", ", potentiallyFraudulent));

Console.WriteLine("-------------------------------------------------");
Random dice = new();

int roll1 = dice.Next(1, 7);
int roll2 = dice.Next(1, 7);
int roll3 = dice.Next(1, 7);

int total = roll1 + roll2 + roll3;
Console.WriteLine($"Dice roll: {roll1} + {roll2} + {roll3} = {total}");

if ((roll1 == roll2) || (roll2 == roll3) || (roll1 == roll3))
{
    if ((roll1 == roll2) && (roll2 == roll3))
    {
        Console.WriteLine("You rolled triples!  +6 bonus to total!");
        total += 6;
    }
    else
    {
        Console.WriteLine("You rolled doubles!  +2 bonus to total!");
        total += 2;
    }
}


Console.WriteLine("-------------------------------------------------");

string originalMessage = "The quick brown fox jumps over the lazy dog.";
char[] message = originalMessage.ToCharArray();
Array.Reverse(message);
int letterCount = message.Count(letter => letter == 'o');
Console.WriteLine($"--= {letterCount} =--");