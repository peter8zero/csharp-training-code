namespace Module4.TDD;

/// <summary>
/// Calculates UK pension Annual Allowance tax charges.
///
/// The Annual Allowance (AA) is the maximum amount of pension savings
/// that can benefit from tax relief in a year.
///
/// Key thresholds (2024/25):
/// - Standard AA: £60,000
/// - Money Purchase AA (MPAA): £10,000 (for those who have flexibly accessed DC pensions)
/// - Tapered AA: For high earners with 'adjusted income' over £260,000,
///   AA reduces by £1 for every £2 over £260,000, down to a minimum of £10,000
///   (Threshold income must also exceed £200,000 for taper to apply)
/// </summary>
public class AnnualAllowanceCalculator
{
    public const decimal StandardAA = 60_000m;
    public const decimal Mpaa = 10_000m;
    public const decimal TaperThreshold = 260_000m;
    public const decimal ThresholdIncome = 200_000m;
    public const decimal MinTaperedAA = 10_000m;

    /// <summary>
    /// Gets the applicable Annual Allowance for a member.
    /// If hasMpaa is true, returns MPAA (£10,000).
    /// If adjustedIncome > £260,000 AND thresholdIncome > £200,000,
    /// applies taper: AA = £60,000 - ((adjustedIncome - £260,000) / 2), minimum £10,000
    /// Otherwise returns standard AA (£60,000).
    /// </summary>
    public decimal GetAnnualAllowance(decimal adjustedIncome, decimal thresholdIncome, bool hasMpaa = false)
    {
        // TODO: Implement the AA calculation logic
        // 1. If hasMpaa, return MPAA
        // 2. If both income thresholds exceeded, calculate tapered AA
        // 3. Otherwise return standard AA
        throw new NotImplementedException("Implement GetAnnualAllowance");
    }

    /// <summary>
    /// Calculates the tax charge on pension savings that exceed the Annual Allowance.
    /// Tax charge = (pensionInput - annualAllowance) × marginalTaxRate
    /// If pensionInput <= annualAllowance, no tax charge (return 0).
    /// </summary>
    public decimal CalculateTaxCharge(decimal pensionInput, decimal annualAllowance, decimal marginalTaxRate)
    {
        // TODO: Calculate the excess and apply tax rate
        // Hint: If pensionInput <= annualAllowance, return 0
        throw new NotImplementedException("Implement CalculateTaxCharge");
    }

    /// <summary>
    /// Validates inputs and throws ArgumentException for invalid values.
    /// - pensionInput must be >= 0
    /// - marginalTaxRate must be between 0 and 1
    /// </summary>
    public decimal CalculateTaxChargeSafe(decimal pensionInput, decimal annualAllowance, decimal marginalTaxRate)
    {
        // TODO: Validate inputs then delegate to CalculateTaxCharge
        throw new NotImplementedException("Implement CalculateTaxChargeSafe");
    }
}
