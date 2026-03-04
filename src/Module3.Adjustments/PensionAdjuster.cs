namespace Module3.Adjustments;

/// <summary>
/// Adjusts a pension amount using actuarial factors from an injected factor table.
/// This demonstrates dependency injection — the PensionAdjuster doesn't know
/// which specific factor table it's using, only that it implements IFactorTable.
/// </summary>
public class PensionAdjuster
{
    private readonly IFactorTable _factorTable;

    // Exercise 3: Constructor injection
    // The factor table is passed in via the constructor
    public PensionAdjuster(IFactorTable factorTable)
    {
        _factorTable = factorTable;
    }

    /// <summary>
    /// Adjusts a pension by applying the factor for the given age.
    /// Result = annualPension × factor
    /// </summary>
    public decimal AdjustPension(decimal annualPension, int retirementAge)
    {
        // TODO: Get the factor from _factorTable and multiply by the pension
        // Hint: var factor = _factorTable.GetFactor(retirementAge, "Retirement");
        throw new NotImplementedException("Exercise 3: Implement AdjustPension");
    }

    /// <summary>
    /// Adjusts pension with a fallback: if no factor exists for the age, returns the pension unchanged.
    /// </summary>
    public decimal AdjustPensionSafe(decimal annualPension, int retirementAge)
    {
        // TODO: Use HasFactor to check first, return unadjusted pension if no factor exists
        throw new NotImplementedException("Exercise 3: Implement AdjustPensionSafe");
    }
}
