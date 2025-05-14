using System;

namespace UnitTests;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class WalletUnitTests
{
    private Wallet _testWallet;
    private Action signalCountingFunction;
    private int signalCount;
    
    [Before]
    public void Setup()
    {
        _testWallet = new Wallet();
        _testWallet.StartingMoney = 0;
        signalCountingFunction = () => signalCount++;
        _testWallet.MoneyChanged += signalCountingFunction;
    }

    [BeforeTest]
    public void SetupBeforeTest()
    {
        _testWallet.ResetMoney();
        signalCount = 0;
    }

    [TestCase]
    public void MoneyChanged_MoneyAdded_SignalFired()
    {
        _testWallet.AddMoney(20);
        AssertInt(signalCount).IsEqual(1);
    }

    [TestCase]
    public void AddMoney_MoneyAdded_AmountIsAdded()
    {
        _testWallet.AddMoney(20);
        AssertFloat(_testWallet.Money).IsEqual(20);
    }

    [TestCase]
    public void HasEnough_EmptyWallet_NotEnoughMoney()
    {
        AssertBool(_testWallet.HasEnough(20)).IsEqual(false);
    }
    
    [TestCase]
    public void HasEnough_WalletFilled_HasEnoughMoney()
    {
        _testWallet.AddMoney(30);
        AssertBool(_testWallet.HasEnough(20)).IsEqual(true);
    }

    [TestCase]
    public void TrySpend_WalletFilled_MoneySpent()
    {
        _testWallet.AddMoney(30);
        _testWallet.TrySpend(20);
        AssertFloat(_testWallet.Money).IsEqual(10);
    }
    
    [TestCase]
    public void TrySpend_WalletFilled_FailToSpend()
    {
        _testWallet.AddMoney(10);
        _testWallet.TrySpend(20);
        AssertFloat(_testWallet.Money).IsEqual(10);
    }

    [TestCase]
    public void TrySpend_WalletFilled_AbleToSpend()
    {
        _testWallet.AddMoney(30);
        AssertBool(_testWallet.TrySpend(20)).IsEqual(true);
    }

    [TestCase]
    public void TrySpend_WalletFilled_UnableToSpend()
    {
        _testWallet.AddMoney(10);
        AssertBool(_testWallet.TrySpend(20)).IsEqual(false);       
    }
}