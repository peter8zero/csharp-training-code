namespace Module1.PensionBasics;

public class PensionCalculator
{
    // Exercise 1: Calculate annual pension
    // Formula: salary * serviceYears * accrualRate
    // Example: £40,000 salary, 20 years service, 1/60th accrual = £40,000 * 20 * (1/60) = £13,333.33
    /// <summary>
    /// Calculates the annual pension based on final salary, years of service, and accrual rate.
    /// Formula: salary × serviceYears × accrualRate
    /// </summary>
    public decimal CalculateAnnualPension(decimal salary, int serviceYears, decimal accrualRate)
    {
        // TODO: Implement the pension calculation formula
        // Hint: Simply multiply the three values together
        //some comment
        //throw new NotImplementedException("Exercise 1: Implement CalculateAnnualPension");
    }

    // Exercise 2: Get the accrual rate from the enum
    // Sixtieths → 1/60 = 0.016666...
    // Eightieths → 1/80 = 0.0125
    /// <summary>
    /// Returns the decimal accrual rate for a given AccrualBasis.
    /// Sixtieths = 1/60, Eightieths = 1/80
    /// </summary>
    public decimal GetAccrualRate(AccrualBasis basis)
    {
        // TODO: Use an if/else or switch to return the correct rate
        // Hint: 1m/60m gives you a decimal result (the 'm' suffix means decimal literal)
        throw new NotImplementedException("Exercise 2: Implement GetAccrualRate");
    }

    // Exercise 3: Handle part-time service
    // A member who works 3 days out of 5 has a proportion of 0.6
    // Their pension = salary * serviceYears * accrualRate * partTimeProportion
    /// <summary>
    /// Calculates pension adjusted for part-time service.
    /// Formula: salary × serviceYears × accrualRate × partTimeProportion
    /// </summary>
    public decimal CalculatePartTimePension(decimal salary, int serviceYears, decimal accrualRate, decimal partTimeProportion)
    {
        // TODO: Implement the part-time pension calculation
        // Hint: It's the same as Exercise 1 but multiplied by the part-time proportion
        throw new NotImplementedException("Exercise 3: Implement CalculatePartTimePension");
    }

    // Exercise 4: Early retirement reduction
    // If a member retires before their Normal Pension Age (NPA), their pension is reduced.
    // The reduction is typically a percentage per year early.
    // Example: NPA is 65, retiring at 62 = 3 years early
    // If reduction is 4% per year, total reduction = 12%, so they get 88% of their pension
    /// <summary>
    /// Applies an early retirement reduction to a pension amount.
    /// If retirementAge >= normalPensionAge, no reduction is applied.
    /// Otherwise, reduction = yearsEarly × reductionPerYear (as a decimal, e.g., 0.04 for 4%)
    /// </summary>
    public decimal ApplyEarlyRetirementReduction(decimal annualPension, int retirementAge, int normalPensionAge, decimal reductionPerYear)
    {
        // TODO: Calculate years early, apply percentage reduction
        // Hint: If retiring at or after NPA, return the full pension
        // Hint: yearsEarly = normalPensionAge - retirementAge
        // Hint: reducedPension = annualPension * (1 - yearsEarly * reductionPerYear)
        throw new NotImplementedException("Exercise 4: Implement ApplyEarlyRetirementReduction");
    }
}
