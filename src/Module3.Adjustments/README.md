# Module 3: Pension Adjustments

## What You'll Learn

This module teaches four important C# concepts through the lens of pension adjustment calculations:

1. **Interfaces** -- defining contracts that classes must follow
2. **Dependency Injection** -- passing dependencies in via constructors
3. **Dictionary<TKey, TValue>** -- key-value lookups for factor tables
4. **Switch Expressions** -- modern C# pattern matching syntax

By the end of this module, you'll be able to build a system that adjusts pension
amounts using actuarial factor tables, calculate commutation (lump sum trade-offs),
and handle GMP (Guaranteed Minimum Pension) splits and revaluation.

---

## Pension Context

### Factor Tables

In real pension administration, actuarial factor tables are central to most
calculations. A factor table maps an age (and sometimes gender, scheme, or date)
to a decimal multiplier. These factors are set by the scheme actuary and reviewed
periodically.

Common factor types:
- **Early retirement factors** (< 1.0): reduce the pension when retiring before
  Normal Pension Age (NPA), typically 65
- **Late retirement factors** (> 1.0): increase the pension when retiring after NPA
- **Commutation factors**: convert annual pension into a one-off lump sum

### Commutation

Members of UK DB schemes can typically exchange ("commute") part of their annual
pension for a tax-free lump sum. The calculation is:

```
Lump Sum = Commuted Pension x Commutation Factor
```

For example, commuting 25% of a 20,000 pension at age 65 (factor 15.0):
- Commuted portion: 20,000 x 0.25 = 5,000
- Lump sum: 5,000 x 15.0 = 75,000
- Residual pension: 20,000 - 5,000 = 15,000

### GMP (Guaranteed Minimum Pension)

GMP is a minimum pension that certain UK occupational schemes must provide for
service between 1978 and 1997. It has two tranches:
- **Pre-88 GMP**: accrued 6 April 1978 to 5 April 1988
- **Post-88 GMP**: accrued 6 April 1988 to 5 April 1997

The pension above the GMP is called the **Excess**. GMP amounts are revalued
(increased) each year between leaving and retirement using one of several methods.

---

## C# Concepts

### Interfaces

An interface defines a contract -- a set of methods and properties that a class
must implement. Interfaces use the `interface` keyword and by convention start
with `I`.

```csharp
public interface IFactorTable
{
    decimal GetFactor(int age, string factorType);
    bool HasFactor(int age, string factorType);
}
```

Any class that implements `IFactorTable` must provide both methods:

```csharp
public class EarlyRetirementFactorTable : IFactorTable
{
    public decimal GetFactor(int age, string factorType)
    {
        // Must implement this
    }

    public bool HasFactor(int age, string factorType)
    {
        // Must implement this too
    }
}
```

Why interfaces matter:
- They let you write code against an abstraction, not a specific class
- You can swap implementations without changing the calling code
- They make testing easier (you can create mock/fake implementations)

### Dependency Injection (Constructor Injection)

Instead of creating dependencies inside a class, you pass them in through the
constructor. This is the simplest form of dependency injection:

```csharp
public class PensionAdjuster
{
    private readonly IFactorTable _factorTable;

    // The factor table is "injected" via the constructor
    public PensionAdjuster(IFactorTable factorTable)
    {
        _factorTable = factorTable;
    }

    public decimal AdjustPension(decimal pension, int age)
    {
        var factor = _factorTable.GetFactor(age, "Retirement");
        return pension * factor;
    }
}
```

Now PensionAdjuster works with ANY factor table:

```csharp
// Early retirement scenario
var adjuster1 = new PensionAdjuster(new EarlyRetirementFactorTable());

// Late retirement scenario -- same class, different table
var adjuster2 = new PensionAdjuster(new LateRetirementFactorTable());
```

The PensionAdjuster doesn't know or care which table it's using. It only knows
the table implements `IFactorTable`. This is "coding to an interface."

### Dictionary<TKey, TValue>

A Dictionary is a collection of key-value pairs with fast lookup by key.
Perfect for factor tables where you look up a factor by age.

```csharp
// Creating a dictionary with initializer syntax
private readonly Dictionary<int, decimal> _factors = new()
{
    { 55, 0.55m },
    { 60, 0.76m },
    { 65, 1.00m }
};

// Looking up a value (throws KeyNotFoundException if not found)
decimal factor = _factors[60]; // Returns 0.76m

// Safe lookup with TryGetValue
if (_factors.TryGetValue(age, out var factor))
{
    // factor is available here
}

// Checking if a key exists
bool exists = _factors.ContainsKey(60); // true
```

`TryGetValue` is preferred over checking `ContainsKey` then indexing, because
it does a single lookup instead of two.

### Switch Expressions

Switch expressions are a concise way to return a value based on pattern matching.
They were introduced in C# 8.0.

```csharp
// Traditional switch statement
switch (revalType)
{
    case RevaluationType.Fixed:
        return 0.035m;
    case RevaluationType.Section52:
        return 0.04m;
    default:
        throw new ArgumentException("Unknown type");
}

// Switch expression (modern C#) -- much cleaner
return revalType switch
{
    RevaluationType.Fixed => 0.035m,
    RevaluationType.Section52 => 0.04m,
    RevaluationType.CpiCapped => Math.Min(cpiRate, 0.025m),
    _ => throw new ArgumentException($"Unknown revaluation type: {revalType}")
};
```

The `_` is the discard pattern -- it matches anything not already matched
(equivalent to `default`).

---

## Exercises

### Exercise 1: EarlyRetirementFactorTable

**File:** `EarlyRetirementFactorTable.cs`

Implement `IFactorTable` using a `Dictionary<int, decimal>`:
1. Create the dictionary with factors for ages 55-65
2. Implement `GetFactor` to look up by age (throw `KeyNotFoundException` if missing)
3. Implement `HasFactor` to check if the age exists

**Factors to use:**
| Age | 55   | 56   | 57   | 58   | 59   | 60   | 61   | 62   | 63   | 64   | 65   |
|-----|------|------|------|------|------|------|------|------|------|------|------|
|     | 0.55 | 0.58 | 0.61 | 0.65 | 0.70 | 0.76 | 0.82 | 0.88 | 0.92 | 0.96 | 1.00 |

### Exercise 2: LateRetirementFactorTable

**File:** `LateRetirementFactorTable.cs`

Same pattern as Exercise 1, but with late retirement factors for ages 65-70.

**Factors to use:**
| Age | 65   | 66   | 67   | 68   | 69   | 70   |
|-----|------|------|------|------|------|------|
|     | 1.00 | 1.05 | 1.11 | 1.17 | 1.24 | 1.31 |

### Exercise 3: PensionAdjuster

**File:** `PensionAdjuster.cs`

The constructor is already provided. Implement:
1. `AdjustPension` -- get the factor and multiply
2. `AdjustPensionSafe` -- check with `HasFactor` first, return pension unchanged
   if no factor exists

### Exercise 4: CommutationCalculator

**File:** `CommutationCalculator.cs`

Implement the `Commute` method:
1. Calculate the commuted pension amount
2. Look up the commutation factor from the injected table
3. Calculate the lump sum
4. Return a `CommutationResult` with all three values

### Exercise 5: GMP Split

**File:** `GmpCalculator.cs` -- `SplitPension` method

Split a total pension into Pre-88 GMP, Post-88 GMP, and Excess:
- Excess = total - pre88 - post88
- If total < GMP amounts, excess should be 0 (use `Math.Max`)

### Exercise 6: GMP Revaluation

**File:** `GmpCalculator.cs` -- `GetRevaluationRate` and `RevalueGmp` methods

1. Use a **switch expression** to return the rate for each `RevaluationType`
2. Implement compound revaluation using a `for` loop:
   ```csharp
   for (int i = 0; i < years; i++)
   {
       amount *= (1 + rate);
   }
   ```

---

## Running the Code

Run the demo program:

```bash
dotnet run --project src/Module3.Adjustments
```

Before completing the exercises, you'll see `NotImplementedException` messages.
As you implement each exercise, the demo will show calculated results.

Run the tests to check your work:

```bash
dotnet test --filter Module3
```

The tests verify exact values, so make sure your factor tables match the
values listed in the exercise tables above.

---

## Key Takeaways

- **Interfaces** define what a class can do, not how it does it
- **Dependency injection** makes classes flexible and testable by passing
  dependencies in rather than creating them internally
- **Dictionary** provides O(1) lookups -- ideal for factor tables
- **Switch expressions** are concise and expressive for mapping values
- Real pension systems have hundreds of factor tables, all following the same
  pattern you've built here -- the interface + DI approach scales well

## Next Steps

In Module 4, you'll build on these concepts to handle pension increases
(annual revaluation of pensions in payment) using inheritance and more
advanced patterns.
