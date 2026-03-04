using Module4.TDD;
using Xunit;

namespace Module4.Tests;

public class RevaluationCalculatorTests
{
    private readonly RevaluationCalculator _calculator = new();

    // ==========================================
    // Fixed Rate Revaluation
    // ==========================================

    [Fact]
    public void FixedRate_ZeroYears_ReturnsOriginalAmount()
    {
        var result = _calculator.FixedRateRevaluation(10_000m, 0.035m, 0);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void FixedRate_OneYear_AppliesRateOnce()
    {
        var result = _calculator.FixedRateRevaluation(10_000m, 0.035m, 1);
        Assert.Equal(10_350m, result);
    }

    [Fact]
    public void FixedRate_TenYears_CompoundsCorrectly()
    {
        var result = _calculator.FixedRateRevaluation(10_000m, 0.035m, 10);
        // 10000 × 1.035^10 ≈ 14105.99
        Assert.Equal(14_106.0m, Math.Round(result, 1));
    }

    [Fact]
    public void FixedRate_ZeroRate_ReturnsSameAmount()
    {
        var result = _calculator.FixedRateRevaluation(5_000m, 0m, 20);
        Assert.Equal(5_000m, result);
    }

    [Theory]
    [InlineData(1_000, 0.05, 5, 1276.3)]   // 1000 × 1.05^5
    [InlineData(2_000, 0.03, 3, 2185.5)]   // 2000 × 1.03^3
    [InlineData(5_000, 0.04, 1, 5200.0)]   // 5000 × 1.04
    public void FixedRate_VariousScenarios_ReturnsCorrectAmount(
        decimal amount, decimal rate, int years, decimal expected)
    {
        var result = _calculator.FixedRateRevaluation(amount, rate, years);
        Assert.Equal(expected, Math.Round(result, 1));
    }

    // ==========================================
    // Compound Revaluation
    // ==========================================

    [Fact]
    public void Compound_SameAsFixedRate()
    {
        var fixed_ = _calculator.FixedRateRevaluation(10_000m, 0.04m, 5);
        var compound = _calculator.CompoundRevaluation(10_000m, 0.04m, 5);
        Assert.Equal(Math.Round(fixed_, 2), Math.Round(compound, 2));
    }

    [Fact]
    public void Compound_ZeroYears_ReturnsOriginal()
    {
        var result = _calculator.CompoundRevaluation(8_000m, 0.05m, 0);
        Assert.Equal(8_000m, result);
    }

    // ==========================================
    // CPI-Capped Revaluation
    // ==========================================

    [Fact]
    public void CpiCapped_AllBelowCap_UsesActualCpi()
    {
        // All CPI rates below 5% cap
        var cpiRates = new List<decimal> { 0.02m, 0.03m, 0.01m };
        var result = _calculator.CpiCappedRevaluation(10_000m, cpiRates, 0.05m);
        // Year 1: 10000 × 1.02 = 10200
        // Year 2: 10200 × 1.03 = 10506
        // Year 3: 10506 × 1.01 = 10611.06
        Assert.Equal(10_611.06m, Math.Round(result, 2));
    }

    [Fact]
    public void CpiCapped_AllAboveCap_UsesCapRate()
    {
        var cpiRates = new List<decimal> { 0.08m, 0.06m, 0.10m };
        var result = _calculator.CpiCappedRevaluation(10_000m, cpiRates, 0.025m);
        // All capped at 2.5%
        // 10000 × 1.025^3 = 10768.91 (approx)
        var expected = 10_000m * 1.025m * 1.025m * 1.025m;
        Assert.Equal(Math.Round(expected, 2), Math.Round(result, 2));
    }

    [Fact]
    public void CpiCapped_MixedRates_CapsWhereNeeded()
    {
        var cpiRates = new List<decimal> { 0.02m, 0.05m, 0.03m };
        var result = _calculator.CpiCappedRevaluation(10_000m, cpiRates, 0.03m);
        // Year 1: 10000 × 1.02 = 10200 (CPI 2% below cap 3%)
        // Year 2: 10200 × 1.03 = 10506 (CPI 5% capped at 3%)
        // Year 3: 10506 × 1.03 = 10821.18 (CPI 3% equals cap)
        Assert.Equal(10_821.18m, Math.Round(result, 2));
    }

    [Fact]
    public void CpiCapped_EmptyRates_ReturnsOriginal()
    {
        var result = _calculator.CpiCappedRevaluation(10_000m, new List<decimal>(), 0.025m);
        Assert.Equal(10_000m, result);
    }
}
