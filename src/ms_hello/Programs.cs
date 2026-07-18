namespace ms_hello;

class Program
{
    static void Main()
    {
        Console.Write("Enter Your name: ");
        var name = Console.ReadLine();
        Console.WriteLine($"Hello fom MicroSoft, {name}!\nDo You like out platform .Net?");
        Console.WriteLine("Enter any number:");
        string? user_input = Console.ReadLine();
        float float_input = 0;
        if (user_input != null)
        { // To parse into float we need to check if the input is`t null
            float_input = float.Parse(user_input);
        }
        // Converting into a double value works safer
        double double_input = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Parsed to float: {0}.\nConverted to double: {1}.", float_input, double_input);

    }
}