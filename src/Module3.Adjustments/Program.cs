namespace Module3.Adjustments;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Module 3: Pension Adjustments ===");
        Console.WriteLine();

        // Exercises 1-2: Factor tables
        Console.WriteLine("--- Exercises 1-2: Factor Tables ---");
        try
        {
            var earlyFactors = new EarlyRetirementFactorTable();
            Console.WriteLine($"Early retirement factor at age 60: {earlyFactors.GetFactor(60, "EarlyRetirement")}");
            Console.WriteLine($"Early retirement factor at age 63: {earlyFactors.GetFactor(63, "EarlyRetirement")}");

            var lateFactors = new LateRetirementFactorTable();
            Console.WriteLine($"Late retirement factor at age 67: {lateFactors.GetFactor(67, "LateRetirement")}");
            Console.WriteLine($"Late retirement factor at age 70: {lateFactors.GetFactor(70, "LateRetirement")}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 3: PensionAdjuster with DI
        Console.WriteLine("--- Exercise 3: Pension Adjuster (DI) ---");
        try
        {
            var earlyTable = new EarlyRetirementFactorTable();
            var adjuster = new PensionAdjuster(earlyTable);
            var pension = 20_000m;
            var adjusted = adjuster.AdjustPension(pension, 60);
            Console.WriteLine($"Pension £{pension:N2} at age 60 → £{adjusted:N2}");

            var lateTable = new LateRetirementFactorTable();
            var lateAdjuster = new PensionAdjuster(lateTable);
            adjusted = lateAdjuster.AdjustPension(pension, 68);
            Console.WriteLine($"Pension £{pension:N2} at age 68 → £{adjusted:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 4: Commutation
        Console.WriteLine("--- Exercise 4: Commutation ---");
        try
        {
            // Use a simple commutation factor table
            var commFactors = new CommutationFactorTable();
            var commCalc = new CommutationCalculator(commFactors);
            var result = commCalc.Commute(20_000m, 0.25m, 65);
            Console.WriteLine($"Commuting 25% of £20,000 pension at age 65:");
            Console.WriteLine($"  Lump sum: £{result.LumpSum:N2}");
            Console.WriteLine($"  Residual pension: £{result.ResidualPension:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercises 5-6: GMP
        Console.WriteLine("--- Exercises 5-6: GMP Calculations ---");
        try
        {
            var gmpCalc = new GmpCalculator();
            var split = gmpCalc.SplitPension(20_000m, 3_000m, 2_000m);
            Console.WriteLine($"Total: £{split.Total:N2}");
            Console.WriteLine($"  Pre-88 GMP: £{split.Pre88Gmp:N2}");
            Console.WriteLine($"  Post-88 GMP: £{split.Post88Gmp:N2}");
            Console.WriteLine($"  Excess: £{split.Excess:N2}");

            Console.WriteLine();
            var revalued = gmpCalc.RevalueGmp(3_000m, RevaluationType.Fixed, 10);
            Console.WriteLine($"£3,000 GMP revalued (Fixed 3.5%) over 10 years: £{revalued:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }

        Console.WriteLine();
        Console.WriteLine("Run 'dotnet test --filter Module3' to check your answers!");
    }
}

// A simple commutation factor table for the demo
// (This is provided complete — not a TODO exercise)
public class CommutationFactorTable : IFactorTable
{
    private readonly Dictionary<int, decimal> _factors = new()
    {
        { 55, 22.0m }, { 56, 21.5m }, { 57, 21.0m }, { 58, 20.3m },
        { 59, 19.5m }, { 60, 18.5m }, { 61, 17.8m }, { 62, 17.0m },
        { 63, 16.2m }, { 64, 15.5m }, { 65, 15.0m }, { 66, 14.2m },
        { 67, 13.5m }, { 68, 12.7m }, { 69, 12.0m }, { 70, 11.5m }
    };

    public decimal GetFactor(int age, string factorType) =>
        _factors.TryGetValue(age, out var factor) ? factor : throw new KeyNotFoundException($"No commutation factor for age {age}");

    public bool HasFactor(int age, string factorType) => _factors.ContainsKey(age);
}
