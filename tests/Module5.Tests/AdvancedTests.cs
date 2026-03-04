using Module5.Advanced;
using Xunit;

namespace Module5.Tests;

public class PensionCalculationBuilderTests
{
    private static Scheme DefaultScheme => new()
    {
        Name = "Test Scheme", Type = "DB60ths", NormalPensionAge = 65,
        EarlyRetirementReductionPerYear = 0.04m, LateRetirementIncreasePerYear = 0.05m,
        CommutationFactor = 15.0m
    };

    private static MemberRecord DefaultMember => new()
    {
        Name = "Test Member", Salary = 60_000m, ServiceYears = 30, SchemeType = "DB60ths"
    };

    [Fact]
    public void Build_BasicCalculation_ReturnsCorrectBasePension()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .Build();

        // 60,000 x 30 x 1/60 = 30,000
        Assert.Equal(30_000m, result.BasePension);
    }

    [Fact]
    public void Build_AtNPA_AdjustedEqualsBas()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .AtRetirementAge(65)
            .Build();

        Assert.Equal(result.BasePension, result.AdjustedPension);
    }

    [Fact]
    public void Build_EarlyRetirement_ReducesPension()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .AtRetirementAge(62)
            .Build();

        // 30,000 x (1 - 3 x 0.04) = 30,000 x 0.88 = 26,400
        Assert.Equal(26_400m, result.AdjustedPension);
    }

    [Fact]
    public void Build_LateRetirement_IncreasesPension()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .AtRetirementAge(68)
            .Build();

        // 30,000 x (1 + 3 x 0.05) = 30,000 x 1.15 = 34,500
        Assert.Equal(34_500m, result.AdjustedPension);
    }

    [Fact]
    public void Build_WithCommutation_CalculatesLumpSum()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .WithCommutation(0.25m)
            .Build();

        // Base = 30,000. Commuted = 30,000 x 0.25 = 7,500
        // Lump sum = 7,500 x 15 = 112,500
        // Residual = 30,000 - 7,500 = 22,500
        Assert.Equal(112_500m, result.LumpSum);
        Assert.Equal(22_500m, result.ResidualPension);
    }

    [Fact]
    public void Build_NoCommutation_LumpSumIsZero()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .Build();

        Assert.Equal(0m, result.LumpSum);
        Assert.Equal(result.AdjustedPension, result.ResidualPension);
    }

    [Fact]
    public void Build_FluentChaining_ReturnsSameBuilder()
    {
        var builder = new PensionCalculationBuilder();
        var returned = builder.ForMember(DefaultMember);
        Assert.Same(builder, returned);
    }

    [Fact]
    public void Build_MemberNameInResult()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .Build();

        Assert.Equal("Test Member", result.MemberName);
    }

    [Fact]
    public void Build_DefaultRetirementAge_UsesSchemeNPA()
    {
        var result = new PensionCalculationBuilder()
            .ForMember(DefaultMember)
            .WithScheme(DefaultScheme)
            .Build();

        Assert.Equal(65, result.RetirementAge);
    }

    [Fact]
    public void Build_WithoutMember_ThrowsInvalidOperation()
    {
        var builder = new PensionCalculationBuilder()
            .WithScheme(DefaultScheme);

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutScheme_ThrowsInvalidOperation()
    {
        var builder = new PensionCalculationBuilder()
            .ForMember(DefaultMember);

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_80thsScheme_UsesCorrectAccrual()
    {
        var member80 = new MemberRecord { Name = "80ths Member", Salary = 40_000m, ServiceYears = 20, SchemeType = "DB80ths" };
        var scheme80 = new Scheme { Name = "80ths Scheme", Type = "DB80ths", NormalPensionAge = 65, CommutationFactor = 15.0m };

        var result = new PensionCalculationBuilder()
            .ForMember(member80)
            .WithScheme(scheme80)
            .Build();

        // 40,000 x 20 x 1/80 = 10,000
        Assert.Equal(10_000m, result.BasePension);
    }

    [Fact]
    public void Reset_ClearsState_AllowsReuse()
    {
        var builder = new PensionCalculationBuilder();

        var result1 = builder.ForMember(DefaultMember).WithScheme(DefaultScheme).Build();

        var member2 = new MemberRecord { Name = "Other", Salary = 30_000m, ServiceYears = 10, SchemeType = "DB60ths" };
        var result2 = builder.Reset().ForMember(member2).WithScheme(DefaultScheme).Build();

        Assert.Equal("Other", result2.MemberName);
        Assert.Equal(5_000m, result2.BasePension); // 30,000 x 10 x 1/60
    }
}

public class SchemeAnalyticsTests
{
    private static List<CalculationResult> SampleResults => new()
    {
        new CalculationResult { MemberName = "Alice", SchemeType = "DB60ths", RetirementAge = 65, BasePension = 21_000m, AdjustedPension = 21_000m, ResidualPension = 15_750m, LumpSum = 78_750m, CommutationPercentage = 0.25m },
        new CalculationResult { MemberName = "Bob", SchemeType = "DB60ths", RetirementAge = 63, BasePension = 28_600m, AdjustedPension = 25_168m, ResidualPension = 18_876m, LumpSum = 94_380m, CommutationPercentage = 0.25m },
        new CalculationResult { MemberName = "Carol", SchemeType = "DB80ths", RetirementAge = 65, BasePension = 8_550m, AdjustedPension = 8_550m, ResidualPension = 6_412.5m, LumpSum = 32_062.5m, CommutationPercentage = 0.25m },
        new CalculationResult { MemberName = "David", SchemeType = "DB60ths", RetirementAge = 67, BasePension = 23_383.33m, AdjustedPension = 25_721.67m, ResidualPension = 19_291.25m, LumpSum = 96_456.25m, CommutationPercentage = 0.25m },
        new CalculationResult { MemberName = "Emma", SchemeType = "CARE", RetirementAge = 60, BasePension = 7_632.65m, AdjustedPension = 6_106.12m, ResidualPension = 4_579.59m, LumpSum = 22_897.95m, CommutationPercentage = 0.25m },
    };

    [Fact]
    public void GetTotalLiabilityByScheme_GroupsCorrectly()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetTotalLiabilityByScheme();

        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey("DB60ths"));
        Assert.True(result.ContainsKey("DB80ths"));
        Assert.True(result.ContainsKey("CARE"));
    }

    [Fact]
    public void GetTotalLiabilityByScheme_SumsCorrectly()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetTotalLiabilityByScheme();

        var expectedDb60 = 21_000m + 25_168m + 25_721.67m;
        Assert.Equal(Math.Round(expectedDb60, 2), Math.Round(result["DB60ths"], 2));
    }

    [Fact]
    public void GetHighPensionMembers_FiltersCorrectly()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetHighPensionMembers(20_000m);

        Assert.Contains("Alice", result);
        Assert.Contains("Bob", result);
        Assert.Contains("David", result);
        Assert.DoesNotContain("Carol", result);
        Assert.DoesNotContain("Emma", result);
    }

    [Fact]
    public void GetRunningTotals_AccumulatesCorrectly()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetRunningTotals();

        Assert.Equal(5, result.Count);
        Assert.Equal(SampleResults[0].AdjustedPension, result[0].RunningTotal);
        var totalAll = SampleResults.Sum(r => r.AdjustedPension);
        Assert.Equal(Math.Round(totalAll, 2), Math.Round(result[^1].RunningTotal, 2));
    }

    [Fact]
    public void FlattenAndSumByMember_CombinesTranches()
    {
        var tranches = new List<List<CalculationResult>>
        {
            new() {
                new CalculationResult { MemberName = "Alice", AdjustedPension = 10_000m },
                new CalculationResult { MemberName = "Bob", AdjustedPension = 5_000m },
            },
            new() {
                new CalculationResult { MemberName = "Alice", AdjustedPension = 8_000m },
                new CalculationResult { MemberName = "Bob", AdjustedPension = 3_000m },
            }
        };

        var result = SchemeAnalytics.FlattenAndSumByMember(tranches);

        Assert.Equal(2, result.Count);
        Assert.Equal(18_000m, result["Alice"]);
        Assert.Equal(8_000m, result["Bob"]);
    }

    [Fact]
    public void GetSummary_ReturnsCorrectStats()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetSummary();

        Assert.Equal(5, result.TotalMembers);
        Assert.True(result.HighestPension > result.LowestPension);
        Assert.True(result.AveragePension > 0);
        Assert.Equal(SampleResults.Sum(r => r.BasePension), result.TotalBasePension);
    }

    [Fact]
    public void GetAveragePensionByAgeBand_GroupsCorrectly()
    {
        var analytics = new SchemeAnalytics(SampleResults);
        var result = analytics.GetAveragePensionByAgeBand();

        // We have ages 60, 63, 65, 67 -> bands "55-59" and "60-64" and "65-69"
        Assert.True(result.Count >= 2);
    }
}

public class CalculationResultTests
{
    [Fact]
    public void CalculationResult_IsImmutable_WithExpression()
    {
        var original = new CalculationResult
        {
            MemberName = "Alice",
            BasePension = 20_000m,
            AdjustedPension = 20_000m,
            ResidualPension = 15_000m,
            LumpSum = 75_000m
        };

        // Records support 'with' expressions for creating modified copies
        var modified = original with { MemberName = "Bob" };

        Assert.Equal("Alice", original.MemberName);
        Assert.Equal("Bob", modified.MemberName);
        Assert.Equal(original.BasePension, modified.BasePension);
    }

    [Fact]
    public void CalculationResult_ValueEquality()
    {
        var a = new CalculationResult { MemberName = "Alice", BasePension = 20_000m };
        var b = new CalculationResult { MemberName = "Alice", BasePension = 20_000m };

        Assert.Equal(a, b);
    }
}
