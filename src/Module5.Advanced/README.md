# Module 5: Builder Pattern, Records & Advanced LINQ

## Overview

This module brings together several intermediate-to-advanced C# features through
a realistic pension calculation pipeline. You will build a fluent calculation
builder, model immutable results with records, and analyse batch outputs using
advanced LINQ operations.

By the end of this module you will be able to:

- Implement the **builder pattern** with a fluent API
- Use C# **records** for immutable data with value equality
- Apply advanced **LINQ** operations: `GroupBy`, `SelectMany`, `Aggregate`
- Chain these concepts into a complete pension calculation pipeline

---

## Concept 1: The Builder Pattern

### What is it?

The builder pattern separates the construction of a complex object from its
representation. Instead of a constructor with many parameters, you configure an
object step by step through method calls.

### Why use it?

Pension calculations need many inputs: member details, scheme rules, retirement
age, commutation options. A constructor with eight parameters is hard to read
and easy to get wrong:

```csharp
// Hard to read — which parameter is which?
var result = Calculate(member, scheme, 63, 0.25m, true, false, null, 15.0m);
```

The builder pattern makes intent clear:

```csharp
var result = new PensionCalculationBuilder()
    .ForMember(member)
    .WithScheme(scheme)
    .AtRetirementAge(63)
    .WithCommutation(0.25m)
    .Build();
```

### How does method chaining work?

Each configuration method stores a value and returns `this` — the builder
instance itself. This is what allows you to chain calls:

```csharp
public PensionCalculationBuilder ForMember(MemberRecord member)
{
    _member = member;
    return this;    // <-- enables chaining
}
```

The `Build()` method at the end validates the configuration, runs the
calculation, and returns the finished result.

### When to use it

- When constructing objects with many optional parameters
- When the construction process has multiple steps or validations
- When you want a readable, self-documenting API
- When the same construction process should create different representations

### Builder vs Constructor vs Factory

| Approach    | Best for                                    |
|-------------|---------------------------------------------|
| Constructor | Simple objects with few required parameters  |
| Factory     | Choosing between different types at runtime  |
| Builder     | Complex objects with many optional settings  |

---

## Concept 2: Records and Immutability

### What is a record?

A `record` in C# is a reference type designed for immutable data. Records give
you value-based equality, a readable `ToString()`, and `with` expressions for
creating modified copies — all without writing boilerplate.

```csharp
public record CalculationResult
{
    public string MemberName { get; init; } = string.Empty;
    public decimal BasePension { get; init; }
    public decimal AdjustedPension { get; init; }
}
```

The `init` keyword means properties can only be set during object creation —
they cannot be changed afterwards.

### Why immutability matters

In pension calculations, results should never be silently modified after
creation. If you need a variation, you create a new object:

```csharp
var original = new CalculationResult { MemberName = "Alice", BasePension = 20_000m };

// This creates a NEW object — original is unchanged
var modified = original with { MemberName = "Bob" };

Console.WriteLine(original.MemberName);  // "Alice" — still intact
Console.WriteLine(modified.MemberName);  // "Bob"
```

This makes code easier to reason about, especially in multi-step pipelines
where results flow from one stage to the next.

### Value equality

Classes use reference equality by default — two objects are equal only if they
are the same instance in memory. Records use value equality — two records are
equal if all their properties match:

```csharp
var a = new CalculationResult { MemberName = "Alice", BasePension = 20_000m };
var b = new CalculationResult { MemberName = "Alice", BasePension = 20_000m };

Console.WriteLine(a == b);       // true — same values
Console.WriteLine(a.Equals(b));  // true
```

This is useful for testing, deduplication, and comparing calculation outputs.

### Positional records

You can also declare records with positional parameters for conciseness:

```csharp
public record Point(double X, double Y);

var p = new Point(3.0, 4.0);
Console.WriteLine(p.X);  // 3.0
```

Positional records automatically generate a constructor, deconstructor, and
init-only properties.

---

## Concept 3: Advanced LINQ

### GroupBy with projections

`GroupBy` partitions a sequence into groups. Combined with `ToDictionary`, it
produces lookup structures:

```csharp
var liabilityByScheme = results
    .GroupBy(r => r.SchemeType)
    .ToDictionary(
        g => g.Key,
        g => g.Sum(r => r.AdjustedPension)
    );
```

This groups calculation results by scheme type and sums the adjusted pension
for each group — a common reporting requirement.

### SelectMany for flattening

When members have multiple calculation tranches (e.g., pre-2006 and post-2006
service), data arrives as a list of lists. `SelectMany` flattens nested
collections:

```csharp
// tranches: List<List<CalculationResult>>
var allResults = tranches
    .SelectMany(tranche => tranche)  // flatten to single list
    .GroupBy(r => r.MemberName)
    .ToDictionary(
        g => g.Key,
        g => g.Sum(r => r.AdjustedPension)
    );
```

### Aggregate for running totals

`Aggregate` (also called `Reduce` or `Fold` in other languages) processes a
sequence element by element, accumulating a result. It is useful for running
totals:

```csharp
var runningTotal = 0m;
var totals = results.Select(r =>
{
    runningTotal += r.AdjustedPension;
    return (r.MemberName, RunningTotal: runningTotal);
}).ToList();
```

### Chaining multiple operations

Real analytics often chain several LINQ operations together. Each operation
transforms the data for the next:

```csharp
var highEarnersByScheme = results
    .Where(r => r.AdjustedPension > 20_000m)          // filter
    .GroupBy(r => r.SchemeType)                         // group
    .ToDictionary(
        g => g.Key,
        g => g.Select(r => r.MemberName).ToList()      // project
    );
```

### LINQ method syntax vs query syntax

This module uses method syntax (`.Where()`, `.Select()`, `.GroupBy()`)
throughout. Query syntax (`from ... where ... select`) is an alternative but
method syntax is more common in production C# code and supports all LINQ
operators.

---

## How This Maps to Real Pension Pipelines

In production pension administration systems, a typical calculation pipeline
looks like this:

1. **Load** member data and scheme rules from a database
2. **Build** calculation parameters (retirement age, commutation options)
3. **Calculate** base pension using accrual rates and service
4. **Adjust** for early/late retirement factors
5. **Apply** commutation to split pension into residual + lump sum
6. **Aggregate** results for reporting (scheme totals, summaries, exceptions)

This module mirrors that pipeline:

- **Steps 1-2**: `PensionCalculationBuilder` configures member + scheme
- **Steps 3-5**: `Build()` runs the calculation through each stage
- **Step 6**: `SchemeAnalytics` analyses the batch output

The builder pattern makes each step explicit. Records ensure results are not
accidentally modified between stages. LINQ provides the analytical layer on top.

---

## Exercises

### Exercise 1: Implement the Builder (PensionCalculationBuilder.cs)

Implement the four configuration methods (`ForMember`, `WithScheme`,
`AtRetirementAge`, `WithCommutation`) and the `Build` method. Each configuration
method should store its value and return `this`. The `Build` method should:

1. Validate that member and scheme are set (throw `InvalidOperationException`)
2. Calculate base pension: `salary * serviceYears * accrualRate`
3. Adjust for early retirement (reduce) or late retirement (increase)
4. Apply commutation to split into residual pension and lump sum
5. Return a `CalculationResult` with all fields populated

### Exercise 2: Batch Calculations (Program.cs)

The demo runner already shows how to use the builder with `Reset()` to process
a list of members. Study how LINQ's `Select` applies the builder to each member.

### Exercise 3: Advanced LINQ Analytics (SchemeAnalytics.cs)

Implement six analytics methods:

- **3a** `GetTotalLiabilityByScheme` — GroupBy + Sum
- **3b** `GetAveragePensionByAgeBand` — GroupBy with age band mapping + Average
- **3c** `GetHighPensionMembers` — Where + Select
- **3d** `GetRunningTotals` — Accumulator pattern with Select
- **3e** `FlattenAndSumByMember` — SelectMany + GroupBy + Sum
- **3f** `GetSummary` — Multiple LINQ aggregations into a SchemeSummary record

### Exercise 4: Immutable Records (CalculationResult.cs)

The `CalculationResult` record is already provided with `init` properties. Study
how `with` expressions and value equality work in the tests. Optionally, try
converting it to a positional record.

### Exercise 5 (Bonus): Validation

Add validation to the builder:
- Commutation percentage must be between 0 and 1
- Retirement age must be between 50 and 75
- Service years cannot exceed the difference between retirement age and 16

---

## Running the Module

Build and run the demo:

```bash
dotnet run --project src/Module5.Advanced
```

Run the tests:

```bash
dotnet test --filter Module5
```

The tests will fail until you implement the TODO exercises. Work through them
one at a time — the test names tell you exactly what each test expects.

---

## Key Takeaways

1. **Builder pattern** creates readable, flexible APIs for complex object
   construction. Each method returns `this` to enable chaining.

2. **Records** provide immutability and value equality with minimal boilerplate.
   Use `with` expressions to create modified copies without mutating the
   original.

3. **Advanced LINQ** — `GroupBy`, `SelectMany`, and `Aggregate` — handles the
   analytical and reporting layer that sits on top of raw calculation outputs.

4. These three concepts combine naturally in pension calculation pipelines:
   builders configure the calculation, records carry the immutable results, and
   LINQ analyses the batch output.

---

## Further Reading

- [Builder Pattern (Refactoring Guru)](https://refactoring.guru/design-patterns/builder)
- [Records in C# (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)
- [LINQ GroupBy (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby)
- [LINQ SelectMany (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany)
- [LINQ Aggregate (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregate)
