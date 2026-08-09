// Grade Point Average Calculator

namespace msLearn2_GPA;

class Program
{
  private readonly (string key, int grade, int hours)[] initData =
  {
    ("English 101", 4, 3),
    ("Algebra 101", 3, 3),
    ("Biology 101", 3, 4),
    ("Computer Science I", 3, 4),
    ("Psychology 101", 4, 3)
  };

  static void Main()
  {
    var init = new Program();

    var sophiaList = new List<SubjectData>();

    foreach (var (key, grade, hours) in init.initData)
    {
      sophiaList.Add(new SubjectData(
        key,
        grade,
        hours,
        SetLetter(grade),
        SetGradePoints(grade, hours)
      ));
    }

    SubjectData[] Sophia = [.. sophiaList];

    int totalCreditPoints = 0;
    int totalCreditHours = 0;

    foreach (var subj in Sophia)
    {
      Console.WriteLine($"{subj.Key,-20}{subj.Grade,-7}{subj.Hours}");
      totalCreditHours += subj.Hours;
      totalCreditPoints += subj.GradePoints;
    }

    Console.WriteLine($"Total:              {totalCreditPoints,-7}{totalCreditHours}");
    float avgPoints = (float)totalCreditPoints / Sophia.Length;
    float avgHours = (float)totalCreditHours / Sophia.Length;
    decimal gradePointAverage = (decimal) totalCreditPoints / totalCreditHours;

    Console.WriteLine($"Average:            {avgPoints:0.00}  {avgHours:0.00}");
    Console.WriteLine($"Final GPA: {gradePointAverage:0.00}");

    decimal x = 7/5;
    Console.WriteLine(x);
  }


  public readonly record struct SubjectData(
    string Key,
    int Grade,
    int Hours,
    char GradeLetter,
    int GradePoints
  );


  public static char SetLetter(int grade)
  {
    return grade switch
    {
      4 => 'A',
      3 => 'B',
      2 => 'C',
      1 => 'D',
      _ => 'F',
    };
  }


  public static int SetGradePoints(int grade, int hours)
  {
    return grade * hours;
  }

}
