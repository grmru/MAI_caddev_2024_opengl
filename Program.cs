using MAI.CadDev.Examples;
using NPOI.XSSF.UserModel;
namespace MAI.CadDev;

public static class Program
{
    public static int Main()
    {
        Core core = new Core(800, 600, "MAI Разработка САПР");
        
        core.Run();

        core.Close();
        core.Dispose();
        Console.WriteLine("Done");
        return 0;
    }
}