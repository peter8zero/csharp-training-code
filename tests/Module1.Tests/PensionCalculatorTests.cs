using Module1.PensionBasics;
using Xunit;

namespace Module1.Tests;

public class PensionCalculatorTests
{
    private readonly PensionCalculator _calculator = new();

    // ==========================================
    // Exercise 1: CalculateAnnualPension
    // ==========================================

    [Fact]
    public void CalculateAnnualPension_StandardCase_ReturnsCorrectAmount()
    {
        // 40,000 salary × 20 years × 1/60 = 13,333.33...
        var result = _calculator.CalculateAnnualPension(40_000m, 20, 1m / 60m);
        Assert.Equal(13_333.33m, Math.Round(result, 2));
    }

    [Fact]
    public void CalculateAnnualPension_ZeroService_ReturnsZero()
    {
        var result = _calculator.CalculateAnnualPension(50_000m, 0, 1m / 60m);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateAnnualPension_EightiethsAccrual_ReturnsCorrectAmount()
    {
        // 48,000 × 30 × 1/80 = 18,000
        var result = _calculator.CalculateAnnualPension(48_000m, 30, 1m / 80m);
        Assert.Equal(18_000m, result);
    }

    [Theory]
    [InlineData(30_000, 10, 60, 5_000.00)]
    [InlineData(60_000, 25, 60, 25_000.00)]
    [InlineData(45_000, 15, 80, 8_437.50)]
    public void CalculateAnnualPension_VariousInputs_ReturnsCorrectAmount(
        decimal salary, int serviceYears, int accrualDivisor, decimal expected)
    {
        var result = _calculator.CalculateAnnualPension(salary, serviceYears, 1m / accrualDivisor);
        Assert.Equal(expected, Math.Round(result, 2));
    }

    // ==========================================
    // Exercise 2: GetAccrualRate
    // ==========================================

    [Fact]
    public void GetAccrualRate_Sixtieths_ReturnsOneOverSixty()
    {
        var result = _calculator.GetAccrualRate(AccrualBasis.Sixtieths);
        Assert.Equal(1m / 60m, result);
    }

    [Fact]
    public void GetAccrualRate_Eightieths_ReturnsOneOverEighty()
    {
        var result = _calculator.GetAccrualRate(AccrualBasis.Eightieths);
        Assert.Equal(1m / 80m, result);
    }

    // ==========================================
    // Exercise 3: CalculatePartTimePension
    // ==========================================

    [Fact]
    public void CalculatePartTimePension_ThreeFifths_ReturnsReducedPension()
    {
        // 40,000 × 20 × 1/60 × 0.6 = 8,000
        var result = _calculator.CalculatePartTimePension(40_000m, 20, 1m / 60m, 0.6m);
        Assert.Equal(8_000m, Math.Round(result, 2));
    }

    [Fact]
    public void CalculatePartTimePension_FullTime_SameAsStandard()
    {
        var standard = _calculator.CalculateAnnualPension(40_000m, 20, 1m / 60m);
        var fullTime = _calculator.CalculatePartTimePension(40_000m, 20, 1m / 60m, 1.0m);
        Assert.Equal(Math.Round(standard, 2), Math.Round(fullTime, 2));
    }

    [Fact]
    public void CalculatePartTimePension_HalfTime_ReturnsHalfPension()
    {
        var full = _calculator.CalculateAnnualPension(50_000m, 25, 1m / 60m);
        var half = _calculator.CalculatePartTimePension(50_000m, 25, 1m / 60m, 0.5m);
        Assert.Equal(Math.Round(full / 2, 2), Math.Round(half, 2));
    }

    // ==========================================
    // Exercise 4: ApplyEarlyRetirementReduction
    // ==========================================

    [Fact]
    public void ApplyEarlyRetirement_ThreeYearsEarly_ReducesByTwelvePercent()
    {
        // 3 years early × 4% = 12% reduction → 88% of pension
        var result = _calculator.ApplyEarlyRetirementReduction(13_333.33m, 62, 65, 0.04m);
        Assert.Equal(11_733.33m, Math.Round(result, 2));
    }

    [Fact]
    public void ApplyEarlyRetirement_AtNPA_NoReduction()
    {
        var result = _calculator.ApplyEarlyRetirementReduction(10_000m, 65, 65, 0.04m);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void ApplyEarlyRetirement_AfterNPA_NoReduction()
    {
        // Retiring after NPA should give full pension (no reduction)
        var result = _calculator.ApplyEarlyRetirementReduction(10_000m, 67, 65, 0.04m);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void ApplyEarlyRetirement_OneYearEarly_ReducesByRate()
    {
        // 1 year early × 3% = 3% reduction → 97%
        var result = _calculator.ApplyEarlyRetirementReduction(20_000m, 64, 65, 0.03m);
        Assert.Equal(19_400m, result);
    }

    [Theory]
    [InlineData(10_000, 60, 65, 0.04, 8_000)]   // 5 years early, 20% reduction
    [InlineData(10_000, 63, 65, 0.05, 9_000)]   // 2 years early, 10% reduction
    [InlineData(10_000, 55, 65, 0.03, 7_000)]   // 10 years early, 30% reduction
    public void ApplyEarlyRetirement_VariousScenarios_ReturnsCorrectReduction(
        decimal pension, int retireAge, int npa, decimal rate, decimal expected)
    {
        var result = _calculator.ApplyEarlyRetirementReduction(pension, retireAge, npa, rate);
        Assert.Equal(expected, Math.Round(result, 2));
    }
}
