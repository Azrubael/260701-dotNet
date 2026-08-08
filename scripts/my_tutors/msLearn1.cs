string[] names = ["Sophia", "Nicolas", "Zakhariah", "Jeong"];
int[][] grades = [
  [93, 87, 98, 95, 100],
  [80, 83, 82, 88, 85],
  [84, 96, 73, 85, 79],
  [90, 92, 98, 100, 97]
];

int studentsNumber = grades.Length;
StudentGrades[] studentGrades = new StudentGrades[studentsNumber];

Console.WriteLine("Student     Grade");
for (int i = 0; i < studentsNumber; i++)
{
  studentGrades[i] = new StudentGrades
  {
    Name = names[i],
    Grades = grades[i]
  };
  studentGrades[i].SetAvg();
  studentGrades[i].Print();
}


class StudentGrades
{
  public string Name { get; set; } = "";
  public int[] Grades { get; set; } = [];

  public float AvgGrade { get; private set; }
  public char AvgLetter { get; private set; }

  public void SetAvg()
  {
    float sum = 0;
    foreach (int grade in Grades) sum += grade;

    AvgGrade = sum / Grades.Length;

    AvgLetter = AvgGrade switch
    {
      >= 90 => 'A',
      >= 80 => 'B',
      >= 70 => 'C',
      >= 60 => 'D',
      _ => 'F',
    };
  }

  public void Print()
  {
    Console.WriteLine($"{Name,-12} {AvgLetter} ({AvgGrade:0.00})");
  }
}