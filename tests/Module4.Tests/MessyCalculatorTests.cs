using Module4.TDD;
using Xunit;

namespace Module4.Tests;

/// <summary>
/// These tests verify the MessyCalculator behaviour.
/// They all pass with the current (messy) implementation.
///
/// REFACTORING EXERCISE:
/// Your task is to refactor MessyCalculator.cs to make the code cleaner
/// while ensuring ALL these tests still pass. Run the tests after each change!
/// </summary>
public class MessyCalculatorTests
{
    private readonly MessyCalculator _calculator = new();

    [Theory]
    [InlineData(40_000, 20, "60ths", 65, false, 13_333.33)]
    [InlineData(48_000, 30, "80ths", 65, false, 18_000.00)]
    [InlineData(49_000, 10, "CARE", 65, false, 10_000.00)]
    public void CalcPension_AtNPA_ReturnsBasicPension(
        decimal salary, int years, string type, int age, bool lumpSum, decimal expected)
    {
        var result = _calculator.CalcPension(salary, years, type, age, lumpSum);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalcPension_EarlyRetirement_ReducesPension()
    {
        // 40000 × 20 / 60 = 13333.33 × (1 - 3 × 0.04) = 13333.33 × 0.88 = 11733.33
        var result = _calculator.CalcPension(40_000m, 20, "60ths", 62, false);
        Assert.Equal(11_733.33m, result);
    }

    [Fact]
    public void CalcPension_LateRetirement_IncreasesPension()
    {
        // 40000 × 20 / 60 = 13333.33 × (1 + 2 × 0.05) = 13333.33 × 1.10 = 14666.67
        var result = _calculator.CalcPension(40_000m, 20, "60ths", 67, false);
        Assert.Equal(14_666.67m, result);
    }

    [Fact]
    public void CalcPension_WithLumpSum_Reduces75Percent()
    {
        // 40000 × 20 / 60 = 13333.33 × 0.75 = 10000.00
        var result = _calculator.CalcPension(40_000m, 20, "60ths", 65, true);
        Assert.Equal(10_000.00m, result);
    }

    [Fact]
    public void CalcPension_EarlyRetirementWithLumpSum_BothApplied()
    {
        // 40000 × 20 / 60 = 13333.33
        // Early: × 0.88 = 11733.33
        // Lump sum: × 0.75 = 8800.00
        var result = _calculator.CalcPension(40_000m, 20, "60ths", 62, true);
        Assert.Equal(8_800.00m, result);
    }

    [Fact]
    public void CalcPension_VeryEarlyRetirement_PensionDoesNotGoNegative()
    {
        // 30 years early × 4% = 120% reduction → capped at 0
        var result = _calculator.CalcPension(40_000m, 20, "60ths", 35, false);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalcPension_80thsScheme_UsesCorrectAccrual()
    {
        // 40000 × 20 / 80 = 10000
        var result = _calculator.CalcPension(40_000m, 20, "80ths", 65, false);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void CalcPension_CAREScheme_UsesCorrectAccrual()
    {
        // 49000 × 10 / 49 = 10000
        var result = _calculator.CalcPension(49_000m, 10, "CARE", 65, false);
        Assert.Equal(10_000m, result);
    }
}
