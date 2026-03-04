namespace Module5.Advanced;

// Exercise 4: Create a CalculationResult record
// Records are immutable reference types — perfect for calculation outputs.
//
// TODO: Replace this class with a record that has:
//   - string MemberName
//   - string SchemeType
//   - int RetirementAge
//   - decimal BasePension (before adjustments)
//   - decimal AdjustedPension (after early/late retirement)
//   - decimal ResidualPension (after commutation)
//   - decimal LumpSum
//   - decimal CommutationPercentage
//
// Hint: public record CalculationResult(string MemberName, ...);

public record CalculationResult
{
    // TODO: Replace this with a positional record or init-only properties
    // Example of a record with init properties:
    // public string MemberName { get; init; } = string.Empty;

    public string MemberName { get; init; } = string.Empty;
    public string SchemeType { get; init; } = string.Empty;
    public int RetirementAge { get; init; }
    public decimal BasePension { get; init; }
    public decimal AdjustedPension { get; init; }
    public decimal ResidualPension { get; init; }
    public decimal LumpSum { get; init; }
    public decimal CommutationPercentage { get; init; }
}

// Provided: Scheme and Member types used by the builder
public class Scheme
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "DB60ths", "DB80ths", "CARE"
    public int NormalPensionAge { get; set; } = 65;
    public decimal EarlyRetirementReductionPerYear { get; set; } = 0.04m;
    public decimal LateRetirementIncreasePerYear { get; set; } = 0.05m;
    public decimal CommutationFactor { get; set; } = 15.0m;
}

public class MemberRecord
{
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int ServiceYears { get; set; }
    public string SchemeType { get; set; } = string.Empty;

    public decimal GetAccrualRate() => SchemeType switch
    {
        "DB60ths" => 1m / 60m,
        "DB80ths" => 1m / 80m,
        "CARE" => 1m / 49m,
        _ => throw new ArgumentException($"Unknown scheme type: {SchemeType}")
    };
}
