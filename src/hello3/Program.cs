namespace hello3
{
  class Program
  {
    static void Main()
    {
      for (int i = 0; i < 10; i++)
      {
      Console.WriteLine($"Hello, World {i*i}!");
      }
      Console.ReadKey();
    }
  }
}