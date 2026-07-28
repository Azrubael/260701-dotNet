using System.Diagnostics;


static void SayHelloEn() => Console.WriteLine("Hello!");

static void SayHelloFr() => Console.WriteLine("Salut!");

static void SayHelloUa() => Console.WriteLine("Привіт!");

static int Fibonacci(int n)
{
	int n1 = 0;
	int n2 = 1;
	int sum;

	Debug.WriteLine($"Entering {nameof(Fibonacci)} method");
	Debug.WriteLine($"We are looking for the {n}th number");
	for (int i = 2; i < n; i++)
	{
		sum = n1 + n2;
		n1 = n2;
		n2 = sum;
		Debug.WriteLineIf(sum == 1, $"sum is 1, n1 is {n1}, n2 is {n2}");
		// If n2 is 5 continue, else break.
		Debug.Assert(n2 == 5, "The return value is not 5 and it should be.");
	}

	return n == 0 ? n1 : n2;
}


Console.Write("Enter Your name: ");
var name = Console.ReadLine();
Console.Write($"{name}, enter Your language (en/fr/ua) or quit (q): ");
string language;
do
{
	language = Console.ReadLine();
	switch (language)
	{
		case "en":
			SayHelloEn(); break;
		case "fr":
			SayHelloFr(); break;
		case "ua":
			SayHelloUa(); break;
		case "q":
			break;
	}
}
while (language != "q");

int result = Fibonacci(5);
Console.WriteLine(result);

