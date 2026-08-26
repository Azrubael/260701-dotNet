namespace mslearn11_TypeConversion;

using System.Globalization;
using System.Text;

class Program
{
  static void Main()
  {
    CultureInfo.CurrentCulture = new CultureInfo("en-US");
    string sep = "\t******************************************\n";

    int first = 2;
    string second = "4";
    string result = first + second;
    Console.WriteLine(result);

    Console.WriteLine("Hello, World!");

    Console.WriteLine($"\t(1) {sep}");
    decimal myDecimal = 3.14m;
    Console.WriteLine($"decimal: {myDecimal}");

    int myInt = (int)myDecimal;
    Console.WriteLine($"int: {myInt}");

    Console.WriteLine($"\t(2) {sep}");
    decimal myDecim = 1.23456789m;
    Console.WriteLine($"decimal: {myDecim}");

    int myInteger = (int)myDecim;
    Console.WriteLine($"int: {myInteger}");

    Console.WriteLine($"\t(3) {sep}");
    int перший = 5;
    int другий = 7;
    string message = перший.ToString() + другий.ToString();
    Console.WriteLine(message);

    Console.WriteLine($"\t(4) {sep}");
    string третій = "5";
    string четвертий = "7";
    int msg1 = int.Parse(третій) + int.Parse(четвертий);
    Console.WriteLine(msg1);
    if (int.TryParse(третій, out int a) && int.TryParse(четвертий, out int b))
    {
      Console.WriteLine(a + b);
    }

    Console.WriteLine($"\t(5) {sep}");
    int value = (int)1.5m; // casting truncates
    Console.WriteLine(value);

    int value2 = Convert.ToInt32(1.5m); // converting rounds up
    Console.WriteLine(value2);

    Console.WriteLine($"\t(6) {sep}");
    string value3 = "102";
    if (int.TryParse(value3, out int result3))
    {
      Console.WriteLine($"Measurement: {result3}");
    }
    else
    {
      Console.WriteLine("Unable to report the measurement.");
    }
    Console.WriteLine($"Measurement (w/ offset): {50 + result3}");

    Console.WriteLine($"\t(7) {sep}");
    string value4 = "bad";
    if (int.TryParse(value4, out int result4))
    {
      Console.WriteLine($"Measurement: {result}");
    }
    else
    {
      Console.WriteLine("Unable to report the measurement.");
    }

    if (result4 > 0)
      Console.WriteLine($"Measurement (w/ offset): {50 + result4}");

    Console.WriteLine($"\t(8) {sep}");
    string[] values = { "12.3", "45", "ABC", "11", "DEF" };
    string msg = string.Empty;
    double total = 0d;
    foreach (string el in values)
    {
      if (double.TryParse(el, out double num))
      {
        total += num;
      }
      else
      {
        msg = string.Concat(msg, el);
      }
    }
    Console.WriteLine($"Message: {msg}");
    Console.WriteLine($"Total: {total}");

    Console.WriteLine($"\t(9) {sep}");
    var message9 = new StringBuilder();
    double total9 = 0d;
    string[] values9 = { "12.3", "45", "ABC", "11", "DEF" };

    Array.ForEach(values9, el =>
    {
      if (double.TryParse(el, out var num))
        total9 += num;
      else
        message9.Append(el);
    });

    string msg9 = message9.ToString();
    Console.WriteLine($"Message: {msg9}");
    Console.WriteLine($"Total: {total9}");

    Console.WriteLine($"\t(10) {sep}");
    Array.ForEach(values, Console.WriteLine);


    Console.WriteLine($"\t(11) {sep}");
    int vvalue1 = 11;
    decimal vvalue2 = 6.2m;
    float vvalue3 = 4.3f;

    // Your code here to set result1
    // Hint: You need to round the result to nearest integer (don't just truncate)
    int result1 = Convert.ToInt32(vvalue1 / vvalue2);
    Console.WriteLine($"Divide value1 by value2, display the result as an int: {result1}");

    // Your code here to set result2
    decimal result2 = vvalue2 / (decimal)vvalue3;
    Console.WriteLine($"Divide value2 by value3, display the result as a decimal: {result2}");

    // Your code here to set result3
    float rresult3 = vvalue3 / vvalue1;
    Console.WriteLine($"Divide value3 by value1, display the result as a float: {rresult3}");

  }


}


