#!/usr/bin/env dotnet run

// The typical command to run this script
// PS d:\> dotnet run hello_world.cs

int[] arr = [54, 7, -41, 2, 4, 2, 89, 33, -5, 12];

int tmp;
for (int i = 0; i < arr.Length - 1; i++) {
	for (int j = i + 1; j < arr.Length; j++) {
		if (arr[i] > arr[j]) {
			tmp = arr[i];
			arr[i] = arr[j];
			arr[j] = tmp;
		}
	}
}

foreach (var el in arr) {
	Console.Write($"{el},  ");
}
Console.WriteLine("\nThis is the end.");
