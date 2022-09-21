using MscThesis.Core.Formats;
using MscThesis.Core;
using System.Collections.Generic;
using System;

namespace MscThesis.Application.Results
{
    // Results of ONE run of ONE algorithm on ONE problem of ONE size
    public class RunResult<T> : IDisplayData<T> where T : InstanceFormat
    {
        private string _algorithmName;
        public Individual<T> Fittest { get; }
        public Dictionary<Property, List<double>> SeriesData { get; }
        public int NumIterations { get; }
        public int NumFunctionCalls { get; }
        public List<double> BestFitnesses { get; }
        public List<int> PopulationSizes { get; }

        public RunResult(IEnumerable<IterationResult<T>> results, FitnessFunction<T> fitnessFunction, string algorithmName)
        {
            _algorithmName = algorithmName;

            var statistics = new Dictionary<Property, List<double>>();
            var bestFitnesses = new List<double>();
            var populationSizes = new List<int>();
            Individual<T> globalFittest = null;
            var numIterations = 0;
            foreach (var result in results)
            {
                foreach (var (key, value) in result.Statistics)
                {
                    // Add to statistics
                    if (statistics.ContainsKey(key))
                    {
                        statistics[key].Add(value);
                    }
                    else
                    {
                        statistics.Add(key, new List<double> { value });
                    }
                }

                var fittest = result.Population.GetFittest();
                if (fittest == null || fittest.Fitness == null)
                {
                    throw new Exception();
                }
                bestFitnesses.Add(fittest.Fitness.Value);
                if (globalFittest == null || globalFittest.Fitness < fittest.Fitness)
                {
                    globalFittest = fittest;
                }

                populationSizes.Add(result.Population.Size);

                numIterations++;
            }

            Fittest = globalFittest;
            SeriesData = statistics;
            NumIterations = numIterations;
            BestFitnesses = bestFitnesses;
            NumFunctionCalls = fitnessFunction.GetNumCalls();
            PopulationSizes = populationSizes;
        }

        public Individual<T> GetFittest()
        {
            return Fittest;
        }

        public Dictionary<(Property property, string algorithmName), double> GetValues()
        {
            return new Dictionary<(Property property, string algorithmName), double>
            {
                { (Property.NumberIterations, _algorithmName), NumIterations },
                { (Property.NumberFitnessCalls, _algorithmName), NumFunctionCalls }
            };
        }

        public Dictionary<(Property property, string algorithmName), (string xAxis, List<double> values)> GetSeriesValues()
        {
            return new Dictionary<(Property property, string algorithmName), (string xAxis, List<double> values)>
            {
            };
        }
    }
}
