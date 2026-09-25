const string input = "<div><h2>Widgets &trade;</h2><span>5000</span></div>";

/*  Desired output
 *	Quantity: 5000
 *	Output: <h2>Widgets &reg;</h2><span>5000</span>
 */


Console.WriteLine(Quote(input, "span"));
Console.WriteLine(Quote(input, "div"));


string Quote(string input, string tag)
{
	string startTag = $"<{tag}>";
	string endTag = $"</{tag}>";
	int openingPosition = input.IndexOf(startTag) + startTag.Length;
	int closingPosition = input.IndexOf(endTag);
	int length = closingPosition - openingPosition;
	return input.Substring(openingPosition, length);
}
