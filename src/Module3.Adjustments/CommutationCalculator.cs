namespace Module3.Adjustments;

/// <summary>
/// Handles pension commutation — converting part of an annual pension
/// into a tax-free lump sum.
///
/// In UK DB pensions, members can typically commute up to 25% of their
/// pension fund value. The lump sum = commuted pension × commutation factor.
/// The commutation factor depends on the member's age at retirement.
///
/// Example commutation factors by age:
///   Age 55 = 22.0, Age 60 = 18.5, Age 65 = 15.0, Age 70 = 11.5
/// </summary>
public class CommutationCalculator
{
    private readonly IFactorTable _factorTable;

    public CommutationCalculator(IFactorTable factorTable)
    {
        _factorTable = factorTable;
    }

    /// <summary>
    /// Calculates the results of commuting a percentage of pension to lump sum.
    /// </summary>
    /// <param name="annualPension">The full annual pension before commutation</param>
    /// <param name="commutationPercentage">Percentage to commute (0.0 to 1.0, e.g., 0.25 for 25%)</param>
    /// <param name="age">Member's age at retirement</param>
    /// <returns>A CommutationResult with the residual pension and lump sum</returns>
    public CommutationResult Commute(decimal annualPension, decimal commutationPercentage, int age)
    {
        // TODO: Calculate:
        //   commutedPension = annualPension × commutationPercentage
        //   lumpSum = commutedPension × commutationFactor (from factor table)
        //   residualPension = annualPension - commutedPension
        // Return a new CommutationResult
        throw new NotImplementedException("Exercise 4: Implement Commute");
    }
}

/// <summary>
/// The result of a pension commutation calculation.
/// </summary>
public class CommutationResult
{
    public decimal ResidualPension { get; set; }
    public decimal LumpSum { get; set; }
    public decimal CommutedPension { get; set; }
}
