namespace Module4.TDD;

/// <summary>
/// REFACTORING EXERCISE
///
/// This calculator works correctly — all tests pass — but the code is messy.
/// Your task: refactor it to be cleaner while keeping all tests green.
///
/// Things to improve:
/// - Magic numbers → named constants or parameters
/// - Duplicated logic → extract methods
/// - Long method → break into smaller, focused methods
/// - Poor naming → rename for clarity
/// </summary>
public class MessyCalculator
{
    // This method calculates everything in one go — it's correct but ugly
    public decimal CalcPension(decimal sal, int yrs, string type, int age, bool wantLumpSum)
    {
        decimal p = 0;

        // calc basic pension
        if (type == "60ths")
        {
            p = sal * yrs / 60m;
        }
        else if (type == "80ths")
        {
            p = sal * yrs / 80m;
        }
        else if (type == "CARE")
        {
            p = sal * yrs / 49m;
        }

        // early retirement
        if (age < 65)
        {
            int e = 65 - age;
            decimal r = 1m - (e * 0.04m);
            if (r < 0) r = 0;
            p = p * r;
        }

        // late retirement
        if (age > 65)
        {
            int l = age - 65;
            decimal b = 1m + (l * 0.05m);
            p = p * b;
        }

        // commutation
        if (wantLumpSum)
        {
            decimal commFactor = 15.0m;
            if (age < 60) commFactor = 18.5m;
            else if (age < 65) commFactor = 16.0m;
            else if (age > 65) commFactor = 13.0m;

            decimal lumpSum = (p * 0.25m) * commFactor;
            p = p * 0.75m;
            // Note: lump sum is calculated but we only return the residual pension
            // In a real system you'd return both
        }

        return Math.Round(p, 2);
    }
}
