# Module 1: Pension Basics

## Learning Objectives

By the end of this module you will be able to:

- Declare variables and use basic C# types (`int`, `decimal`, `string`)
- Understand why `decimal` is used instead of `double` for financial calculations
- Write methods with parameters and return values
- Use enums to represent a fixed set of options
- Apply basic arithmetic to calculate UK defined benefit pensions
- Run unit tests to verify your code

---

## Background: How UK DB Pensions Work

A **Defined Benefit (DB)** pension scheme promises a member a pension based on a formula,
rather than on how much money is in a pot. The most common type in the UK is a
**final salary** scheme, where the pension depends on three things:

1. **Final salary** -- the member's salary at (or near) retirement
2. **Years of service** -- how long they were in the scheme
3. **Accrual rate** -- how much pension they earn per year of service

The basic formula is:

```
Annual Pension = Final Salary x Years of Service x Accrual Rate
```

### Accrual Rates

The accrual rate is usually expressed as a fraction:

- **1/60th** (sixtieths) -- the member earns 1/60th of their salary for each year of service
- **1/80th** (eightieths) -- the member earns 1/80th of their salary for each year of service

A 1/60th scheme is more generous. For example, with 20 years of service:

- 1/60th: 20/60 = 1/3 of salary
- 1/80th: 20/80 = 1/4 of salary

### Worked Example

Jane has a salary of £40,000, has been in her scheme for 20 years, and accrues on a
1/60th basis.

```
Annual Pension = £40,000 x 20 x (1/60) = £13,333.33
```

---

## C# Basics You Will Need

### Variables and Types

C# is a statically typed language. You must declare the type of every variable:

```csharp
int serviceYears = 20;
decimal salary = 40_000m;
string memberName = "Jane Smith";
bool isRetired = false;
```

The underscore in `40_000` is a digit separator -- it makes large numbers easier to read.
The compiler ignores it completely.

### Why `decimal` and Not `double`?

This is critical for pension calculations. The `double` type uses binary floating point,
which cannot represent values like 0.1 exactly:

```csharp
// WRONG for money:
double badMoney = 0.1 + 0.2;  // Result: 0.30000000000000004

// CORRECT for money:
decimal goodMoney = 0.1m + 0.2m;  // Result: 0.3
```

The `decimal` type uses base-10 arithmetic, so it handles pounds and pence precisely.
Always use `decimal` for monetary values. The `m` suffix tells the compiler this is a
decimal literal.

```csharp
decimal salary = 40_000m;
decimal rate = 1m / 60m;     // Decimal division: precise
decimal pension = salary * rate;
```

### String Interpolation

C# lets you embed expressions inside strings using the `$` prefix:

```csharp
decimal pension = 13_333.33m;
Console.WriteLine($"Annual pension: £{pension:N2}");
// Output: Annual pension: £13,333.33
```

The `:N2` is a format specifier meaning "number with 2 decimal places and thousands
separators".

---

## Enums

An **enum** (enumeration) is a type that represents a fixed set of named values. They are
perfect for things like accrual bases, which can only be one of a few options:

```csharp
public enum AccrualBasis
{
    Sixtieths,
    Eightieths
}
```

You use an enum like this:

```csharp
AccrualBasis basis = AccrualBasis.Sixtieths;

if (basis == AccrualBasis.Sixtieths)
{
    Console.WriteLine("Accruing on a 1/60th basis");
}
```

Or with a `switch` expression (modern C#):

```csharp
decimal rate = basis switch
{
    AccrualBasis.Sixtieths => 1m / 60m,
    AccrualBasis.Eightieths => 1m / 80m,
    _ => throw new ArgumentException("Unknown basis")
};
```

The `_` is a discard pattern that catches anything not already matched. It is good
practice to include it so the compiler does not warn about unhandled cases.

---

## Methods

A **method** is a block of code with a name, parameters, and a return type:

```csharp
public decimal CalculateAnnualPension(decimal salary, int serviceYears, decimal accrualRate)
{
    return salary * serviceYears * accrualRate;
}
```

Breaking this down:

- `public` -- accessible from outside the class
- `decimal` -- the return type (what the method gives back)
- `CalculateAnnualPension` -- the method name
- The parameters in parentheses -- the inputs the method needs
- `return` -- sends a value back to the caller

You call a method like this:

```csharp
var calculator = new PensionCalculator();
decimal result = calculator.CalculateAnnualPension(40_000m, 20, 1m / 60m);
```

---

## The Exercises

Open `PensionCalculator.cs`. You will find four methods, each throwing
`NotImplementedException`. Your job is to replace the `throw` statement with working
code.

### Exercise 1: CalculateAnnualPension

**Goal:** Multiply salary, service years, and accrual rate together.

```csharp
public decimal CalculateAnnualPension(decimal salary, int serviceYears, decimal accrualRate)
{
    // Replace the throw with:
    return salary * serviceYears * accrualRate;
}
```

This is deliberately simple. The point is to get comfortable with method signatures,
`decimal` arithmetic, and the `return` keyword.

### Exercise 2: GetAccrualRate

**Goal:** Return the correct decimal value for each `AccrualBasis` enum value.

You need to check whether `basis` is `Sixtieths` or `Eightieths` and return the
corresponding fraction. You can use `if/else`:

```csharp
if (basis == AccrualBasis.Sixtieths)
    return 1m / 60m;
else
    return 1m / 80m;
```

Or a `switch` statement, or the `switch` expression shown in the enums section above.

### Exercise 3: CalculatePartTimePension

**Goal:** Calculate a pension adjusted for part-time service.

A member who works 3 days out of 5 has a part-time proportion of 0.6. Their pension is
the standard calculation multiplied by that proportion.

This builds on Exercise 1 -- you are adding one more multiplication.

### Exercise 4: ApplyEarlyRetirementReduction

**Goal:** Reduce a pension if the member retires before their Normal Pension Age (NPA).

This is the most complex exercise. You need to:

1. Check if the member is retiring early (before NPA). If not, return the full pension.
2. Calculate how many years early they are retiring.
3. Calculate the total reduction percentage.
4. Apply the reduction.

Example walkthrough:

```
Full pension:       £13,333.33
Retirement age:     62
Normal Pension Age: 65
Reduction per year: 4% (0.04)

Years early:        65 - 62 = 3
Total reduction:    3 x 0.04 = 0.12 (12%)
Reduced pension:    £13,333.33 x (1 - 0.12) = £13,333.33 x 0.88 = £11,733.33
```

---

## Running the Code

### Run the demo program

```bash
dotnet run --project src/Module1.PensionBasics
```

Before you implement anything, this will print "Not yet implemented" for each exercise.
As you complete each exercise, you will see the calculated results instead.

### Run the tests

```bash
dotnet test --filter Module1
```

Each exercise has multiple test cases. The tests check edge cases you might not think of,
like zero years of service or retiring after NPA. All tests should pass when your
implementations are correct.

### Recommended workflow

1. Read the exercise description and hints in `PensionCalculator.cs`
2. Write your implementation (replace the `throw` with real code)
3. Run the demo program to see your output
4. Run the tests to check edge cases
5. Move on to the next exercise

---

## Tips

- **Start simple.** Exercise 1 is one line of code. Do not overthink it.
- **Use the `m` suffix** on all decimal literals: `1m`, `60m`, `0.04m`.
- **Read the test cases.** They tell you exactly what inputs and outputs are expected.
  Open `PensionCalculatorTests.cs` to see them.
- **Pay attention to edge cases** in Exercise 4. What happens if someone retires at
  exactly NPA? What if they retire after NPA?

---

## Key Concepts Summary

| Concept | Example | Why It Matters |
|---|---|---|
| `decimal` type | `decimal salary = 40_000m;` | Precise arithmetic for money |
| Enum | `AccrualBasis.Sixtieths` | Fixed set of named options |
| Method | `public decimal Calculate(...)` | Reusable, testable logic |
| String interpolation | `$"Pension: £{amount:N2}"` | Clean output formatting |
| Digit separator | `40_000` | Readability for large numbers |
| Switch expression | `basis switch { ... }` | Clean enum-to-value mapping |

---

## Next Module

Module 2 will introduce classes, properties, and constructors by building a `Member`
class to hold pension scheme member data. You will also learn about nullable types and
input validation.
