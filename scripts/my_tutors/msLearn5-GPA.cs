/*
This C# console application is designed to:
- Use arrays to store student names and assignment scores.
- Use a `foreach` statement to iterate through the student names as an outer program loop.
- Use an `if` statement within the outer loop to identify the current student name and access that student's assignment scores.
- Use a `foreach` statement within the outer loop to iterate though the assignment scores array and sum the values.
- Use an algorithm within the outer loop to calculate the average exam score for each student.
- Use an `if-elseif-else` construct within the outer loop to evaluate the average exam score and assign a letter grade automatically.
- Integrate extra credit scores when calculating the student's final score and letter grade as follows:
    - detects extra credit assignments based on the number of elements in the student's scores array.
    - divides the values of extra credit assignments by 10 before adding extra credit scores to the sum of exam scores.
- use the following report format to report student grades:

    Student         Grade

    Sophia:         92.2    A-
    Andrew:         89.6    B+
    Emma:           85.6    B
    Logan:          91.2    A-
*/


int examAssignments = 5;

string[] studentNames = ["Sophia", "Andrew", "Emma", "Logan"];

int[] sophiaScores = [90, 86, 87, 98, 100, 94, 90];
int[] andrewScores = [92, 89, 81, 96, 90, 89];
int[] emmaScores = [90, 85, 87, 98, 68, 89, 89, 89];
int[] loganScores = [90, 95, 87, 88, 96, 96];

Console.Clear();
Console.WriteLine($"{"Student",-12}{"Exam score",-14}" +
    $"{"Overall",-12}{"Grade",-12}{"Extra credit"}");

decimal currentStudentGrade;
decimal currentStudentExamScore;
int sumAssignmentScores;
int gradedAssignments;
int examScore;
int extraScore;
decimal extraCredit;
int sumExtraScores;
int extraPts;
decimal extraCreditPts;

foreach (string name in studentNames)
{
    string currentStudent = name;

    int[] studentScores = currentStudent switch
    {
        "Sophia" => sophiaScores,
        "Andrew" => andrewScores,
        "Emma" => emmaScores,
        "Logan" => loganScores,
        _ => [10],
    };

    sumAssignmentScores = 0;
    gradedAssignments = 0;
    examScore = 0;
    extraScore = 0;
    extraPts = 0;
    sumExtraScores = 0;

    foreach (int score in studentScores)
    {
        gradedAssignments++;

        if (gradedAssignments <= examAssignments)
        {
            sumAssignmentScores += score;
            examScore += score;
        }
        else
        {
            extraPts += score ;
            sumAssignmentScores += score / 10;
            sumExtraScores += score;
            extraScore++;
        }
    }

    currentStudentGrade = (decimal)sumAssignmentScores / examAssignments;
    currentStudentExamScore = (decimal)examScore / examAssignments;
    extraCredit = (decimal)sumExtraScores / extraScore;
    extraCreditPts = (decimal)extraPts / 10 / examAssignments;

    string? currentStudentLetterGrade = currentStudentGrade switch
    {
        >= 97 => "A+",
        >= 93 => "A",
        >= 90 => "A-",
        >= 87 => "B+",
        >= 83 => "B",
        >= 80 => "B-",
        >= 77 => "C+",
        >= 73 => "C",
        >= 70 => "C-",
        >= 67 => "D+",
        >= 63 => "D",
        >= 60 => "D-",
        _ => "F"
    };

    // Sophia:         92.2    A-
    Console.WriteLine($"{currentStudent,-12}{currentStudentExamScore,-14}" +
        $"{currentStudentGrade,-12}{currentStudentLetterGrade,-12}" +
        $"{extraCredit,-4} ({extraCreditPts:0.00} pts)");
}

Console.WriteLine("\n\rPress the Enter key to continue");
Console.ReadLine();
