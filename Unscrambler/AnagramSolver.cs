namespace Unscrambler;

public static class AnagramSolver
{
    /// <summary>
    /// Strips all non-letter characters, lowercases, and sorts alphabetically.
    /// This is the canonical key used for anagram comparison.
    /// </summary>
    public static string Normalize(string input) =>
        new(
            input
                .Where(char.IsLetter)
                .Select(char.ToLower)
                .Order()
                .ToArray()
            );

    /// <summary>
    /// Returns all NS train stations whose letters are an anagram of the given puzzle.
    /// Spaces, dashes, and other non-letter characters in the puzzle are ignored.
    /// </summary>
    public static IEnumerable<string> Solve(string puzzle)
    {
        var key = Normalize(puzzle);
        return Stations.All.Where(station => Normalize(station) == key);
    }
}
