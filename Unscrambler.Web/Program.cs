using Unscrambler;

const string IndexHtml = """
    <!DOCTYPE html>
    <html lang="nl">
    <head>
      <meta charset="UTF-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1.0" />
      <title>NS Anagram Solver</title>
      <script src="https://unpkg.com/htmx.org@2.0.4" integrity="sha384-HGfztofotfshcF7+8n44JQL2oJmowVChPTg48S+jvZoztPfvwD79OC/LTtG6dMp+" crossorigin="anonymous"></script>
      <style>
        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body {
          font-family: "Segoe UI", Arial, sans-serif;
          background: #003082;
          color: #fff;
          min-height: 100vh;
          display: flex;
          flex-direction: column;
          align-items: center;
          justify-content: center;
          padding: 2rem 1rem;
        }

        .card {
          background: #fff;
          color: #1a1a1a;
          border-radius: 12px;
          padding: 2.5rem 2rem;
          width: 100%;
          max-width: 540px;
          box-shadow: 0 8px 32px rgba(0,0,0,0.25);
        }

        .ns-logo {
          display: flex;
          align-items: center;
          gap: 0.75rem;
          margin-bottom: 1.5rem;
        }

        .ns-badge {
          background: #FFC917;
          color: #003082;
          font-weight: 900;
          font-size: 1.4rem;
          padding: 0.25rem 0.6rem;
          border-radius: 6px;
          letter-spacing: 0.05em;
        }

        .ns-title {
          font-size: 1.2rem;
          font-weight: 600;
          color: #003082;
        }

        h1 {
          font-size: 1.6rem;
          color: #003082;
          margin-bottom: 0.5rem;
        }

        p.subtitle {
          color: #555;
          margin-bottom: 1.75rem;
          font-size: 0.95rem;
        }

        .input-row {
          display: flex;
          gap: 0.75rem;
        }

        input[type="text"] {
          flex: 1;
          padding: 0.7rem 1rem;
          font-size: 1rem;
          border: 2px solid #ccd;
          border-radius: 8px;
          outline: none;
          transition: border-color 0.2s;
        }

        input[type="text"]:focus { border-color: #003082; }

        button {
          background: #FFC917;
          color: #003082;
          font-weight: 700;
          font-size: 1rem;
          border: none;
          border-radius: 8px;
          padding: 0.7rem 1.4rem;
          cursor: pointer;
          transition: background 0.15s;
          white-space: nowrap;
        }

        button:hover { background: #e6b400; }

        .htmx-request button { opacity: 0.7; cursor: wait; }

        #results {
          margin-top: 1.5rem;
          min-height: 2rem;
        }

        .hint, .no-result { color: #666; font-size: 0.95rem; }
        .no-result strong { color: #c00; }
        .found { color: #003082; font-weight: 600; margin-bottom: 0.5rem; }

        .station-list {
          list-style: none;
          padding: 0;
        }

        .station {
          padding: 0.5rem 0.75rem;
          border-left: 4px solid #FFC917;
          margin-bottom: 0.4rem;
          background: #f9f8f3;
          border-radius: 0 6px 6px 0;
          font-size: 1rem;
          font-weight: 500;
          color: #003082;
        }

        footer {
          margin-top: 2rem;
          font-size: 0.8rem;
          color: rgba(255,255,255,0.55);
          text-align: center;
        }
      </style>
    </head>
    <body>
      <div class="card">
        <div class="ns-logo">
          <span class="ns-badge">NS</span>
          <span class="ns-title">Nederlandse Spoorwegen</span>
        </div>
        <h1>Anagram Solver</h1>
        <p class="subtitle">Vul een door elkaar gehusseld stationsnaam in en ontdek welk NS-station het is.</p>

        <form hx-post="/solve"
              hx-target="#results"
              hx-swap="innerHTML"
              hx-trigger="submit">
          <div class="input-row">
            <input type="text"
                   name="puzzle"
                   placeholder="bijv. Laser meld ge"
                   autocomplete="off"
                   autofocus />
            <button type="submit">Zoeken</button>
          </div>
        </form>

        <div id="results">
          <p class="hint">Voer een anagram in om te zoeken.</p>
        </div>
      </div>

      <footer>Treinstation Anagram Solver &mdash; alle NS stations in Nederland</footer>
    </body>
    </html>
    """;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content(IndexHtml, "text/html; charset=utf-8"));

app.MapPost("/solve", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();
    var puzzle = form["puzzle"].FirstOrDefault() ?? string.Empty;
    var matches = AnagramSolver.Solve(puzzle).ToList();
    return Results.Content(RenderResults(matches, puzzle), "text/html; charset=utf-8");
});

app.Run();

// ── Helpers ────────────────────────────────────────────────────────────────

static string RenderResults(List<string> matches, string puzzle)
{
    if (string.IsNullOrWhiteSpace(puzzle))
        return "<p class=\"hint\">Voer een anagram in om te zoeken.</p>";

    if (matches.Count == 0)
        return $"<p class=\"no-result\">Geen station gevonden voor <strong>{HE(puzzle)}</strong>.</p>";

    var items = string.Join("\n", matches.Select(m => $"    <li class=\"station\">{HE(m)}</li>"));
    return $"""
        <p class="found">Gevonden voor <strong>{HE(puzzle)}</strong>:</p>
        <ul class="station-list">
        {items}
        </ul>
        """;
}

static string HE(string s) => System.Net.WebUtility.HtmlEncode(s);
