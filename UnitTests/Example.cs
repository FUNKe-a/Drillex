namespace Examples;

using GdUnit4;
using GdUnit4.Asserts;

[TestSuite]
public class Example
{
    [TestCase]
    public void Success()
    {
        Assertions.AssertBool(true);
    }
}