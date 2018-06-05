namespace OnTheFlyStats
{
    public interface IStats
    {
        double Average { get; }
        double GeometricAverage { get; }
        double Max { get; }
        double MidRange { get; }
        double Min { get; }
        int N { get; }
        double PopulationStandardDeviation { get; }
        double PopulationVariance { get; }
        double Range { get; }
        double RootMeanSquare { get; }
        double SampleStandardDeviation { get; }
        double SampleVariance { get; }
        double SquareMean { get; }
        double StandardError { get; }
        double Sum { get; }

        double StandardScore(double value);
        double Zscore(double sampleMean);
    }
}