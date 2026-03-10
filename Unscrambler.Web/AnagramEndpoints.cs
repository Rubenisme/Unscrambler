using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Unscrambler.Web;

public static class AnagramEndpoints
{
    public static void MapAnagramEndpoints(this WebApplication app)
    {
        app.MapPost("/solve", ([FromForm] string? puzzle) =>
        {
            var searchString = puzzle ?? string.Empty;
            var matches = AnagramSolver.Solve(searchString).ToList();

            return Results.Content(RenderResults(matches, searchString), "text/html; charset=utf-8");

        }).DisableAntiforgery();
    }

    private static string RenderResults(List<string> matches, string puzzle)
    {
        if (string.IsNullOrWhiteSpace(puzzle))
            return "<p class=\"hint\">Enter an anagram to search.</p>";

        if (matches.Count is 0)
            return $"<p class=\"no-result\">No station found for <strong>{WebUtility.HtmlEncode(puzzle)}</strong>.</p>";

        var items = string.Join("\n", matches.Select(m => $"    <li class=\"station\">{WebUtility.HtmlEncode(m)}</li>"));
        return $"""
                <p class="found">Found for <strong>{WebUtility.HtmlEncode(puzzle)}</strong>:</p>
                <ul class="station-list">
                {items}
                </ul>
                """;
    }
}