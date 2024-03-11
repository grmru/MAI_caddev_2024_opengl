namespace MAI.CadDev.Examples;

public static class BaseExample
{
    public static void Do()
    {
        int a = 0;
        int b = 0;
        int c = 0;
        Console.WriteLine(string.Format("1. a={0}; b={1}; c={2};", a, b, c));
        a = 2;
        Console.WriteLine($"2. a={a}; b={b}; c={c};");
        b = 1;
        Console.WriteLine($"3. a={a}; b={b}; c={c};");
        c=a+b;
        Console.WriteLine($"4. a={a}; b={b}; c={c};");

        if (c > 4)
        {
            Console.WriteLine("C больше 4");
        }
        else
        {
            Console.WriteLine("C меньше или равно 4");
        }

        c = 0;
        for (int i = 0; i < 10; i++)
        {
            c = c + 1;
            Console.WriteLine($"[i={i}]. a={a}; b={b}; c={c};");
            if (c > 4) { Console.WriteLine("C больше 4"); }
            else { Console.WriteLine("C меньше или равно 4"); }
        }
    }
}