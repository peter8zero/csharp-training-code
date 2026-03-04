namespace Module5.Advanced;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Module 5: Builder Pattern & Advanced LINQ ===");
        Console.WriteLine();

        // Sample data
        var scheme60 = new Scheme
        {
            Name = "Acme DB 60ths", Type = "DB60ths", NormalPensionAge = 65,
            EarlyRetirementReductionPerYear = 0.04m, LateRetirementIncreasePerYear = 0.05m,
            CommutationFactor = 15.0m
        };

        var members = new List<MemberRecord>
        {
            new() { Name = "Alice", Salary = 45_000m, ServiceYears = 28, SchemeType = "DB60ths" },
            new() { Name = "Bob", Salary = 52_000m, ServiceYears = 33, SchemeType = "DB60ths" },
            new() { Name = "Carol", Salary = 38_000m, ServiceYears = 18, SchemeType = "DB80ths" },
            new() { Name = "David", Salary = 61_000m, ServiceYears = 23, SchemeType = "DB60ths" },
            new() { Name = "Emma", Salary = 34_000m, ServiceYears = 11, SchemeType = "CARE" },
        };

        // Exercise 1: Builder pattern
        Console.WriteLine("--- Exercise 1: Pension Calculation Builder ---");
        try
        {
            var builder = new PensionCalculationBuilder();
            var result = builder
                .ForMember(members[0])
                .WithScheme(scheme60)
                .AtRetirementAge(63)
                .WithCommutation(0.25m)
                .Build();

            Console.WriteLine($"Member: {result.MemberName}");
            Console.WriteLine($"  Base pension: \u00a3{result.BasePension:N2}");
            Console.WriteLine($"  Adjusted (age 63): \u00a3{result.AdjustedPension:N2}");
            Console.WriteLine($"  After commutation: \u00a3{result.ResidualPension:N2}");
            Console.WriteLine($"  Lump sum: \u00a3{result.LumpSum:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 2: Run builder across list of members
        Console.WriteLine("--- Exercise 2: Batch Calculations ---");
        try
        {
            var builder = new PensionCalculationBuilder();
            var results = members.Select(m =>
                builder.Reset()
                    .ForMember(m)
                    .WithScheme(scheme60)
                    .WithCommutation(0.25m)
                    .Build()
            ).ToList();

            foreach (var r in results)
                Console.WriteLine($"  {r.MemberName}: \u00a3{r.AdjustedPension:N2}/year, lump sum \u00a3{r.LumpSum:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 3: Advanced LINQ analytics
        Console.WriteLine("--- Exercise 3: Scheme Analytics ---");
        try
        {
            var builder = new PensionCalculationBuilder();
            var results = members.Select((m, i) =>
                builder.Reset()
                    .ForMember(m)
                    .WithScheme(scheme60)
                    .AtRetirementAge(60 + i)
                    .WithCommutation(0.25m)
                    .Build()
            ).ToList();

            var analytics = new SchemeAnalytics(results);

            var liability = analytics.GetTotalLiabilityByScheme();
            Console.WriteLine("Total liability by scheme:");
            foreach (var kvp in liability)
                Console.WriteLine($"  {kvp.Key}: \u00a3{kvp.Value:N2}");

            var highEarners = analytics.GetHighPensionMembers(15_000m);
            Console.WriteLine($"\nMembers with pension above \u00a315k: {string.Join(", ", highEarners)}");

            var summary = analytics.GetSummary();
            Console.WriteLine($"\nSummary:");
            Console.WriteLine($"  Total members: {summary.TotalMembers}");
            Console.WriteLine($"  Average pension: \u00a3{summary.AveragePension:N2}");
            Console.WriteLine($"  Highest: \u00a3{summary.HighestPension:N2} ({summary.HighestPensionMember})");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }

        Console.WriteLine();
        Console.WriteLine("Run 'dotnet test --filter Module5' to check your answers!");
    }
}
