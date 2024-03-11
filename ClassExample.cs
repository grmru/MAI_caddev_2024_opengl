namespace MAI.CadDev.Examples;

public class ClassExample
{
    string secreteWord;

    public ClassExample()
    {
        secreteWord = "pass";
    }

    public void Modify()
    {
        this.secreteWord = "newPass";
    }

    public string GetData()
    {
        return this.secreteWord;
    }

    public string GetDataFromAnotherClass()
    {
        return ClassExample2.GetData();
    }
}

public static class ClassExample2
{
    static string secreteWord;

    public static void Modify()
    {
        secreteWord = "staticPass";
    }

    public static string GetData()
    {
        return secreteWord;
    } 
}