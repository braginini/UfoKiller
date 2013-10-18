public class GlobalVariable
{
    public int finalScore;
    private static GlobalVariable instance;

    public static GlobalVariable Instance
    {
        get
        {
            if (instance == null)
                instance = new GlobalVariable();
            return instance;
        }
    }
}