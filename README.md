# OnTheFlyStats

[![CI](https://github.com/PFalkowski/OnTheFlyStats/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/OnTheFlyStats/actions/workflows/ci.yml)
[![NuGet version](https://img.shields.io/nuget/v/OnTheFlyStats.svg)](https://www.nuget.org/packages/OnTheFlyStats/)
[![NuGet downloads](https://img.shields.io/nuget/dt/OnTheFlyStats.svg)](https://www.nuget.org/packages/OnTheFlyStats/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_OnTheFlyStats&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_OnTheFlyStats)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_OnTheFlyStats&metric=coverage)](https://sonarcloud.io/summary/new_code?id=PFalkowski_OnTheFlyStats)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

One-pass (O(N)) descriptive statistics. Statistics are always ready and O(1) to retrieve — no recalculation needed.

## Install

```bash
dotnet add package OnTheFlyStats
```

## Statistics

- Mean (arithmetic, geometric, root-mean-square)
- Variance (population and sample)
- Standard deviation (population and sample)
- Standard error of the mean
- Z-score / standard score
- Sum, min, max
- Range and mid-range
- Count (N)

## Usage

Instead of recalculating mean each loop iteration:

```csharp
foreach (var item in populationList)
{
    averageLabel.Value = populationList.Average(); // O(N²) — never do this
    minLabel.Value = populationList.Min();
    maxLabel.Value = populationList.Max();
}
```

Use:

```csharp
var stats = new Stats();
foreach (var item in populationList)
{
    stats.Update(item);
    averageLabel.Value = stats.Mean;
    minLabel.Value = stats.Min;
    maxLabel.Value = stats.Max;
}
```

Or compute from a collection in one line:

```csharp
var stats = matrix[x].Stats();
var listMin = stats.Min;
var listMax = stats.Max;
```

## Confusion Matrix

```csharp
var roc = new ReceiverOperatorCharacteristic(expected, predicted);
Console.WriteLine($"Accuracy: {roc.Accuracy:P1}");
Console.WriteLine($"Sensitivity: {roc.Sensitivity:P1}");
Console.WriteLine($"Specificity: {roc.Specificity:P1}");
```
