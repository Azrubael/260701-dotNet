namespace mslearn11_TypeConversion;

using System.Globalization;

class Program
{
  static void Main()
  {
    CultureInfo.CurrentCulture = new CultureInfo("en-US");

    int first = 2;
    string second = "4";
    string result = first + second;
    Console.WriteLine(result);

    Console.WriteLine("Hello, World!");
  }
}


