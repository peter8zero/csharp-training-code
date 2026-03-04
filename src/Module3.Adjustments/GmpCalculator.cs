namespace Module3.Adjustments;

/// <summary>
/// Splits a pension into its GMP (Guaranteed Minimum Pension) components.
///
/// UK pensions accrued between 1978-1997 may include a GMP element:
/// - Pre-88 GMP: accrued 6 April 1978 to 5 April 1988
/// - Post-88 GMP: accrued 6 April 1988 to 5 April 1997
/// - Excess: the pension above the GMP amount
///
/// GMP revaluation types determine how deferred GMPs increase over time.
/// </summary>
public class GmpCalculator
{
    // Exercise 5: Split pension into GMP components
    /// <summary>
    /// Splits a total pension into its three components.
    /// Excess = totalPension - pre88Gmp - post88Gmp
    /// </summary>
    public GmpSplit SplitPension(decimal totalPension, decimal pre88Gmp, decimal post88Gmp)
    {
        // TODO: Calculate excess and return a GmpSplit
        // Hint: excess = totalPension - pre88Gmp - post88Gmp
        // Handle case where total < GMP amounts (excess should be 0, not negative)
        throw new NotImplementedException("Exercise 5: Implement SplitPension");
    }

    // Exercise 6: Get the revaluation rate using a switch expression
    /// <summary>
    /// Returns the annual revaluation rate for a given revaluation type.
    /// Fixed = 0.035 (3.5%), Section52 = 0.04 (4%), CpiCapped = the lesser of cpiRate and 0.025
    /// </summary>
    public decimal GetRevaluationRate(RevaluationType revalType, decimal cpiRate = 0)
    {
        // TODO: Use a switch expression to return the rate
        // Example: return revalType switch
        // {
        //     RevaluationType.Fixed => ...,
        //     RevaluationType.Section52 => ...,
        //     RevaluationType.CpiCapped => ...,
        //     _ => throw new ArgumentException(...)
        // };
        throw new NotImplementedException("Exercise 6: Implement GetRevaluationRate");
    }

    /// <summary>
    /// Revalues a GMP amount over a number of years using the specified revaluation type.
    /// Uses compound revaluation: amount × (1 + rate)^years
    /// </summary>
    public decimal RevalueGmp(decimal gmpAmount, RevaluationType revalType, int years, decimal cpiRate = 0)
    {
        // TODO: Get the rate, then apply compound revaluation
        // Hint: Use a for loop or Math.Pow (cast carefully with decimals)
        throw new NotImplementedException("Exercise 6: Implement RevalueGmp");
    }
}

public enum RevaluationType
{
    Fixed,
    Section52,
    CpiCapped
}

public class GmpSplit
{
    public decimal Pre88Gmp { get; set; }
    public decimal Post88Gmp { get; set; }
    public decimal Excess { get; set; }
    public decimal Total => Pre88Gmp + Post88Gmp + Excess;
}
