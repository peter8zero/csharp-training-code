namespace Module3.Adjustments;

/// <summary>
/// Provides early retirement factors by age.
/// Early retirement factors are less than 1.0 — they reduce the pension
/// when a member retires before Normal Pension Age (65).
///
/// Typical factors:
///   Age 55 = 0.55, Age 56 = 0.58, Age 57 = 0.61, Age 58 = 0.65,
///   Age 59 = 0.70, Age 60 = 0.76, Age 61 = 0.82, Age 62 = 0.88,
///   Age 63 = 0.92, Age 64 = 0.96, Age 65 = 1.00
/// </summary>
public class EarlyRetirementFactorTable : IFactorTable
{
    // TODO: Create a Dictionary<int, decimal> to store early retirement factors
    // Hint: private readonly Dictionary<int, decimal> _factors = new()
    // {
    //     { 55, 0.55m }, { 56, 0.58m }, ... etc
    // };

    public decimal GetFactor(int age, string factorType)
    {
        // TODO: Look up the factor by age in the dictionary
        // If the age isn't found, throw a KeyNotFoundException with a helpful message
        // Ignore factorType for this table (it only has one type)
        throw new NotImplementedException("Implement GetFactor using the dictionary");
    }

    public bool HasFactor(int age, string factorType)
    {
        // TODO: Check if the dictionary contains the given age
        throw new NotImplementedException("Implement HasFactor");
    }
}
