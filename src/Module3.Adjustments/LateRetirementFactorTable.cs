namespace Module3.Adjustments;

/// <summary>
/// Provides late retirement factors by age.
/// Late retirement factors are greater than 1.0 — they increase the pension
/// when a member retires after Normal Pension Age (65).
///
/// Typical factors:
///   Age 65 = 1.00, Age 66 = 1.05, Age 67 = 1.11,
///   Age 68 = 1.17, Age 69 = 1.24, Age 70 = 1.31
/// </summary>
public class LateRetirementFactorTable : IFactorTable
{
    // TODO: Create a Dictionary<int, decimal> with late retirement factors
    // Factors should be > 1.0 for ages above 65

    public decimal GetFactor(int age, string factorType)
    {
        // TODO: Look up the factor by age
        throw new NotImplementedException("Implement GetFactor");
    }

    public bool HasFactor(int age, string factorType)
    {
        // TODO: Check if the age exists
        throw new NotImplementedException("Implement HasFactor");
    }
}
