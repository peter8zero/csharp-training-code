namespace Module5.Advanced;

/// <summary>
/// Advanced LINQ exercises for analysing pension calculation results.
/// These exercises use more complex LINQ operations:
/// SelectMany, Aggregate, GroupBy with projections, and chaining.
/// </summary>
public class SchemeAnalytics
{
    private readonly List<CalculationResult> _results;

    public SchemeAnalytics(List<CalculationResult> results)
    {
        _results = results;
    }

    // Exercise 3a: Total liability by scheme
    /// <summary>
    /// Groups results by SchemeType and returns total adjusted pension per scheme.
    /// </summary>
    public Dictionary<string, decimal> GetTotalLiabilityByScheme()
    {
        // TODO: Use GroupBy and ToDictionary with Sum
        throw new NotImplementedException("Exercise 3a: Implement GetTotalLiabilityByScheme");
    }

    // Exercise 3b: Average pension by age band
    /// <summary>
    /// Groups results into age bands (55-59, 60-64, 65-69, 70+) and returns average pension per band.
    /// </summary>
    public Dictionary<string, decimal> GetAveragePensionByAgeBand()
    {
        // TODO: Create age band labels, group by band, average the adjusted pension
        // Hint: Use a helper method or inline logic to map age to band:
        //   age < 60 → "55-59"
        //   age < 65 → "60-64"
        //   age < 70 → "65-69"
        //   else → "70+"
        throw new NotImplementedException("Exercise 3b: Implement GetAveragePensionByAgeBand");
    }

    // Exercise 3c: Members with pension above threshold
    /// <summary>
    /// Returns names of members whose adjusted pension exceeds the threshold.
    /// </summary>
    public List<string> GetHighPensionMembers(decimal threshold)
    {
        // TODO: Filter and project to names
        throw new NotImplementedException("Exercise 3c: Implement GetHighPensionMembers");
    }

    // Exercise 3d: Running total using Aggregate
    /// <summary>
    /// Calculates a running total of adjusted pensions, returning each step.
    /// Returns a list of (MemberName, RunningTotal) tuples.
    /// </summary>
    public List<(string MemberName, decimal RunningTotal)> GetRunningTotals()
    {
        // TODO: Use a loop or Aggregate to build running totals
        // Hint:
        //   var runningTotal = 0m;
        //   return _results.Select(r => {
        //       runningTotal += r.AdjustedPension;
        //       return (r.MemberName, runningTotal);
        //   }).ToList();
        throw new NotImplementedException("Exercise 3d: Implement GetRunningTotals");
    }

    // Exercise 3e: Flatten multi-tranche pensions with SelectMany
    /// <summary>
    /// Given a list of members where each might have multiple calculation results
    /// (e.g., different tranches), flattens them into a single list and sums by member.
    /// </summary>
    public static Dictionary<string, decimal> FlattenAndSumByMember(
        List<List<CalculationResult>> trancheResults)
    {
        // TODO: Use SelectMany to flatten, then GroupBy member name and sum
        // Hint: trancheResults.SelectMany(t => t).GroupBy(r => r.MemberName)...
        throw new NotImplementedException("Exercise 3e: Implement FlattenAndSumByMember");
    }

    // Exercise 3f: Summary statistics
    /// <summary>
    /// Returns a summary of the calculation results.
    /// </summary>
    public SchemeSummary GetSummary()
    {
        // TODO: Calculate all summary fields using LINQ
        throw new NotImplementedException("Exercise 3f: Implement GetSummary");
    }
}

public record SchemeSummary
{
    public int TotalMembers { get; init; }
    public decimal TotalBasePension { get; init; }
    public decimal TotalAdjustedPension { get; init; }
    public decimal TotalLumpSums { get; init; }
    public decimal AveragePension { get; init; }
    public decimal HighestPension { get; init; }
    public decimal LowestPension { get; init; }
    public string HighestPensionMember { get; init; } = string.Empty;
}
