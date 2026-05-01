namespace Unscrambler;

public static class AnagramSolver
{
    private static int[] LetterCounts(string input)
    {
        var counts = new int[26];

        foreach (var idx in input
                     .Where(char.IsLetter)
                     .Select(c => char.ToLowerInvariant(c) - 'a'))
        {
            if (idx is >= 0 and < 26)
                counts[idx]++;
        }

        return counts;
    }

    private static bool ContainsLetters(int[] station, int[] puzzle)
    {
        for (var i = 0; i < 26; i++)
        {
            if (puzzle[i] > station[i])
                return false;
        }

        return true;
    }

    public static IEnumerable<string> Solve(string puzzle)
    {
        var puzzleCounts = LetterCounts(puzzle);

        foreach (var station in Stations.All)
        {
            var stationCounts = LetterCounts(station);

            if (ContainsLetters(stationCounts, puzzleCounts))
                yield return station;
        }
    }
}
