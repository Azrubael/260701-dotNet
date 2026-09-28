//  The interviewer wants you to write a program that checks whether an IPv4 address is valid or invalid

Console.WriteLine("Eneter an IPv4 address XX.XX.XX.XX: ");
string? Input;
while (true)
{
	Input = Console.ReadLine();
	if ((Input != null) && (Input.Length > 6)) break;
}

string[] ipv4Address = [];
if (Input != null)
{
	ipv4Address = Input.Split(".", StringSplitOptions.RemoveEmptyEntries);
}

if (ValidateLength(ipv4Address)
	&& ValidateZeroes(ipv4Address)
	&& ValidateRange(ipv4Address)
	&& ValidateDigits(ipv4Address)) 
{
    Console.WriteLine($"ip '{Input}' is a valid IPv4 address");
} 
else 
{
    Console.WriteLine($"ip '{Input}' is an invalid IPv4 address");
}


static bool ValidateLength(string[] ipv4Address)
{
	return ( ipv4Address.Length == 4 ? true : false );
}


static bool ValidateZeroes(string[] ipv4Address)
{
	foreach(string group in ipv4Address)
	{
		if (group.Length > 1 && group.StartsWith("0"))
			return false;
	}
	return true;
}


static bool ValidateRange(string[] ipv4Address)
{
	foreach(string group in ipv4Address)
	{
		int value = int.Parse(group);
		if (value < 0 || value > 255)
		{
			return false;
		}
	}
	return true;
}


static bool ValidateDigits(string[] ipv4Address)
{
	foreach(string group in ipv4Address)
	{
		bool isHex = !string.IsNullOrEmpty(group) &&
			group.All(char.IsDigit);
		if (!isHex)
		{
			return false;
		}
	}
	return true;
}
