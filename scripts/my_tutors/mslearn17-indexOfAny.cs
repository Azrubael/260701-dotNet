string msg = "Hello, world!";
char[] charsToFind = ['a', 'e', 'i'];

int idx = msg.IndexOfAny(charsToFind);
Console.WriteLine($"Found '{msg[idx]}' in '{msg}' at index: {idx}.");

////////////////////////////////////////////////////////

string message = "Help (find) the {opening symbols}";
Console.WriteLine($"Searching THIS Message: {message}");
char[] opnSymbols = ['[', '{', '('];
int startPosition = message.IndexOfAny(charsToFind);
int openingPosition = message.IndexOfAny(opnSymbols);
Console.WriteLine($"Found WITHOUT using startPosition: {message[openingPosition..]}");

openingPosition = message.IndexOfAny(opnSymbols, startPosition);
Console.WriteLine($"Found WITH using startPosition {startPosition}:  {message[openingPosition..]}");


//////////////////////////////////////////////////////

message = "(What if) I have [different symbols] but every {open symbol} needs a [matching closing symbol]?";
int closingPosition = 0;
while (true)
{
  int opnPos = message.IndexOfAny(opnSymbols, closingPosition);
  if (opnPos == -1) break;
  string currentSymbol = message.Substring(opnPos, 1);
  char matchingSymbol = ' ';
  switch (currentSymbol)
  {
    case "[":
      matchingSymbol = ']';
      break;
    case "{":
      matchingSymbol = '}';
      break;
    case "(":
      matchingSymbol = ')';
      break;
  }
  opnPos += 1;
  closingPosition = message.IndexOf(matchingSymbol, opnPos);

  int length = closingPosition - opnPos;
  Console.WriteLine(message.AsSpan(opnPos, length));
}

//////////////////////////////////////////////////////////

message = "Age: 25";
int colon = message.IndexOf(':');

if (int.TryParse(message.AsSpan(colon + 1), out int age))
{
    Console.WriteLine(age);
}

/////////////////////////////////////////////////////////
string message = "This--is--ex-amp-le--da-ta";
message = message.Replace("--", " ");
message = message.Replace("-", "");
Console.WriteLine(message);