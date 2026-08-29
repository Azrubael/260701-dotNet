

string value = "abc123";
char[] valueArray = value.ToCharArray();
Array.Reverse(valueArray);
string result1 = new(valueArray);
Console.WriteLine(result1);

string result2 = string.Join(",", valueArray);
Console.WriteLine(result2);

string[] items = result2.Split(',');
foreach (string item in items)
{
    Console.WriteLine(item);
}

///
string orderStream = "B123,C234,A345,C15,B177,G3003,C235,B179";
Console.WriteLine("Parse a string of orders, sort the orders and tag possible errors:");
Console.WriteLine(orderStream);
string[] orders = orderStream.Split(',');
foreach(string order in orders)
{
    char[] charsArray = order.ToCharArray();
    int digitsCounter = 0;
    bool firstLetter = false;
    for ( int i = 0; i < charsArray.Length; i++ )
    {
        if (charsArray[0] >= 'A' && charsArray[0] <= 'Z') firstLetter = true;
        if (char.IsDigit(charsArray[i])) digitsCounter++;
    }
    Console.WriteLine( (digitsCounter == 3 && firstLetter == true)
                        ? order
                        : $"{order}\t-- Error");
}