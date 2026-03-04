namespace Module2.MemberData;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Module 2: Member Data & LINQ ===");
        Console.WriteLine();

        var members = SampleData.GetMembers();
        var queries = new MemberQueries(members);
        var today = new DateTime(2026, 2, 27);

        Console.WriteLine($"Total members in dataset: {members.Count}");
        Console.WriteLine();

        // Exercise 1
        Console.WriteLine("--- Exercise 1: Members by Scheme ---");
        try
        {
            var db60 = queries.GetMembersByScheme("DB60ths");
            Console.WriteLine($"DB60ths members: {db60.Count}");
            foreach (var m in db60) Console.WriteLine($"  {m.Name} - £{m.Salary:N0}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 2
        Console.WriteLine("--- Exercise 2: Active Members ---");
        try
        {
            var active = queries.GetActiveMembers();
            Console.WriteLine($"Active members: {active.Count} (of {members.Count} total)");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 3
        Console.WriteLine("--- Exercise 3: Total Liability ---");
        try
        {
            var total = queries.CalculateTotalLiability();
            Console.WriteLine($"Total annual pension liability: £{total:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 4
        Console.WriteLine("--- Exercise 4: Average Salary by Scheme ---");
        try
        {
            var averages = queries.GetAverageSalaryByScheme();
            foreach (var kvp in averages)
                Console.WriteLine($"  {kvp.Key}: £{kvp.Value:N2}");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 5
        Console.WriteLine("--- Exercise 5: Approaching Retirement ---");
        try
        {
            var approaching = queries.GetMembersApproachingRetirement(today);
            Console.WriteLine($"Members within 5 years of NPA: {approaching.Count}");
            foreach (var m in approaching)
            {
                int age = today.Year - m.DateOfBirth.Year;
                if (m.DateOfBirth.Date > today.AddYears(-age)) age--;
                Console.WriteLine($"  {m.Name} (age {age})");
            }
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 6
        Console.WriteLine("--- Exercise 6: Ordered by Pension ---");
        try
        {
            var ordered = queries.GetMembersOrderedByPension();
            foreach (var m in ordered.Take(5))
            {
                var pension = m.Salary * m.ServiceYears * MemberQueries.GetAccrualRate(m.SchemeType);
                Console.WriteLine($"  {m.Name}: £{pension:N2}/year");
            }
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }
        Console.WriteLine();

        // Exercise 7
        Console.WriteLine("--- Exercise 7: Member Summaries ---");
        try
        {
            var summaries = queries.GetMemberSummaries(today);
            foreach (var s in summaries.Take(5))
                Console.WriteLine($"  {s.Name}: £{s.AnnualPension:N2}/year, {s.YearsToRetirement} years to retirement");
        }
        catch (NotImplementedException ex) { Console.WriteLine($"  {ex.Message}"); }

        Console.WriteLine();
        Console.WriteLine("Run 'dotnet test --filter Module2' to check your answers!");
    }
}
