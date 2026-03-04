namespace Module1.PensionBasics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Module 1: Pension Basics ===");
        Console.WriteLine();

        var calculator = new PensionCalculator();

        // Exercise 1: Basic pension calculation
        Console.WriteLine("--- Exercise 1: Annual Pension ---");
        try
        {
            decimal pension = calculator.CalculateAnnualPension(40_000m, 20, 1m / 60m);
            Console.WriteLine($"Salary: £40,000 | Service: 20 years | Accrual: 1/60th");
            Console.WriteLine($"Annual Pension: £{pension:N2}");
            // Expected: £13,333.33
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine($"  Not yet implemented: {ex.Message}");
        }
        Console.WriteLine();

        // Exercise 2: Accrual rates from enum
        Console.WriteLine("--- Exercise 2: Accrual Rates ---");
        try
        {
            decimal sixtieths = calculator.GetAccrualRate(AccrualBasis.Sixtieths);
            decimal eightieths = calculator.GetAccrualRate(AccrualBasis.Eightieths);
            Console.WriteLine($"Sixtieths rate: {sixtieths:F6} (expected: 0.016667)");
            Console.WriteLine($"Eightieths rate: {eightieths:F6} (expected: 0.012500)");
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine($"  Not yet implemented: {ex.Message}");
        }
        Console.WriteLine();

        // Exercise 3: Part-time pension
        Console.WriteLine("--- Exercise 3: Part-Time Pension ---");
        try
        {
            decimal partTimePension = calculator.CalculatePartTimePension(40_000m, 20, 1m / 60m, 0.6m);
            Console.WriteLine($"Salary: £40,000 | Service: 20 years | Accrual: 1/60th | Part-time: 0.6");
            Console.WriteLine($"Part-Time Pension: £{partTimePension:N2}");
            // Expected: £8,000.00
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine($"  Not yet implemented: {ex.Message}");
        }
        Console.WriteLine();

        // Exercise 4: Early retirement
        Console.WriteLine("--- Exercise 4: Early Retirement ---");
        try
        {
            decimal fullPension = 13_333.33m;
            decimal reduced = calculator.ApplyEarlyRetirementReduction(fullPension, 62, 65, 0.04m);
            Console.WriteLine($"Full pension: £{fullPension:N2} | Retiring at 62 (NPA 65) | 4% per year reduction");
            Console.WriteLine($"Reduced Pension: £{reduced:N2}");
            // Expected: £11,733.33 (88% of £13,333.33)
        }
        catch (NotImplementedException ex)
        {
            Console.WriteLine($"  Not yet implemented: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Run 'dotnet test --filter Module1' to check your answers!");
    }
}
