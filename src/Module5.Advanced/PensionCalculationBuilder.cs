namespace Module5.Advanced;

/// <summary>
/// A fluent builder for constructing pension calculations step by step.
///
/// Usage:
///   var result = new PensionCalculationBuilder()
///       .ForMember(member)
///       .WithScheme(scheme)
///       .AtRetirementAge(63)
///       .WithCommutation(0.25m)
///       .Build();
///
/// The builder pattern lets you chain method calls in a readable way.
/// Each method returns 'this' (the builder itself) to enable chaining.
/// </summary>
public class PensionCalculationBuilder
{
    // TODO: Add private fields to store the configuration
    // Hint:
    private MemberRecord? _member;
    private Scheme? _scheme;
    private int? _retirementAge;
    private decimal _commutationPercentage;

    /// <summary>
    /// Sets the member for the calculation.
    /// </summary>
    public PensionCalculationBuilder ForMember(MemberRecord member)
    {
        // TODO: Store the member and return 'this' for chaining
        // Hint: _member = member; return this;
        throw new NotImplementedException("Exercise 1: Implement ForMember");
    }

    /// <summary>
    /// Sets the scheme rules for the calculation.
    /// </summary>
    public PensionCalculationBuilder WithScheme(Scheme scheme)
    {
        // TODO: Store the scheme and return this
        throw new NotImplementedException("Exercise 1: Implement WithScheme");
    }

    /// <summary>
    /// Sets the retirement age. If not called, defaults to scheme's NPA.
    /// </summary>
    public PensionCalculationBuilder AtRetirementAge(int age)
    {
        // TODO: Store the retirement age and return this
        throw new NotImplementedException("Exercise 1: Implement AtRetirementAge");
    }

    /// <summary>
    /// Sets the commutation percentage (0.0 to 1.0). Default is 0 (no commutation).
    /// </summary>
    public PensionCalculationBuilder WithCommutation(decimal percentage)
    {
        // TODO: Store the commutation percentage and return this
        throw new NotImplementedException("Exercise 1: Implement WithCommutation");
    }

    /// <summary>
    /// Builds and returns the calculation result.
    ///
    /// Steps:
    /// 1. Validate that member and scheme are set
    /// 2. Calculate base pension = salary × serviceYears × accrualRate
    /// 3. Apply early/late retirement adjustment
    /// 4. Apply commutation if requested
    /// 5. Return a CalculationResult with all values
    /// </summary>
    public CalculationResult Build()
    {
        // TODO: Implement the build logic
        //
        // Exercise 5 (Bonus): Add validation
        // if (_member is null) throw new InvalidOperationException("Member is required");
        // if (_scheme is null) throw new InvalidOperationException("Scheme is required");
        //
        // Step 1: Get retirement age (use scheme NPA if not specified)
        // var retirementAge = _retirementAge ?? _scheme.NormalPensionAge;
        //
        // Step 2: Calculate base pension
        // var basePension = _member.Salary * _member.ServiceYears * _member.GetAccrualRate();
        //
        // Step 3: Apply early/late retirement
        // var adjustedPension = basePension;
        // if (retirementAge < _scheme.NormalPensionAge)
        // {
        //     int yearsEarly = _scheme.NormalPensionAge - retirementAge;
        //     adjustedPension *= (1 - yearsEarly * _scheme.EarlyRetirementReductionPerYear);
        //     if (adjustedPension < 0) adjustedPension = 0;
        // }
        // else if (retirementAge > _scheme.NormalPensionAge)
        // {
        //     int yearsLate = retirementAge - _scheme.NormalPensionAge;
        //     adjustedPension *= (1 + yearsLate * _scheme.LateRetirementIncreasePerYear);
        // }
        //
        // Step 4: Apply commutation
        // var commutedAmount = adjustedPension * _commutationPercentage;
        // var lumpSum = commutedAmount * _scheme.CommutationFactor;
        // var residualPension = adjustedPension - commutedAmount;
        //
        // Step 5: Return result
        throw new NotImplementedException("Exercise 1: Implement Build");
    }

    /// <summary>
    /// Resets the builder to its initial state so it can be reused.
    /// </summary>
    public PensionCalculationBuilder Reset()
    {
        _member = null;
        _scheme = null;
        _retirementAge = null;
        _commutationPercentage = 0;
        return this;
    }
}
