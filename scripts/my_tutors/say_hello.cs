void SayHelloEn() => Console.WriteLine("Hello!");

void SayHelloFr() => Console.WriteLine("Salut!");

void SayHelloUa() => Console.WriteLine("Привіт!");

Console.Write("Enter Your name: ");
var name = Console.ReadLine();
Console.Write($"{name}, enter Your language (en/fr/ua) or quit (q): ");
string language;
do {
	language = Console.ReadLine();
	switch (language) {
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