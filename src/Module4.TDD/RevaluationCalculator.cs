namespace Module4.TDD;

/// <summary>
/// Calculates deferred pension revaluation — how a deferred pension
/// increases between leaving the scheme and retirement.
///
/// Three revaluation methods:
/// 1. Fixed rate: pension × (1 + rate)^years
/// 2. Compound: same as fixed but potentially different rate
/// 3. CPI-capped: annual increase is the lesser of CPI and a cap rate
///    (applied year by year since CPI varies)
/// </summary>
public class RevaluationCalculator
{
    /// <summary>
    /// Applies fixed-rate compound revaluation over N years.
    /// Formula: amount × (1 + fixedRate)^years
    /// </summary>
    public decimal FixedRateRevaluation(decimal amount, decimal fixedRate, int years)
    {
        // TODO: Implement fixed rate revaluation
        // Hint: Use a for loop multiplying by (1 + fixedRate) each year
        // Or: amount * (decimal)Math.Pow((double)(1 + fixedRate), years)
        throw new NotImplementedException("Implement FixedRateRevaluation");
    }

    /// <summary>
    /// Applies compound revaluation at a given annual rate.
    /// Same formula as fixed rate but semantically different — this is used
    /// when the rate represents a compound growth rate.
    /// </summary>
    public decimal CompoundRevaluation(decimal amount, decimal annualRate, int years)
    {
        // TODO: Implement compound revaluation
        throw new NotImplementedException("Implement CompoundRevaluation");
    }

    /// <summary>
    /// Applies CPI-capped revaluation year by year.
    /// Each year's increase = min(cpiRate for that year, capRate).
    /// The cpiRates list contains one rate per year of deferral.
    /// </summary>
    public decimal CpiCappedRevaluation(decimal amount, IList<decimal> annualCpiRates, decimal capRate)
    {
        // TODO: Loop through each year's CPI rate
        // Apply min(cpiRate, capRate) as that year's increase
        // Compound: amount = amount * (1 + min(cpi, cap))
        throw new NotImplementedException("Implement CpiCappedRevaluation");
    }
}
