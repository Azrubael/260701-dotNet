// Starting with .NET 6, the below two statements are automatically included
// in a new project by way of the ImplcitUsings property group.
// using System.IO;
// using System.Collections.Generic;

using Newtonsoft.Json;


const string ptr = ">>>>>>>>>>> ";
var msg = ".NET exposes the full path to the current directory via the Directory.GetCurrentDirectory method.";
Console.WriteLine(msg);
Console.WriteLine("GetCurrentDirectory:  " + Directory.GetCurrentDirectory());
msg = "The System.Environment.SpecialFolder enumeration specifies constants to retrieve paths to special system folders.";
Console.WriteLine(msg);
string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
Console.WriteLine(ptr + docPath);


static IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = [];

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        if (file.EndsWith(".json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

var salesFiles = FindFiles("stores");
foreach (var file in salesFiles)
{
    Console.WriteLine(file);
}

// Output
// stores/sales.json
// stores/201/sales.json
// stores/202/sales.json
// stores/203/sales.json
// stores/204/sales.json

// ===========================================================
// Find all *.txt files in the stores folder and its subfolders
IEnumerable<string> allFilesInAllFolders = Directory.EnumerateFiles("stores", "*.txt", SearchOption.AllDirectories);

foreach (var file in allFilesInAllFolders)
{
    Console.WriteLine(ptr + file);
}

// Outputs:
// stores/totals.txt
// stores/201/inventory.txt


// ===========================================================
//  Find all directories and return collection of directory full names in a specified path.
IEnumerable<string> listOfDirectories = Directory.EnumerateDirectories("stores");

foreach (var dir in listOfDirectories) {
    Console.WriteLine(dir);
}

// Outputs:
// stores/201
// stores/202


// You can get the most information about a directory or a file by using the DirectoryInfo or FileInfo classes, respectively
string fileName = $"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales{Path.DirectorySeparatorChar}sales.json";
FileInfo info = new(fileName);
Console.WriteLine($"Full Name: {info.FullName}{Environment.NewLine}Directory: {info.Directory}{Environment.NewLine}Extension: {info.Extension}{Environment.NewLine}Create Date: {info.CreationTime}");

// Outputs:
// Full Name: D:\Project\code\dotNet\src\mslearn-dotnet-files\stores\201\sales\sales.json
// Directory: D:\Project\code\dotNet\src\mslearn-dotnet-files\stores\201\sales
// Extension: .json
// Create Date: 01.01.1601 2:00:00


// =========================================================
// Create a directory and a file
var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "stores","201","newDir");
if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
else
{
 Console.WriteLine("{0} already exists!", dirPath);
 File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(),
                                "greeting.txt"), "Hello World!");
}


// =========================================================
// Create the SalesTotals directory
var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);
salesFiles = FindFiles2(storesDirectory);
foreach(var item in salesFiles)
{
    Console.WriteLine(item);
}

File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), String.Empty);

static IEnumerable<string> FindFiles2(string folderName)
{
    List<string> salesFiles = [];

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

// =========================================================
// Read data from files
char ss = Path.DirectorySeparatorChar;
File.ReadAllText($"stores{ss}201{ss}sales.json");
// The return object from ReadAllText is a string.
// { "total": 22385.32 }


// Parse data in files, using null-forgiving operator
string? salesJson = File.ReadAllText($"stores{ss}201{ss}sales.json");
var salesData = JsonConvert.DeserializeObject<SalesTotal>(salesJson)
    ?? throw new InvalidOperationException("Parsing JSON deserialized to null.");
Console.WriteLine(salesData.Total);
Console.WriteLine(salesData.Total);


// =========================================================
// Write data to files
var data = JsonConvert.DeserializeObject<SalesTotal>(salesJson)
    ?? throw new InvalidOperationException("Writing JSON deserialized to null.");

File.WriteAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", data.Total.ToString());

// totals.txt
// 22385.32


// Append data to files
data = JsonConvert.DeserializeObject<SalesTotal>(salesJson)
    ?? throw new InvalidOperationException("Appending JSON deserialized to null.");

File.AppendAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", $" + {data.Total}{Environment.NewLine}");

// totals.txt
// 22385.32
// 22385.32


// =========================================================
var currentDirectory9 = Directory.GetCurrentDirectory();
var storesDirectory9 = Path.Combine(currentDirectory9, "stores");

var salesTotalDir9 = Path.Combine(currentDirectory9, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir9);

var salesFiles9 = FindFiles9(storesDirectory9);

var salesTotal9 = CalculateSalesTotal9(salesFiles9);

File.AppendAllText(Path.Combine(salesTotalDir9, "totals.txt"), $"{salesTotal9}{Environment.NewLine}");

static IEnumerable<string> FindFiles9(string folderName)
{
    List<string> salesFiles = [];

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

static double CalculateSalesTotal9(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

record SalesData (double Total);



class SalesTotal
{
  public double Total { get; set; }
}