using Unscrambler;

// If a puzzle is passed as command-line arguments, solve it once and exit.
// E.g.: dotnet run -- "Laser meld ge"
//   or: dotnet run -- Laser meld ge
if (args.Length > 0)
{
    Solve(string.Join(" ", args));
    return;
}

// Interactive mode: keep asking for puzzles until the user quits.
Console.WriteLine("NS Train Station Anagram Solver");
Console.WriteLine("Type a scrambled station name and press Enter (or 'quit' to exit).");
Console.WriteLine();

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (input is null || input.Equals("quit", StringComparison.OrdinalIgnoreCase))
        break;

    if (string.IsNullOrWhiteSpace(input))
        continue;

    Solve(input);
}

static void Solve(string puzzle)
{
    var matches = AnagramSolver.Solve(puzzle).ToList();

    if (matches.Count == 0)
        Console.WriteLine("No matching station found.");
    else
        foreach (var station in matches)
            Console.WriteLine($"  → {station}");

    Console.WriteLine();
}
