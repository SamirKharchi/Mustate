namespace Mustate.UnitTests.Models;

public static class MutableGenerators
{
    /// <summary>
    /// Generates an instance of <see cref="SomeMutable"/> with following properties set:
    /// 1. NonMutable = "nonMutable"
    /// 2. MutableFull = { Mutable = "" }
    /// </summary>
    /// <returns></returns>
    public static SomeMutable CreateModel()
    {
        return new SomeMutable
        {
            NonMutable = "nonMutable",
            MutableFull = new AnotherMutable
            {
                Mutable = ""
            }
        };
    }
    
}