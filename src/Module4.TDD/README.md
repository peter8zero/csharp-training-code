# Module 4: Test-Driven Development

## Overview

This module teaches Test-Driven Development (TDD) through pension-domain calculations:
revaluation of deferred pensions and Annual Allowance tax charges. You will write
tests first, implement code to make them pass, and refactor messy code with the
safety net of an existing test suite.

---

## What is TDD?

Test-Driven Development is a software development practice where you write tests
**before** writing the production code. It follows a strict three-step cycle:

### The Red-Green-Refactor Cycle

```
1. RED    — Write a test that fails (because the code doesn't exist yet)
2. GREEN  — Write the minimum code needed to make the test pass
3. REFACTOR — Clean up the code while keeping the tests green
```

Repeat this cycle for each small piece of functionality. The discipline of TDD
forces you to think about **what** the code should do before thinking about
**how** to do it.

### Why TDD Matters for Pension Calculations

Pension calculations are a domain where correctness is non-negotiable:

- **Regulatory compliance**: Pension benefits must be calculated according to
  scheme rules and legislation. An error could affect thousands of members.
- **Financial accuracy**: Rounding errors, off-by-one mistakes in year counts,
  or wrong rate applications can compound over decades of pension accrual.
- **Auditability**: Tests serve as executable documentation of the calculation
  rules. When an auditor asks "how do you calculate CPI-capped revaluation?",
  you can point to the test suite.
- **Change confidence**: Pension legislation changes regularly. When you update
  a threshold or add a new calculation method, tests tell you immediately if
  you have broken existing behaviour.

---

## xUnit Fundamentals

This course uses **xUnit**, the most popular testing framework for .NET.

### [Fact] — A Single Test Case

A `[Fact]` attribute marks a method as a test with no parameters:

```csharp
[Fact]
public void Add_TwoPositiveNumbers_ReturnsSum()
{
    var calculator = new Calculator();
    var result = calculator.Add(2, 3);
    Assert.Equal(5, result);
}
```

### [Theory] with [InlineData] — Parameterised Tests

A `[Theory]` runs the same test logic with different inputs. Each `[InlineData]`
provides one set of arguments:

```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(0, 0, 0)]
[InlineData(-1, 1, 0)]
public void Add_VariousInputs_ReturnsExpectedSum(int a, int b, int expected)
{
    var calculator = new Calculator();
    var result = calculator.Add(a, b);
    Assert.Equal(expected, result);
}
```

Use `[Theory]` when you want to test the same behaviour with multiple examples.
This is particularly useful for pension calculations where you often test the
same formula with different salary, service, or rate inputs.

### Common Assertions

```csharp
Assert.Equal(expected, actual);          // Value equality
Assert.Equal(expected, actual, 2);       // Decimal places precision
Assert.True(condition);                  // Boolean check
Assert.False(condition);                 // Boolean check
Assert.Throws<ExceptionType>(() => ...) // Exception expected
Assert.InRange(actual, low, high);       // Range check
```

---

## The Arrange-Act-Assert Pattern

Every test should follow the AAA pattern. This keeps tests readable and
consistent:

```csharp
[Fact]
public void FixedRate_OneYear_AppliesRateOnce()
{
    // Arrange — set up the objects and inputs
    var calculator = new RevaluationCalculator();
    decimal amount = 10_000m;
    decimal rate = 0.035m;
    int years = 1;

    // Act — call the method under test
    var result = calculator.FixedRateRevaluation(amount, rate, years);

    // Assert — verify the result
    Assert.Equal(10_350m, result);
}
```

For simple tests, you can collapse this into fewer lines, but the logical
structure should always be: set up, execute, verify.

---

## Test Naming Conventions

This course follows the convention:

```
MethodName_Scenario_ExpectedBehaviour
```

Examples:
- `FixedRate_ZeroYears_ReturnsOriginalAmount`
- `GetAnnualAllowance_WithMpaa_Returns10000`
- `CalcPension_EarlyRetirement_ReducesPension`

The name should read as a sentence that describes the specification. When a test
fails, the name tells you exactly what broke without reading the test code.

Other naming styles exist (`Should_ReturnX_When_Y`, `Given_When_Then`), but
consistency within a project matters more than which style you pick.

---

## Exercise 1: Make the Revaluation Tests Pass

**Approach**: Tests are already written. You implement the code.

### Getting Started

1. Open `tests/Module4.Tests/RevaluationCalculatorTests.cs` and read through
   the tests. Understand what each one expects.

2. Run the tests to confirm they all fail (RED):
   ```bash
   dotnet test --filter RevaluationCalculatorTests
   ```

3. Open `src/Module4.TDD/RevaluationCalculator.cs` and implement each method.

### Walkthrough: FixedRateRevaluation

Look at the tests for this method:
- Zero years returns the original amount
- One year multiplies by (1 + rate)
- Ten years compounds correctly
- Zero rate returns the same amount

The formula is: `amount * (1 + rate) ^ years`

In C# you can implement this with a loop:
```csharp
decimal result = amount;
for (int i = 0; i < years; i++)
{
    result *= (1 + fixedRate);
}
return result;
```

Or using `Math.Pow`:
```csharp
return amount * (decimal)Math.Pow((double)(1 + fixedRate), years);
```

The loop approach keeps everything in `decimal` precision, which matters for
financial calculations. `Math.Pow` uses `double` internally and may introduce
floating-point rounding — but for this exercise either approach works.

### Walkthrough: CpiCappedRevaluation

This method takes a list of annual CPI rates and applies the **lesser** of each
year's CPI and the cap rate. Think of it as a loop where each year's growth
factor is `1 + Math.Min(cpiRate, capRate)`.

4. After implementing each method, run the tests again. Keep going until they
   all pass (GREEN).

5. Review your code — is there any duplication? Can anything be simplified?
   If so, refactor while keeping tests green.

---

## Exercise 2: Write Tests for Annual Allowance, Then Implement

**Approach**: You write BOTH the tests and the code. True TDD.

### Getting Started

1. Open `tests/Module4.Tests/AnnualAllowanceCalculatorTests.cs`. Three example
   tests are provided. Read the commented TODO items for test ideas.

2. Open `src/Module4.TDD/AnnualAllowanceCalculator.cs` and read the XML
   documentation to understand the rules.

### Step-by-Step TDD Cycle

**Cycle 1: Tapered Annual Allowance**

Write this test first:
```csharp
[Fact]
public void GetAnnualAllowance_HighEarner_ReturnsTaperedAmount()
{
    // Adjusted income £300,000, threshold income £250,000
    // Taper: 60,000 - ((300,000 - 260,000) / 2) = 40,000
    var result = _calculator.GetAnnualAllowance(300_000m, 250_000m);
    Assert.Equal(40_000m, result);
}
```

Run it — it fails (RED). Now implement just enough of `GetAnnualAllowance` to
make this test and the existing tests pass (GREEN).

**Cycle 2: Taper Floor**

What happens when adjusted income is so high that the taper would reduce the AA
below £10,000? Write a test for it:
```csharp
[Fact]
public void GetAnnualAllowance_VeryHighEarner_ReturnsMinimum10000()
{
    // Adjusted income £500,000 — taper would give 60k - 120k = negative
    // Should be floored at £10,000
    var result = _calculator.GetAnnualAllowance(500_000m, 250_000m);
    Assert.Equal(10_000m, result);
}
```

**Cycle 3: Threshold Income Gate**

The taper only applies if threshold income exceeds £200,000. What if adjusted
income is £300,000 but threshold income is only £150,000? The taper should NOT
apply. Write a test, then update your implementation.

**Continue** writing tests for the tax charge methods and the input validation
in `CalculateTaxChargeSafe`. Aim for at least 6 additional tests beyond the
three provided.

### Boundary Testing

Pension calculations are full of thresholds. Always test:
- Values exactly on the threshold (£260,000 adjusted income)
- Values just above and just below
- Zero values
- Edge cases (negative inputs, rates above 100%)

---

## Exercise 3: Refactor the Messy Calculator

**Approach**: The code works. The tests pass. Make it better.

### Getting Started

1. Run the messy calculator tests to confirm they all pass:
   ```bash
   dotnet test --filter MessyCalculatorTests
   ```

2. Open `src/Module4.TDD/MessyCalculator.cs` and read through the code.

3. Identify the problems:
   - **Magic numbers**: What are `60`, `80`, `49`, `65`, `0.04`, `0.05`,
     `0.25`, `15.0`, `18.5`, `16.0`, `13.0`?
   - **Poor variable names**: `p`, `e`, `r`, `l`, `b`, `sal`, `yrs`
   - **One massive method**: The method does accrual, early/late adjustment,
     and commutation all in one block
   - **String comparison for type**: Could use an enum or constants

4. Refactor step by step. After EACH change, run the tests:
   ```bash
   dotnet test --filter MessyCalculatorTests
   ```

### Suggested Refactoring Steps

1. **Rename variables** for clarity (`p` -> `pension`, `sal` -> `salary`, etc.)
2. **Extract constants** (`NormalPensionAge = 65`, `EarlyRetirementReduction = 0.04m`)
3. **Extract methods** (`CalculateBasicPension`, `ApplyEarlyRetirementReduction`,
   `ApplyLateRetirementEnhancement`, `ApplyCommutation`)
4. **Consider an enum** for the accrual type instead of magic strings

The key lesson: refactoring is safe when you have tests. Without tests, every
change is a risk. With tests, you can restructure confidently.

### Important Rule

Do NOT change the public method signature of `CalcPension`. The tests call that
method, so it must keep the same name and parameters. You can change everything
inside it and add private helper methods.

---

## Tips for Writing Good Tests

1. **Test one thing per test**. If a test fails, you should immediately know
   what went wrong. A test that checks five things gives you less information.

2. **Use descriptive names**. `Test1` tells you nothing.
   `GetAnnualAllowance_HighEarner_ReturnsTaperedAmount` tells you everything.

3. **Test edge cases**. Zero values, negative numbers, empty collections,
   maximum values, and boundary conditions are where bugs live.

4. **Keep tests independent**. Each test should set up its own data and not
   depend on another test running first. In xUnit, each test gets a fresh
   instance of the test class.

5. **Don't test implementation details**. Test what the method returns, not
   how it calculates internally. If you refactor the internals, the tests
   should still pass.

6. **Use [Theory] for tables of examples**. When you have the same logic
   tested with different numbers, parameterised tests reduce duplication
   and make it easy to add more cases.

7. **Assert precisely**. Use `Assert.Equal` with exact expected values for
   pension calculations. Avoid `Assert.True(result > 0)` when you can
   check the exact figure.

---

## Running Tests

Run all Module 4 tests:
```bash
dotnet test --filter Module4
```

Run a specific test class:
```bash
dotnet test --filter RevaluationCalculatorTests
dotnet test --filter AnnualAllowanceCalculatorTests
dotnet test --filter MessyCalculatorTests
```

Run a specific test:
```bash
dotnet test --filter "FixedRate_ZeroYears_ReturnsOriginalAmount"
```

See detailed output:
```bash
dotnet test --filter Module4 -v normal
```

---

## Summary

| Exercise | What you do | TDD skill practised |
|----------|------------|---------------------|
| Revaluation Calculator | Implement code to pass existing tests | GREEN phase — writing code to satisfy tests |
| Annual Allowance | Write tests first, then implement | Full RED-GREEN-REFACTOR cycle |
| Messy Calculator | Refactor working code, keep tests green | REFACTOR phase — improving code safely |

Together these three exercises cover the complete TDD workflow. The pension
domain gives you realistic calculations where precision, boundary conditions,
and legislative rules make testing essential rather than optional.
