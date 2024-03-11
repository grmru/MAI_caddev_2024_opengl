using MAI.CadDev.Examples;
namespace MAI.CadDev;

public static class Program
{
    public static int Main()
    {
        ClassExample obj1 = new ClassExample();
        Console.WriteLine("Данные объекта1: " + obj1.GetData());
        obj1.Modify();
        Console.WriteLine("Данные объекта1 (после изменения): " + obj1.GetData());

        Console.WriteLine("---");

        Console.WriteLine("Данные объекта2: " + ClassExample2.GetData());
        ClassExample2.Modify();
        Console.WriteLine("Данные объекта2 (после изменения): " + ClassExample2.GetData());

        Console.WriteLine("---");

        Console.WriteLine("Данные объекта2 через метод первого объекта: " + obj1.GetDataFromAnotherClass());

        Console.WriteLine("--- пересоздали объект1");

        obj1 = new ClassExample();
        Console.WriteLine("Данные объекта1: " + obj1.GetData());
        Console.WriteLine("Данные объекта2 через метод первого объекта: " + obj1.GetDataFromAnotherClass());

        return 0;
    }
}