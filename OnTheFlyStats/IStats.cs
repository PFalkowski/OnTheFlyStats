﻿using System;

namespace OnTheFlyStats
{
    public interface IStats<TT> where TT : IConvertible
    {
        TT Average { get; }
        TT GeometricAverage { get; }
        TT Max { get; }
        TT MidRange { get; }
        TT Min { get; }
        int N { get; }
        TT PopulationStandardDeviation { get; }
        TT PopulationVariance { get; }
        TT Range { get; }
        TT RootMeanSquare { get; }
        TT SampleStandardDeviation { get; }
        TT SampleVariance { get; }
        TT SquareMean { get; }
        TT StandardError { get; }
        TT Sum { get; }

        TT StandardScore(TT value);
        TT Zscore(TT sampleMean);
        TT Normalize(TT value);
    }
}