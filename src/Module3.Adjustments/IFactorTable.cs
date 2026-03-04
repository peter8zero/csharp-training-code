namespace Module3.Adjustments;

/// <summary>
/// Represents a table of actuarial factors used in pension calculations.
/// Factor tables map an age to a decimal factor for a given factor type.
/// </summary>
public interface IFactorTable
{
    /// <summary>
    /// Gets the factor for a given age and factor type.
    /// </summary>
    /// <param name="age">The member's age at the relevant date</param>
    /// <param name="factorType">The type of factor (e.g., "EarlyRetirement", "LateRetirement", "Commutation")</param>
    /// <returns>The actuarial factor as a decimal</returns>
    decimal GetFactor(int age, string factorType);

    /// <summary>
    /// Checks whether a factor exists for the given age and type.
    /// </summary>
    bool HasFactor(int age, string factorType);
}
