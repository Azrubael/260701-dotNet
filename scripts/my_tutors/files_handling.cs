Console.WriteLine("Input a string line:");
string? text = Console.ReadLine();

byte[] array2write = Array.Empty<byte>();
string fileName = "tutorial.txt";

if (text != null)
    array2write = System.Text.Encoding.Default.GetBytes(text);

// An attempt to write
{
    using FileStream stream = new(fileName, FileMode.Create); // Create clears old contents
    stream.Write(array2write, 0, array2write.Length);
    stream.Flush();
}

// An attempt to read
byte[] array2read;
{
    using FileStream readingStream = File.OpenRead(fileName);
    array2read = new byte[readingStream.Length];
    readingStream.ReadExactly(array2read);
}

string text2out = System.Text.Encoding.Default.GetString(array2read);
Console.WriteLine(text2out);
