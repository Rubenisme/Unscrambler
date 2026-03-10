namespace Unscrambler.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        // CreateSlimBuilder strips out HTTPS support to save space.
        // We must explicitly add it back to Kestrel.
        builder.WebHost.UseKestrelHttpsConfiguration();

        var app = builder.Build();

        // Redirect HTTP requests to HTTPS (Optional, but good practice)
        app.UseHttpsRedirection();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapAnagramEndpoints();

        app.Run();
    }
}