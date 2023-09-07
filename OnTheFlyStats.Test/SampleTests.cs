using System.Collections.Generic;
using Xunit;

namespace OnTheFlyStats.Test;

public class SampleTests
{
    [Fact]
    public void Constructor_WithValidInput_InitializesProperties()
    {
        // Arrange
        var input = new List<double> { 1.0, 2.0, 3.0 };
        var name = "TestSample";

        // Act
        var sample = new Sample(input, name);

        // Assert
        Assert.Equal(name, sample.Name);
        Assert.Equal(input, sample.InternalSample);
    }

    [Fact]
    public void Constructor_WithNullInput_InitializesProperties()
    {
        // Arrange
        List<double> input = null;
        var name = "TestSample";

        // Act
        var sample = new Sample(input, name);

        // Assert
        Assert.Equal(name, sample.Name);
        Assert.Empty(sample.InternalSample);
    }

    [Fact]
    public void Update_AddsValueToInternalSample()
    {
        // Arrange
        var sample = new Sample();
        var value = 5.0;

        // Act
        sample.Update(value);

        // Assert
        Assert.Single(sample.InternalSample);
        Assert.Equal(value, sample.InternalSample[0]);
    }
}