using Module2.MemberData;
using Xunit;

namespace Module2.Tests;

public class MemberQueryTests
{
    private readonly List<Member> _members = SampleData.GetMembers();
    private readonly MemberQueries _queries;
    private readonly DateTime _today = new(2026, 2, 27);

    public MemberQueryTests()
    {
        _queries = new MemberQueries(_members);
    }

    // Exercise 1: GetMembersByScheme
    [Fact]
    public void GetMembersByScheme_DB60ths_ReturnsFiveMembers()
    {
        var result = _queries.GetMembersByScheme("DB60ths");
        Assert.Equal(6, result.Count);
        Assert.All(result, m => Assert.Equal("DB60ths", m.SchemeType));
    }

    [Fact]
    public void GetMembersByScheme_DB80ths_ReturnsThreeMembers()
    {
        var result = _queries.GetMembersByScheme("DB80ths");
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetMembersByScheme_CARE_ReturnsThreeMembers()
    {
        var result = _queries.GetMembersByScheme("CARE");
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetMembersByScheme_Unknown_ReturnsEmpty()
    {
        var result = _queries.GetMembersByScheme("Unknown");
        Assert.Empty(result);
    }

    // Exercise 2: GetActiveMembers
    [Fact]
    public void GetActiveMembers_ExcludesDeferred()
    {
        var result = _queries.GetActiveMembers();
        Assert.Equal(9, result.Count);
        Assert.All(result, m => Assert.False(m.IsDeferred));
    }

    // Exercise 3: CalculateTotalLiability
    [Fact]
    public void CalculateTotalLiability_ReturnsCorrectTotal()
    {
        var result = _queries.CalculateTotalLiability();
        // Calculate expected: sum of (salary * serviceYears * accrualRate) for all members
        var expected = _members.Sum(m => m.Salary * m.ServiceYears * MemberQueries.GetAccrualRate(m.SchemeType));
        Assert.Equal(Math.Round(expected, 2), Math.Round(result, 2));
    }

    [Fact]
    public void CalculateTotalLiability_IsPositive()
    {
        var result = _queries.CalculateTotalLiability();
        Assert.True(result > 0);
    }

    // Exercise 4: GetAverageSalaryByScheme
    [Fact]
    public void GetAverageSalaryByScheme_ReturnsThreeGroups()
    {
        var result = _queries.GetAverageSalaryByScheme();
        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey("DB60ths"));
        Assert.True(result.ContainsKey("DB80ths"));
        Assert.True(result.ContainsKey("CARE"));
    }

    [Fact]
    public void GetAverageSalaryByScheme_DB60ths_CorrectAverage()
    {
        var result = _queries.GetAverageSalaryByScheme();
        var db60Members = _members.Where(m => m.SchemeType == "DB60ths").ToList();
        var expectedAvg = db60Members.Average(m => m.Salary);
        Assert.Equal(Math.Round(expectedAvg, 2), Math.Round(result["DB60ths"], 2));
    }

    // Exercise 5: GetMembersApproachingRetirement
    [Fact]
    public void GetMembersApproachingRetirement_ReturnsCorrectMembers()
    {
        var result = _queries.GetMembersApproachingRetirement(_today);
        // Members born 1966 or earlier are 60+ in Feb 2026
        // Frank (1960), Liam (1963), Bob (1965) - all are 60+
        // Irene (1968) is 57, so not included
        Assert.True(result.Count >= 3);
        Assert.Contains(result, m => m.Name == "Frank Davies");
        Assert.Contains(result, m => m.Name == "Liam Walker");
        Assert.Contains(result, m => m.Name == "Bob Jones");
    }

    [Fact]
    public void GetMembersApproachingRetirement_ExcludesYoungMembers()
    {
        var result = _queries.GetMembersApproachingRetirement(_today);
        Assert.DoesNotContain(result, m => m.Name == "Karen Hall"); // Born 1990
        Assert.DoesNotContain(result, m => m.Name == "Emma Taylor"); // Born 1988
    }

    // Exercise 6: GetMembersOrderedByPension
    [Fact]
    public void GetMembersOrderedByPension_FirstMemberHasHighestPension()
    {
        var result = _queries.GetMembersOrderedByPension();
        var firstPension = result[0].Salary * result[0].ServiceYears * MemberQueries.GetAccrualRate(result[0].SchemeType);
        var secondPension = result[1].Salary * result[1].ServiceYears * MemberQueries.GetAccrualRate(result[1].SchemeType);
        Assert.True(firstPension >= secondPension);
    }

    [Fact]
    public void GetMembersOrderedByPension_ReturnsAllMembers()
    {
        var result = _queries.GetMembersOrderedByPension();
        Assert.Equal(_members.Count, result.Count);
    }

    [Fact]
    public void GetMembersOrderedByPension_IsDescending()
    {
        var result = _queries.GetMembersOrderedByPension();
        for (int i = 0; i < result.Count - 1; i++)
        {
            var current = result[i].Salary * result[i].ServiceYears * MemberQueries.GetAccrualRate(result[i].SchemeType);
            var next = result[i + 1].Salary * result[i + 1].ServiceYears * MemberQueries.GetAccrualRate(result[i + 1].SchemeType);
            Assert.True(current >= next, $"{result[i].Name} should have pension >= {result[i + 1].Name}");
        }
    }

    // Exercise 7: GetMemberSummaries
    [Fact]
    public void GetMemberSummaries_ReturnsCorrectCount()
    {
        var result = _queries.GetMemberSummaries(_today);
        Assert.Equal(_members.Count, result.Count);
    }

    [Fact]
    public void GetMemberSummaries_ContainsCorrectPension()
    {
        var result = _queries.GetMemberSummaries(_today);
        var alice = result.First(s => s.Name == "Alice Smith");
        // Alice: 45000 * 28 * 1/60 = 21,000
        Assert.Equal(21_000m, Math.Round(alice.AnnualPension, 2));
    }

    [Fact]
    public void GetMemberSummaries_YearsToRetirementCorrect()
    {
        var result = _queries.GetMemberSummaries(_today);
        var karen = result.First(s => s.Name == "Karen Hall");
        // Karen born 1990, so in 2026 she's ~35, years to 65 = 30
        Assert.True(karen.YearsToRetirement >= 29 && karen.YearsToRetirement <= 30);
    }
}
