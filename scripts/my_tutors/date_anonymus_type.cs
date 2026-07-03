// PS D:\my_tutors> dotnet run date_anonymus_type.cs
// Ticks: 639185744824596640, formatted: лип. 02, 2026 07:34 +00:00
// Ticks: 639185780824597248, formatted: лип. 02, 2026 08:34 +00:00
// Ticks: 639185816824597249, formatted: лип. 02, 2026 09:34 +00:00

var dates = new[]
{
    DateTime.UtcNow.AddHours(-1),
    DateTime.UtcNow,
    DateTime.UtcNow.AddHours(1),
};

foreach (var anonymous in
             dates.Select(
                 date => new { Formatted = $"{date:MMM dd, yyyy hh:mm zzz}", date.Ticks }))
{
    Console.WriteLine($"Ticks: {anonymous.Ticks}, formatted: {anonymous.Formatted}");
}