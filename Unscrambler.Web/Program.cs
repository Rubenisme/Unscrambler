namespace Unscrambler.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateEmptyBuilder(new());
        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapAnagramEndpoints();

        app.Run();
    }
}