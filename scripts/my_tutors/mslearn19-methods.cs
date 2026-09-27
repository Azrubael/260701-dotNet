// #1
int[] schedule = [ 800, 1200, 1600, 2000 ];
DisplayAdjustedTimes(schedule, 6, -6);
Console.WriteLine("*************************\n\n");

// #2
string[] students = ["Jenna", "Ayesha", "Carlos", "Viktor"];

DisplayStudents(students);
DisplayStudents(["Robert","Vanya"]);
Console.WriteLine("*************************\n\n");

// #3
GetCircleArea(12);
GetCircleCircumference(12);
Console.WriteLine("*************************\n\n");


// #4
string status = "Healthy";

Console.WriteLine($"Start: {status}");
SetHealth(false);
Console.WriteLine($"End: {status}");


/// Adjusts scheduled times to a different GMT time zone
static void DisplayAdjustedTimes(int[] times, int currentGMT, int newGMT)
{
  int diff = 0;
  if (Math.Abs(newGMT) > 12 || Math.Abs(currentGMT) > 12)
    Console.WriteLine("Invalid GMT");
  else if (newGMT <= 0 && currentGMT <= 0 || newGMT >= 0 && currentGMT >= 0)
    diff = 100 * (Math.Abs(newGMT) - Math.Abs(currentGMT));
  else
    diff = 100 * (Math.Abs(newGMT) + Math.Abs(currentGMT));

  foreach( int time in times)
    Console.WriteLine($"{time} -> {(time + diff) % 2400}");
}


/// Prints out the strings one by one in a row
static void DisplayStudents(string[] students)
{
    foreach (string student in students)
    {
        Console.Write($"{student}, ");
    }
    Console.WriteLine();
}


/// Calculates the circle's area
static void GetCircleArea(int radius)
{
    double area = Math.PI * (radius * radius);
    Console.WriteLine($"Area = {area}");
}


/// Calculates the circle's Circumference
static void GetCircleCircumference(int radius)
{
    double circumference = 2 * Math.PI * radius;
    Console.WriteLine($"Circumference = {circumference}");
}


/// Set the string with the bool value
void SetHealth(bool isHealthy)
{
    status = (isHealthy ? "Healthy" : "Unhealthy");
    Console.WriteLine($"Middle: {status}");
}