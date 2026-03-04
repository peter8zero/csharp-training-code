using Module4.TDD;
using Xunit;

namespace Module4.Tests;

/// <summary>
/// EXERCISE: Write tests for AnnualAllowanceCalculator
///
/// This file contains a few example tests to get you started.
/// Your task is to:
/// 1. Write the remaining tests FIRST (before implementing the code)
/// 2. Run the tests — they should all fail (RED)
/// 3. Implement the code in AnnualAllowanceCalculator.cs
/// 4. Run the tests again — they should pass (GREEN)
///
/// Test ideas (write at least 6 more tests):
/// - Standard AA for normal earner
/// - MPAA when hasMpaa is true
/// - Tapered AA at various income levels
/// - Tapered AA at minimum (£10,000)
/// - Threshold income below £200k (no taper even if adjusted income high)
/// - Tax charge when below AA (should be 0)
/// - Tax charge when above AA
/// - Negative pension input validation
/// - Invalid tax rate validation
/// - Exactly on threshold (boundary testing)
/// </summary>
public class AnnualAllowanceCalculatorTests
{
    private readonly AnnualAllowanceCalculator _calculator = new();

    // --- Example tests (provided) ---

    [Fact]
    public void GetAnnualAllowance_StandardEarner_Returns60000()
    {
        var result = _calculator.GetAnnualAllowance(100_000m, 100_000m);
        Assert.Equal(60_000m, result);
    }

    [Fact]
    public void GetAnnualAllowance_WithMpaa_Returns10000()
    {
        var result = _calculator.GetAnnualAllowance(100_000m, 100_000m, hasMpaa: true);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void CalculateTaxCharge_BelowAllowance_ReturnsZero()
    {
        var result = _calculator.CalculateTaxCharge(50_000m, 60_000m, 0.40m);
        Assert.Equal(0m, result);
    }

    // --- TODO: Write your own tests below ---
    // Follow the Red-Green-Refactor cycle:
    // 1. Write a failing test
    // 2. Run it to see it fail
    // 3. Write the minimum code to make it pass
    // 4. Refactor if needed

    // Example of a test you might write:
    // [Fact]
    // public void GetAnnualAllowance_HighEarner_ReturnsTaperedAmount()
    // {
    //     // Adjusted income £300,000, threshold income £250,000
    //     // Taper: 60,000 - ((300,000 - 260,000) / 2) = 60,000 - 20,000 = 40,000
    //     var result = _calculator.GetAnnualAllowance(300_000m, 250_000m);
    //     Assert.Equal(40_000m, result);
    // }

    // TODO: Test tapered AA at various income levels

    // TODO: Test tapered AA minimum floor (£10,000)

    // TODO: Test that taper doesn't apply when threshold income <= £200,000

    // TODO: Test CalculateTaxCharge above allowance

    // TODO: Test CalculateTaxChargeSafe with negative input

    // TODO: Test CalculateTaxChargeSafe with invalid tax rate

    // TODO: Test boundary case — exactly on threshold
}
