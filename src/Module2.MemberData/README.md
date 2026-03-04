# Module 2: Member Data & LINQ

## Overview

This module teaches you how to work with collections of data in C# using
`List<T>`, custom classes, and LINQ (Language Integrated Query). The domain
is UK defined benefit pension scheme member data -- you will filter, sort,
group, and calculate pension entitlements using LINQ methods.

By the end of this module you will be able to:

- Define classes with auto-properties
- Work with `List<T>` and other generic collections
- Use LINQ to query, filter, sort, and aggregate data
- Write lambda expressions
- Understand the difference between records and classes

---

## Collections in C#

### Why generics matter

In older C# (and languages like VB6), collections stored everything as
`object`, so you had to cast items back to the right type. Generics let you
declare a collection that only holds a specific type:

```csharp
// Non-generic (avoid this)
ArrayList list = new ArrayList();
list.Add("hello");
list.Add(42);  // no compile error, but mixing types causes bugs

// Generic (use this)
List<string> names = new List<string>();
names.Add("Alice");
// names.Add(42);  // compile error -- caught immediately
```

### List<T> basics

`List<T>` is the workhorse collection in C#. It is a resizable array that
stores elements of type `T`.

```csharp
var salaries = new List<decimal> { 45_000m, 52_000m, 38_000m };

salaries.Add(61_000m);          // append
salaries.Remove(38_000m);       // remove first match
int count = salaries.Count;     // 3
decimal first = salaries[0];    // 45000 (index access)
```

Other useful collections you will encounter:

| Type | Use case |
|------|----------|
| `List<T>` | Ordered, resizable list |
| `Dictionary<TKey, TValue>` | Key-value lookup |
| `HashSet<T>` | Unique values, fast lookup |
| `Queue<T>` / `Stack<T>` | FIFO / LIFO processing |

---

## Classes and Properties

### Auto-properties

A property is a member that provides a flexible mechanism to read or write a
value. Auto-properties let you declare them concisely:

```csharp
public class Member
{
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public bool IsDeferred { get; set; }
}
```

This is equivalent to writing a private backing field plus getter and setter
methods, but the compiler does it for you. The `= string.Empty` part is a
default value, which avoids null reference warnings.

### Creating instances

```csharp
var member = new Member
{
    Name = "Alice Smith",
    Salary = 45_000m,
    IsDeferred = false
};
```

This is called an **object initialiser**. You can also set properties after
construction:

```csharp
var member = new Member();
member.Name = "Alice Smith";
member.Salary = 45_000m;
```

### Read-only properties

If you want a property that can only be set in the constructor or initialiser:

```csharp
public string Name { get; init; } = string.Empty;
```

The `init` accessor replaces `set` and makes the property immutable after
construction.

---

## Records vs Classes

C# 9 introduced **records** -- types designed to hold immutable data with
built-in value equality:

```csharp
// A record with positional parameters
public record MemberSummary(string Name, decimal AnnualPension, int YearsToRetirement);

// Usage
var summary = new MemberSummary("Alice", 21_000m, 9);
Console.WriteLine(summary.Name);            // Alice
Console.WriteLine(summary);                 // MemberSummary { Name = Alice, ... }
```

Key differences from classes:

| Feature | Class | Record |
|---------|-------|--------|
| Equality | Reference (same object) | Value (same data) |
| Mutability | Mutable by default | Immutable by default |
| `ToString()` | Type name only | All property values |
| Use case | Entities with identity | Data transfer objects |

In pension systems, records are great for calculation results and summaries,
while classes suit entities like members that change over time.

---

## LINQ Fundamentals

LINQ lets you query collections using a consistent syntax. It works with any
`IEnumerable<T>`, which includes `List<T>`, arrays, and database results.

You need this using directive (already included in modern .NET via global
usings):

```csharp
using System.Linq;
```

### Method syntax vs query syntax

C# supports two ways to write LINQ. This course uses **method syntax**
because it is more common in production code, but you should recognise both:

```csharp
// Method syntax (preferred)
var highEarners = members.Where(m => m.Salary > 50_000m).ToList();

// Query syntax (SQL-like)
var highEarners = (from m in members
                   where m.Salary > 50_000m
                   select m).ToList();
```

Both produce identical results. Method syntax chains naturally and is easier
to extend with custom logic.

### Lambda expressions

A lambda is an inline function. The `=>` reads as "goes to":

```csharp
m => m.Salary > 50_000m
//  ^ parameter   ^ expression (returns bool)
```

For multiple statements, use braces:

```csharp
m => {
    var rate = GetAccrualRate(m.SchemeType);
    return m.Salary * m.ServiceYears * rate;
}
```

---

## LINQ Methods Reference

Here is every LINQ method you will use in the exercises, with a pension-themed
example for each.

### Where -- filtering

Returns elements that match a condition:

```csharp
var active = members.Where(m => !m.IsDeferred).ToList();
var db60 = members.Where(m => m.SchemeType == "DB60ths").ToList();
```

### Select -- projection

Transforms each element into a new shape:

```csharp
var names = members.Select(m => m.Name).ToList();
// ["Alice Smith", "Bob Jones", ...]

var summaries = members.Select(m => new MemberSummary(
    m.Name,
    m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType),
    65 - CalculateAge(m.DateOfBirth)
)).ToList();
```

### OrderBy / OrderByDescending -- sorting

```csharp
var bySalary = members.OrderBy(m => m.Salary).ToList();
var byPensionDesc = members.OrderByDescending(m =>
    m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType)
).ToList();
```

### Sum -- aggregation

```csharp
var totalSalary = members.Sum(m => m.Salary);
var totalLiability = members.Sum(m =>
    m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType)
);
```

### Average -- mean value

```csharp
var avgSalary = members.Average(m => m.Salary);
// 47,291.67 (roughly)
```

### GroupBy -- categorisation

Groups elements by a key and lets you aggregate each group:

```csharp
var byScheme = members.GroupBy(m => m.SchemeType);

foreach (var group in byScheme)
{
    Console.WriteLine($"{group.Key}: {group.Count()} members");
    Console.WriteLine($"  Average salary: {group.Average(m => m.Salary):C}");
}
```

Converting groups to a dictionary:

```csharp
var avgByScheme = members
    .GroupBy(m => m.SchemeType)
    .ToDictionary(
        g => g.Key,
        g => g.Average(m => m.Salary)
    );
```

### First / FirstOrDefault -- single element

```csharp
var alice = members.First(m => m.Name == "Alice Smith");
var unknown = members.FirstOrDefault(m => m.Name == "Nobody");
// unknown is null (no exception thrown)
```

### Any / All -- boolean checks

```csharp
bool hasDeferred = members.Any(m => m.IsDeferred);        // true
bool allActive = members.All(m => !m.IsDeferred);          // false
bool anyHighEarner = members.Any(m => m.Salary > 60_000m); // true
```

### Count -- counting

```csharp
int total = members.Count;                                  // property (fast)
int careCount = members.Count(m => m.SchemeType == "CARE"); // method with predicate
```

### Take / Skip -- paging

```csharp
var topFive = members.OrderByDescending(m => m.Salary).Take(5).ToList();
var page2 = members.Skip(10).Take(10).ToList();
```

---

## Pension Context: Accrual Rates

In UK defined benefit schemes, the **accrual rate** determines how much
pension a member earns per year of service:

| Scheme | Accrual Rate | Meaning |
|--------|-------------|---------|
| DB 1/60ths | 1/60 per year | 60ths of final salary per year |
| DB 1/80ths | 1/80 per year | 80ths of final salary per year |
| CARE | 1/49 per year | 49ths of revalued salary per year |

The annual pension formula (simplified) is:

```
Annual Pension = Salary x Service Years x Accrual Rate
```

For example, Alice Smith with 28 years in a 1/60ths scheme earning 45,000:

```
45,000 x 28 x (1/60) = £21,000 per year
```

---

## Exercises

Open `MemberQueries.cs` and implement the seven TODO methods. Each method has
a clear signature, XML documentation, and hints in the comments.

| # | Method | LINQ methods to use |
|---|--------|-------------------|
| 1 | `GetMembersByScheme` | `Where` |
| 2 | `GetActiveMembers` | `Where` |
| 3 | `CalculateTotalLiability` | `Sum` |
| 4 | `GetAverageSalaryByScheme` | `GroupBy`, `ToDictionary`, `Average` |
| 5 | `GetMembersApproachingRetirement` | `Where` (with age calculation) |
| 6 | `GetMembersOrderedByPension` | `OrderByDescending` |
| 7 | `GetMemberSummaries` | `Select` |

### Tips

- Use the provided `GetAccrualRate()` helper for exercises 3, 6, and 7.
- For age calculation (exercise 5), remember to adjust if the birthday has
  not yet occurred this year:
  ```csharp
  int age = today.Year - dob.Year;
  if (dob.Date > today.AddYears(-age)) age--;
  ```
- Exercise 7 returns `MemberSummary` records. Years to retirement is
  `65 - age` (can be negative for members past NPA).

---

## Running the Code

Run the demo program to see your progress:

```bash
dotnet run --project src/Module2.MemberData
```

Unimplemented exercises will show their TODO message. Completed exercises
will display results.

Run the tests to verify your solutions:

```bash
dotnet test --filter Module2
```

All 15 tests should pass when you have completed every exercise.

---

## Bonus Challenges

If you finish early, try these without looking at hints:

1. **Highest earner per scheme**: Return a `Dictionary<string, Member>` where
   each value is the member with the highest salary in that scheme.

2. **Pension band distribution**: Count how many members fall into pension
   bands: under 10k, 10-20k, 20-30k, 30k+.

3. **Service quartiles**: Split members into four equal groups by service
   years and calculate the average pension for each quartile.

4. **Chain it all**: In a single LINQ chain, find active DB60ths members,
   calculate their pension, order descending, take the top 3, and project
   to `MemberSummary`.
