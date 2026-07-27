### 2026-07-26
--------------

**Work with the file system**
For instance, the Tailwind Traders file structure has a root folder called stores. In that folder are subfolders organized by store number, and inside those folders are the sales-total and inventory files. The structure looks like this example:
📂 stores
    📄 sales.json
    📄 totals.txt
    📂 201
       📄 sales.json
       📄 salestotals.json
       📄 inventory.txt
    📂 202
	
	
The Directory class is often used to list out (or enumerate) directories.
```csharp
// List all directories
IEnumerable<string> listOfDirectories = Directory.EnumerateDirectories("stores");

foreach (var dir in listOfDirectories) {
    Console.WriteLine(dir);
}

// Outputs:
// stores/201
// stores/202

// List files in a specific directory
IEnumerable<string> files = Directory.EnumerateFiles("stores");

foreach (var file in files)
{
    Console.WriteLine(file);
}

// Outputs:
// stores/totals.txt
// stores/sales.json

// List all content in a directory and all subdirectories
// Find all *.txt files in the stores folder and its subfolders
IEnumerable<string> allFilesInAllFolders = Directory.EnumerateFiles("stores", "*.txt", SearchOption.AllDirectories);

foreach (var file in allFilesInAllFolders)
{
    Console.WriteLine(file);
}

// Outputs:
// stores/totals.txt
// stores/201/inventory.txt
```


dotnet --list-sdks

# Run the following command to clone the starter project and go to the cloned project:
git clone https://github.com/MicrosoftDocs/mslearn-dotnet-files && cd mslearn-dotnet-files

# Run the following command to create a new .NET Console project:
dotnet new console -f net8.0 -n mslearn-dotnet-files -o .

# Run the following command to open the new .NET project in the same instance of Visual Studio Code:
code -a .


# Parse data in files
dotnet add package Newtonsoft.Json