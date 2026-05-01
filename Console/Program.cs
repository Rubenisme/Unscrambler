namespace Console;

internal class Program
{
    // If a puzzle is passed as command-line arguments, solve it once and exit.
    // E.g.: dotnet run -- "Laser meld ge"
    //   or: dotnet run -- Laser meld ge
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            Solve(string.Join(" ", args));
            return;
        }

        // Interactive mode: keep asking for puzzles until the user quits.
        System.Console.WriteLine("NS Train Station Anagram Solver");
        System.Console.WriteLine("Type a scrambled station name and press Enter (or 'quit' to exit).");
        System.Console.WriteLine();

        while (true)
        {
            System.Console.Write("> ");
            var input = System.Console.ReadLine();

            if (input is null || input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.IsNullOrWhiteSpace(input))
                continue;

            Solve(input);
        }

        static void Solve(string puzzle)
        {
            var matches = Unscrambler.AnagramSolver.Solve(puzzle).ToList();

            if (matches.Count is 0)
                System.Console.WriteLine("No matching station found.");
            else
                foreach (var station in matches)
                    System.Console.WriteLine($"  → {station}");

            System.Console.WriteLine();
        }
    }
}