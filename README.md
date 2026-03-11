# C# Pension Calculations Training Course

A structured 5-day C# training course (~2 hours per day) focused on UK Defined Benefit pension calculations. Progresses from absolute beginner to intermediate, with each module building on the previous one.

## Prerequisites

- **.NET 8 SDK** (or later) installed — [download](https://dotnet.microsoft.com/download)
- **VS Code** with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension
- No prior C# experience required (some programming knowledge helpful)

## Quick Start

```bash
# Clone/open the project
cd csharp-tutor

# Verify the solution builds
dotnet build

# Run a module's demo
dotnet run --project src/Module1.PensionBasics

# Run a module's tests
dotnet test --filter Module1
```

## Course Structure

Each module is a separate C# console project with a companion test project. The workflow for each module:

1. **Read** the module's `README.md` for teaching content and background
2. **Implement** the `// TODO:` exercises in the source files
3. **Run** the demo program to see your output: `dotnet run --project src/ModuleN.xxx`
4. **Test** your solutions: `dotnet test --filter ModuleN`

### Day 1: C# Fundamentals & Pension Basics
**Project:** `Module1.PensionBasics` | **Tests:** `Module1.Tests`

- Variables, types, `decimal` for money, string interpolation
- Methods, parameters, return values
- Enums (`AccrualBasis`)
- **Exercises:** Calculate annual pension, accrual rates, part-time service, early retirement reduction

```bash
dotnet run --project src/Module1.PensionBasics
dotnet test --filter Module1
```

### Day 2: Collections, Lists & LINQ
**Project:** `Module2.MemberData` | **Tests:** `Module2.Tests`

- `List<T>`, classes with auto-properties, records
- LINQ: `Where`, `Select`, `OrderBy`, `Sum`, `Average`, `GroupBy`, `First`, `Any`, `Count`
- Lambda expressions
- **Exercises:** Filter/sort/group/aggregate a dataset of 12 pension scheme members

```bash
dotnet run --project src/Module2.MemberData
dotnet test --filter Module2
```

### Day 3: Interfaces, DI & Pension Adjustments
**Project:** `Module3.Adjustments` | **Tests:** `Module3.Tests`

- Interfaces and dependency injection (constructor injection)
- `Dictionary<TKey, TValue>` for factor table lookups
- Switch expressions
- **Exercises:** Factor tables, pension adjuster with DI, commutation calculator, GMP splits and revaluation

```bash
dotnet run --project src/Module3.Adjustments
dotnet test --filter Module3
```

### Day 4: Test-Driven Development
**Project:** `Module4.TDD` | **Tests:** `Module4.Tests`

- Red-Green-Refactor cycle
- xUnit: `[Fact]`, `[Theory]`, `[InlineData]`
- Arrange-Act-Assert pattern, test naming conventions
- **Exercises:**
  1. Make pre-written revaluation tests pass (implement code)
  2. Write your OWN tests for Annual Allowance, then implement (true TDD)
  3. Refactor messy but working code while keeping tests green

```bash
dotnet run --project src/Module4.TDD
dotnet test --filter Module4
```

### Day 5: Builder Pattern, Records & Advanced LINQ
**Project:** `Module5.Advanced` | **Tests:** `Module5.Tests`

- Builder pattern with fluent API (method chaining)
- Records, immutability, `with` expressions
- Advanced LINQ: `SelectMany`, `Aggregate`, complex `GroupBy`, running totals
- **Exercises:** Build a full pension calculation pipeline, run it across members, produce scheme-level analytics

```bash
dotnet run --project src/Module5.Advanced
dotnet test --filter Module5
```

## Project Structure

```
csharp-tutor/
├── README.md                              ← You are here
├── CSharpPensionCourse.sln
├── src/
│   ├── Module1.PensionBasics/             ← Day 1 source + README
│   ├── Module2.MemberData/                ← Day 2 source + README
│   ├── Module3.Adjustments/               ← Day 3 source + README
│   ├── Module4.TDD/                       ← Day 4 source + README
│   └── Module5.Advanced/                  ← Day 5 source + README
└── tests/
    ├── Module1.Tests/
    ├── Module2.Tests/
    ├── Module3.Tests/
    ├── Module4.Tests/
    └── Module5.Tests/
```

## How It Works

- Source files contain `// TODO:` markers where you write your implementations
- Methods throw `NotImplementedException` until you replace them with real code
- Pre-written unit tests validate your solutions — run them to check your work
- Each module's `Program.cs` has a demo runner that shows output as you complete exercises
- `dotnet build` compiles everything (with warnings for TODOs, but no errors)
- `dotnet test` runs all tests — they start red and turn green as you implement

## Debugging: Stepping Through Code Line by Line

One of the most powerful ways to understand what your code is doing is to run it in **debug mode** — this lets you pause execution at any line and watch exactly what's happening.

### Setting a Breakpoint

A **breakpoint** is a marker that tells the debugger "pause here". To set one in VS Code:

1. Open any `.cs` file (e.g. `src/Module1.PensionBasics/PensionCalculator.cs`)
2. Click in the **gutter** — the narrow column to the left of the line numbers
3. A red dot appears — that's your breakpoint

### Starting the Debugger

1. Open the file that contains your `Main` method or the code you want to trace (`Program.cs` is a good starting point)
2. Press **F5** (or go to **Run → Start Debugging**)
3. VS Code will build the project and run it — execution will pause at your breakpoint

> If VS Code asks which project to debug, select the module you're working on (e.g. `Module1.PensionBasics`).

### Stepping Through Code

Once paused at a breakpoint, use these keys to move through the code:

| Key | Action | When to use |
|-----|--------|-------------|
| **F10** | Step Over | Run the current line and move to the next one |
| **F11** | Step Into | If the line calls a method, jump inside that method |
| **Shift+F11** | Step Out | Finish the current method and return to where it was called |
| **F5** | Continue | Run freely until the next breakpoint (or program end) |

### Watching Variables

While paused, VS Code shows you the current value of every variable:

- **Variables panel** (bottom-left): lists all variables in scope and their current values
- **Hover over a variable** in the editor: a tooltip shows its value
- **Watch panel**: click **+** to add any expression (e.g. `pension * 12`) and watch it update as you step

### Example Workflow

1. Open `src/Module1.PensionBasics/PensionCalculator.cs`
2. Set a breakpoint on the first line inside `CalculateAnnualPension`
3. Press **F5** to start
4. Use **F10** to step over each line — watch the `Variables` panel to see `salary`, `accrualRate`, and the return value change in real time
5. Press **F5** to finish running when you've seen enough

This is the single best way to debug unexpected results — if a calculation is returning the wrong number, step through it line by line and you'll see exactly where it goes wrong.

## Tips

- Work through the modules in order — later modules build on earlier concepts
- Read the module README before diving into code
- Look at the test file to understand what's expected before implementing
- Use `dotnet test --filter Module1 -v normal` for verbose test output
- The `Program.cs` demo runner wraps each exercise in try/catch, so unfinished exercises show their TODO message rather than crashing
