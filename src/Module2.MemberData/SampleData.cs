namespace Module2.MemberData;

public static class SampleData
{
    public static List<Member> GetMembers() => new()
    {
        new Member { Name = "Alice Smith", DateOfBirth = new DateTime(1970, 3, 15), JoinDate = new DateTime(1995, 6, 1), Salary = 45_000m, ServiceYears = 28, IsDeferred = false, SchemeType = "DB60ths" },
        new Member { Name = "Bob Jones", DateOfBirth = new DateTime(1965, 8, 22), JoinDate = new DateTime(1990, 1, 15), Salary = 52_000m, ServiceYears = 33, IsDeferred = false, SchemeType = "DB60ths" },
        new Member { Name = "Carol Williams", DateOfBirth = new DateTime(1980, 11, 5), JoinDate = new DateTime(2005, 4, 1), Salary = 38_000m, ServiceYears = 18, IsDeferred = false, SchemeType = "DB80ths" },
        new Member { Name = "David Brown", DateOfBirth = new DateTime(1975, 1, 30), JoinDate = new DateTime(2000, 9, 1), Salary = 61_000m, ServiceYears = 23, IsDeferred = false, SchemeType = "DB60ths" },
        new Member { Name = "Emma Taylor", DateOfBirth = new DateTime(1988, 6, 12), JoinDate = new DateTime(2012, 3, 1), Salary = 34_000m, ServiceYears = 11, IsDeferred = false, SchemeType = "CARE" },
        new Member { Name = "Frank Davies", DateOfBirth = new DateTime(1960, 4, 8), JoinDate = new DateTime(1985, 7, 1), Salary = 48_000m, ServiceYears = 30, IsDeferred = true, SchemeType = "DB60ths" },
        new Member { Name = "Grace Wilson", DateOfBirth = new DateTime(1972, 9, 25), JoinDate = new DateTime(1998, 2, 1), Salary = 55_000m, ServiceYears = 25, IsDeferred = false, SchemeType = "DB80ths" },
        new Member { Name = "Henry Moore", DateOfBirth = new DateTime(1985, 12, 3), JoinDate = new DateTime(2010, 6, 1), Salary = 42_000m, ServiceYears = 13, IsDeferred = false, SchemeType = "CARE" },
        new Member { Name = "Irene Clark", DateOfBirth = new DateTime(1968, 7, 19), JoinDate = new DateTime(1992, 11, 1), Salary = 67_000m, ServiceYears = 31, IsDeferred = true, SchemeType = "DB60ths" },
        new Member { Name = "James Lewis", DateOfBirth = new DateTime(1978, 2, 14), JoinDate = new DateTime(2003, 5, 1), Salary = 39_500m, ServiceYears = 20, IsDeferred = false, SchemeType = "DB80ths" },
        new Member { Name = "Karen Hall", DateOfBirth = new DateTime(1990, 10, 7), JoinDate = new DateTime(2015, 1, 1), Salary = 31_000m, ServiceYears = 8, IsDeferred = false, SchemeType = "CARE" },
        new Member { Name = "Liam Walker", DateOfBirth = new DateTime(1963, 5, 28), JoinDate = new DateTime(1988, 3, 1), Salary = 58_000m, ServiceYears = 35, IsDeferred = true, SchemeType = "DB60ths" },
    };
}
