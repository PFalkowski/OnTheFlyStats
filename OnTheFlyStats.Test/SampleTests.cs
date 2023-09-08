using System;
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

    [Fact]
    public void Update_AddsValueToInternalSample_ByConvertibleMethod()
    {
        // Arrange
        var sample = new Sample();
        IConvertible value1 = 5.0;
        IConvertible value2 = 3.0;
        IConvertible value3 = 7.0;

        // Act
        sample.Update(value1);
        sample.Update(value2);
        sample.Update(value3);

        // Assert
        Assert.Equal(value1, sample.InternalSample[0]);
        Assert.Equal(value2, sample.InternalSample[1]);
        Assert.Equal(value3, sample.InternalSample[2]);
    }
}