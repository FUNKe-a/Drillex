using Godot;
namespace MaterialUnitTests;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class MaterialUnitTest
{
    private Material _testMaterial;

    [Before]
    public void Setup()
    {
        _testMaterial = AutoFree(new Material());
    }

    [BeforeTest]
    public void SetupBeforeTest()
    {
        _testMaterial.MonetaryValue = 0;
        _testMaterial.Multiplier = 1;
        _testMaterial.Position = Vector2.Zero;
    }

    [TestCase(20, 2, 40)]
    [TestCase(30, 2, 60)]
    public void MonetaryValue_WithMultiplier_ValueGetsMultiplied(int monetaryValue, float multiplier, int expected)
    {
        _testMaterial.MonetaryValue = (ulong)monetaryValue;
        _testMaterial.Multiplier = multiplier;
        
        AssertInt((int)_testMaterial.MonetaryValue).IsEqual(expected);
    }

    [TestCase]
    public void MonetaryValue_WithoutMultiplier_ReturnsUnchanged()
    {
        _testMaterial.MonetaryValue = 134;
        
        AssertInt((int)_testMaterial.MonetaryValue).IsEqual(134);
    }

    [TestCase]
    public void MonetaryValue_NegativeMultiplier_MaintainsOldMultiplier()
    {
        _testMaterial.MonetaryValue = 10;
        _testMaterial.Multiplier = 4;

        _testMaterial.Multiplier = -2;
        AssertInt((int)_testMaterial.MonetaryValue).IsEqual(40);
    }

    [TestCase]
    public void Ready_SetsInitialStateCorrectly()
    {
        _testMaterial.Position = new Vector2(63.1f, 31.9f);
        _testMaterial._Ready();
        
        AssertInt((int)_testMaterial.MonetaryValue).IsEqual(0);
        AssertFloat(_testMaterial.Multiplier).IsEqual(1);
        AssertVector(_testMaterial.Position).IsEqual(new Vector2(64f, 32f));
    }
}