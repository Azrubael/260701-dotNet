using System.ComponentModel;

string message = "What is the value <span>between the tags</span>?";

string tag = "span";
string startTag = $"<{tag}>";
string endTag = $"</{tag}>";
int openingPosition = message.IndexOf(startTag) + startTag.Length;
int closingPosition = message.IndexOf(endTag);

int length = closingPosition - openingPosition;
Console.WriteLine(message.Substring(openingPosition, length));

//////////////////////////////////////////////////////

string message2 = "hello there!";
int first_h = message2.IndexOf('h');
int last_h = message2.LastIndexOf('h');

Console.WriteLine($"For the message: '{message2}', the first 'h' is at position {first_h} and the last 'h' is at position {last_h}.");

//////////////////////////////////////////////////////

string msg = "(What if) there are (more than) one (set of parentheses)?";
while (true)
{
	int opnPos = msg.IndexOf('(');
	if (opnPos == -1 ) break;

	opnPos += 1;
	int closPos = msg.IndexOf(')');
	length = closPos - opnPos;
	Console.WriteLine(msg.AsSpan(opnPos, length));

	msg = msg[(closPos + 1)..];
}
