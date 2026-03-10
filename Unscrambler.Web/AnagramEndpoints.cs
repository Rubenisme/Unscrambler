using System.Net;

namespace Unscrambler.Web;

public static class AnagramEndpoints
{
    public static void MapAnagramEndpoints(this WebApplication app)
    {
        app.MapPost("/solve", async (HttpRequest request) =>
        {
            var form = await request.ReadFormAsync();
            var puzzle = form["puzzle"].FirstOrDefault() ?? string.Empty;
            var matches = AnagramSolver.Solve(puzzle).ToList();
            return Results.Content(RenderResults(matches, puzzle), "text/html; charset=utf-8");
        }).DisableAntiforgery();
    }

    private static string RenderResults(List<string> matches, string puzzle)
    {
        if (string.IsNullOrWhiteSpace(puzzle))
        {
            return """<div class="empty"><p>Enter a puzzle above to reveal the station.</p></div>""";
        }

        if (matches.Count is 0)
        {
            return $"""
                    <div class="no-result">
                        No station found for <strong>{WebUtility.HtmlEncode(puzzle)}</strong>.
                    </div>
                    """;
        }

        var items = string.Join("\n", matches.Select(m => $"<li class=\"station\">{WebUtility.HtmlEncode(m)}</li>"));

        return $"""
                <div class="found">Matches for <strong>{WebUtility.HtmlEncode(puzzle)}</strong>:</div>
                <ul class="station-list">
                    {items}
                </ul>
                """;
    }
}