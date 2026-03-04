namespace Module4.TDD;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Module 4: Test-Driven Development ===");
        Console.WriteLine();
        Console.WriteLine("This module is all about writing and running tests.");
        Console.WriteLine("Open the test files in tests/Module4.Tests/ to get started.");
        Console.WriteLine();

        // Demo: Revaluation Calculator
        Console.WriteLine("--- Revaluation Calculator ---");
        try
        {
            var calc = new RevaluationCalculator();
            var revalued = calc.FixedRateRevaluation(10_000m, 0.035m, 10);
            Console.WriteLine($"£10,000 revalued at 3.5% fixed for 10 years: £{revalued:N2}");

            var cpiRates = new List<decimal> { 0.02m, 0.03m, 0.025m, 0.04m, 0.01m };
            var cpiResult = calc.CpiCappedRevaluation(10_000m, cpiRates, 0.025m);
            Console.WriteLine($"£10,000 CPI-capped (2.5% cap) over 5 years: £{cpiResult:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Demo: Annual Allowance
        Console.WriteLine("--- Annual Allowance Calculator ---");
        try
        {
            var aaCalc = new AnnualAllowanceCalculator();
            var standardAA = aaCalc.GetAnnualAllowance(100_000m, 100_000m);
            Console.WriteLine($"Standard earner AA: £{standardAA:N0}");

            var taperedAA = aaCalc.GetAnnualAllowance(300_000m, 250_000m);
            Console.WriteLine($"High earner (£300k) AA: £{taperedAA:N0}");

            var charge = aaCalc.CalculateTaxCharge(80_000m, 60_000m, 0.40m);
            Console.WriteLine($"Tax charge on £80k input (AA £60k, 40% rate): £{charge:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Demo: Messy Calculator (works but needs refactoring)
        Console.WriteLine("--- Messy Calculator (refactoring exercise) ---");
        var messy = new MessyCalculator();
        var pension = messy.CalcPension(40_000m, 20, "60ths", 62, false);
        Console.WriteLine($"£40k salary, 20 yrs, 60ths, age 62, no lump sum: £{pension:N2}");
        pension = messy.CalcPension(40_000m, 20, "60ths", 65, true);
        Console.WriteLine($"£40k salary, 20 yrs, 60ths, age 65, with lump sum: £{pension:N2}");

        Console.WriteLine();
        Console.WriteLine("Run 'dotnet test --filter Module4' to check your answers!");
    }
}
