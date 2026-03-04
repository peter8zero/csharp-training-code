using Module3.Adjustments;
using Xunit;

namespace Module3.Tests;

public class EarlyRetirementFactorTableTests
{
    private readonly EarlyRetirementFactorTable _table = new();

    [Theory]
    [InlineData(55, 0.55)]
    [InlineData(60, 0.76)]
    [InlineData(65, 1.00)]
    public void GetFactor_ValidAge_ReturnsExpectedFactor(int age, decimal expected)
    {
        var result = _table.GetFactor(age, "EarlyRetirement");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFactor_InvalidAge_ThrowsException()
    {
        Assert.Throws<KeyNotFoundException>(() => _table.GetFactor(50, "EarlyRetirement"));
    }

    [Fact]
    public void HasFactor_ValidAge_ReturnsTrue()
    {
        Assert.True(_table.HasFactor(60, "EarlyRetirement"));
    }

    [Fact]
    public void HasFactor_InvalidAge_ReturnsFalse()
    {
        Assert.False(_table.HasFactor(50, "EarlyRetirement"));
    }

    [Fact]
    public void GetFactor_Age65_ReturnsOne()
    {
        Assert.Equal(1.00m, _table.GetFactor(65, "EarlyRetirement"));
    }
}

public class LateRetirementFactorTableTests
{
    private readonly LateRetirementFactorTable _table = new();

    [Theory]
    [InlineData(65, 1.00)]
    [InlineData(67, 1.11)]
    [InlineData(70, 1.31)]
    public void GetFactor_ValidAge_ReturnsExpectedFactor(int age, decimal expected)
    {
        var result = _table.GetFactor(age, "LateRetirement");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFactor_InvalidAge_ThrowsException()
    {
        Assert.Throws<KeyNotFoundException>(() => _table.GetFactor(75, "LateRetirement"));
    }

    [Fact]
    public void AllFactors_AreGreaterThanOrEqualToOne()
    {
        for (int age = 65; age <= 70; age++)
        {
            var factor = _table.GetFactor(age, "LateRetirement");
            Assert.True(factor >= 1.0m, $"Factor at age {age} should be >= 1.0 but was {factor}");
        }
    }
}

public class PensionAdjusterTests
{
    [Fact]
    public void AdjustPension_WithEarlyRetirement_ReducesPension()
    {
        var table = new EarlyRetirementFactorTable();
        var adjuster = new PensionAdjuster(table);

        var result = adjuster.AdjustPension(20_000m, 60);
        Assert.Equal(15_200m, result); // 20,000 × 0.76
    }

    [Fact]
    public void AdjustPension_AtNPA_NoChange()
    {
        var table = new EarlyRetirementFactorTable();
        var adjuster = new PensionAdjuster(table);

        var result = adjuster.AdjustPension(20_000m, 65);
        Assert.Equal(20_000m, result); // 20,000 × 1.00
    }

    [Fact]
    public void AdjustPension_WithLateRetirement_IncreasesPension()
    {
        var table = new LateRetirementFactorTable();
        var adjuster = new PensionAdjuster(table);

        var result = adjuster.AdjustPension(20_000m, 68);
        Assert.Equal(23_400m, result); // 20,000 × 1.17
    }

    [Fact]
    public void AdjustPensionSafe_UnknownAge_ReturnsPensionUnchanged()
    {
        var table = new EarlyRetirementFactorTable();
        var adjuster = new PensionAdjuster(table);

        var result = adjuster.AdjustPensionSafe(20_000m, 50); // Age 50 not in table
        Assert.Equal(20_000m, result);
    }

    [Fact]
    public void AdjustPensionSafe_KnownAge_AppliesFactor()
    {
        var table = new EarlyRetirementFactorTable();
        var adjuster = new PensionAdjuster(table);

        var result = adjuster.AdjustPensionSafe(20_000m, 60);
        Assert.Equal(15_200m, result);
    }
}

public class CommutationCalculatorTests
{
    private readonly CommutationCalculator _calculator;

    public CommutationCalculatorTests()
    {
        _calculator = new CommutationCalculator(new CommutationFactorTable());
    }

    [Fact]
    public void Commute_25Percent_ReturnsCorrectLumpSum()
    {
        var result = _calculator.Commute(20_000m, 0.25m, 65);
        // Commuted pension = 20,000 × 0.25 = 5,000
        // Lump sum = 5,000 × 15.0 = 75,000
        Assert.Equal(75_000m, result.LumpSum);
    }

    [Fact]
    public void Commute_25Percent_ReturnsCorrectResidual()
    {
        var result = _calculator.Commute(20_000m, 0.25m, 65);
        Assert.Equal(15_000m, result.ResidualPension); // 20,000 - 5,000
    }

    [Fact]
    public void Commute_ZeroPercent_NoChange()
    {
        var result = _calculator.Commute(20_000m, 0m, 65);
        Assert.Equal(20_000m, result.ResidualPension);
        Assert.Equal(0m, result.LumpSum);
    }

    [Fact]
    public void Commute_CommutedPensionAmount_IsCorrect()
    {
        var result = _calculator.Commute(20_000m, 0.25m, 65);
        Assert.Equal(5_000m, result.CommutedPension);
    }

    [Theory]
    [InlineData(60, 18.5)]
    [InlineData(65, 15.0)]
    [InlineData(70, 11.5)]
    public void Commute_DifferentAges_UseCorrectFactor(int age, decimal expectedFactor)
    {
        var result = _calculator.Commute(10_000m, 0.10m, age);
        // Commuted = 10,000 × 0.10 = 1,000
        // Lump sum = 1,000 × factor
        Assert.Equal(1_000m * expectedFactor, result.LumpSum);
    }
}

public class GmpCalculatorTests
{
    private readonly GmpCalculator _calculator = new();

    [Fact]
    public void SplitPension_StandardCase_CalculatesExcess()
    {
        var result = _calculator.SplitPension(20_000m, 3_000m, 2_000m);
        Assert.Equal(3_000m, result.Pre88Gmp);
        Assert.Equal(2_000m, result.Post88Gmp);
        Assert.Equal(15_000m, result.Excess);
        Assert.Equal(20_000m, result.Total);
    }

    [Fact]
    public void SplitPension_NoGmp_AllExcess()
    {
        var result = _calculator.SplitPension(20_000m, 0m, 0m);
        Assert.Equal(20_000m, result.Excess);
    }

    [Fact]
    public void SplitPension_GmpExceedsTotal_ExcessIsZero()
    {
        var result = _calculator.SplitPension(4_000m, 3_000m, 2_000m);
        Assert.Equal(0m, result.Excess);
    }

    [Theory]
    [InlineData(RevaluationType.Fixed, 0.035)]
    [InlineData(RevaluationType.Section52, 0.04)]
    public void GetRevaluationRate_FixedTypes_ReturnsCorrectRate(RevaluationType type, decimal expected)
    {
        var result = _calculator.GetRevaluationRate(type);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetRevaluationRate_CpiCapped_ReturnsCpiWhenBelowCap()
    {
        var result = _calculator.GetRevaluationRate(RevaluationType.CpiCapped, 0.02m);
        Assert.Equal(0.02m, result); // CPI 2% is below 2.5% cap
    }

    [Fact]
    public void GetRevaluationRate_CpiCapped_ReturnsCapWhenAbove()
    {
        var result = _calculator.GetRevaluationRate(RevaluationType.CpiCapped, 0.05m);
        Assert.Equal(0.025m, result); // CPI 5% capped at 2.5%
    }

    [Fact]
    public void RevalueGmp_FixedRate_CompoundsCorrectly()
    {
        var result = _calculator.RevalueGmp(1_000m, RevaluationType.Fixed, 10);
        // 1000 × (1.035)^10 = 1410.60 (approx)
        Assert.True(result > 1_400m && result < 1_420m, $"Expected ~£1,410.60 but got £{result:N2}");
    }

    [Fact]
    public void RevalueGmp_ZeroYears_ReturnsOriginal()
    {
        var result = _calculator.RevalueGmp(1_000m, RevaluationType.Fixed, 0);
        Assert.Equal(1_000m, result);
    }

    [Fact]
    public void RevalueGmp_OneYear_AppliesOnce()
    {
        var result = _calculator.RevalueGmp(1_000m, RevaluationType.Fixed, 1);
        Assert.Equal(1_035m, result); // 1000 × 1.035
    }
}
