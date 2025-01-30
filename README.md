# OnTheFlyStats

[![NuGet version (OnTheFlyStats)](https://img.shields.io/nuget/v/OnTheFlyStats.svg)](https://www.nuget.org/packages/OnTheFlyStats/)
[![Licence (OnTheFlyStats)](https://img.shields.io/github/license/mashape/apistatus.svg)](https://choosealicense.com/licenses/mit/)
[![.NET Build and Test](https://github.com/PFalkowski/OnTheFlyStats/actions/workflows/dotnet.yml/badge.svg)](https://github.com/PFalkowski/OnTheFlyStats/actions/workflows/dotnet.yml)

One pass (O(N)) descriptive statistics including:

- variance
- means (arithmetic, geometric and RootMeanSquare)
- sum, min &amp; max
- sample and population variance
- standard deviation of sample and population
- Range and mid range
- Count (N)

Instead of writing:             
```c#
var listMin = matrix[x].Min(); // TODO: calculate in one go
var listMax = matrix[x].Max(); // TODO: calculate in one go
```
Use: 
```c#
var stats = matrix[x].Stats();
var listMin = stats.Min;
var listMax = stats.Max;
```

Instead of recalculating mean each time the loop updates:
```c#
foreach (var item in populationList)
{
    averageLabel.Value = populationList.Average(); // O(N * N) overhead, never write code like this
    minLabel.Value = populationList.Min(); // O(N * N) overhead, never write code like this
    maxLabel.Value = populationList.Max(); // O(N * N) overhead, never write code like this
}
```
Use: 
```c#
var populationStats = new Stats();
foreach (var item in populationList)
{
    populationStats.Update(item);
    averageLabel.Value = populationStats.Average;
    minLabel.Value = populationStats.Min;
    maxLabel.Value = populationStats.Max;
}
```
