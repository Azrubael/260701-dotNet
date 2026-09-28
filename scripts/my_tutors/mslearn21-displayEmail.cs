string[,] corporate =
{
    {"Robert", "Bavin"}, {"Simon", "Bright"},
    {"Kim", "Sinclair"}, {"Aashrita", "Kamath"},
    {"Sarah", "Delucchi"}, {"Sinan", "Ali"}
};

string[,] external =
{
    {"Vinnie", "Ashton"}, {"Cody", "Dysart"},
    {"Shay", "Lawrence"}, {"Daren", "Valdes"}
};

string externalDomain = "hayworth.com";

for (int i = 0; i < corporate.GetLength(0); i++)
{
    // display internal email addresses
    Console.WriteLine(GetEmail(corporate[i,0], corporate[i,1]));
}

for (int i = 0; i < external.GetLength(0); i++)
{
    // display external email addresses
    Console.WriteLine(GetEmail(corporate[i,0], corporate[i,1], domain: externalDomain));
}

static string GetEmail(
    string firstName,
    string secondName,
    string domain = "contoso.com")
{
  return $"{firstName[..2]}{secondName}@{domain}".ToLower();
}