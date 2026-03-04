namespace Module2.MemberData;

public class Member
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinDate { get; set; }
    public decimal Salary { get; set; }
    public int ServiceYears { get; set; }
    public bool IsDeferred { get; set; }
    public string SchemeType { get; set; } = string.Empty; // "DB60ths", "DB80ths", "CARE"
}

public record MemberSummary(string Name, decimal AnnualPension, int YearsToRetirement);
