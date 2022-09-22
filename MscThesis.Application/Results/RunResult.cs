using MscThesis.Core.Formats;
using MscThesis.Core;
using System.Collections.Generic;
using System;

namespace MscThesis.Runner.Results
{
    // Results of ONE run of ONE algorithm on ONE problem of ONE size
    internal class RunResult<T> : IDisplayableResult<T> where T : InstanceFormat
    {
        public string AlgorithmName { get; }
        public Individual<T> Fittest { get; }
        public Dictionary<Property, List<double>> SeriesData { get; }
        public Dictionary<Property, double> ItemData { get; }

        public RunResult(IEnumerable<IterationResult<T>> results, FitnessFunction<T> fitnessFunction, string algorithmName)
        {
            AlgorithmName = algorithmName;
            ItemData = new Dictionary<Property, double>();
            SeriesData = new Dictionary<Property, List<double>>();

            var bestFitnesses = new List<double>();
            var populationSizes = new List<double>();
            Individual<T> globalFittest = null;
            var numIterations = 0;
            foreach (var result in results)
            {
                foreach (var (key, value) in result.Statistics)
                {
                    // Add to statistics
                    if (SeriesData.ContainsKey(key))
                    {
                        SeriesData[key].Add(value);
                    }
                    else
                    {
                        SeriesData.Add(key, new List<double> { value });
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

                populationSizes.Add((double) result.Population.Size);

                numIterations++;
            }

            Fittest = globalFittest;
            ItemData[Property.NumberIterations] = numIterations;
            ItemData[Property.NumberFitnessCalls] = fitnessFunction.GetNumCalls();
            SeriesData[Property.BestFitness] = bestFitnesses;
            SeriesData[Property.PopulationSize] = populationSizes;
        }

        public Individual<T> GetFittest()
        {
            return Fittest;
        }

        public ICollection<string> GetCases()
        {
            return new List<string> { AlgorithmName }; 
        }

        public ICollection<Property> GetItemProperties(string testCase)
        {
            if (testCase != AlgorithmName)
            {
                return new List<Property>();
            }

            return ItemData.Keys;
        }

        public double GetItemValue(string testCase, Property property)
        {
            if (testCase != AlgorithmName)
            {
                throw new Exception("");
            }

            if (ItemData.ContainsKey(property))
            {
                return ItemData[property];
            }
            else
            {
                throw new Exception("");
            }
        }

        public ICollection<Property> GetSeriesProperties(string testCase)
        {
            return SeriesData.Keys;
        }

        public List<double> GetSeriesValues(string testCase, Property property)
        {
            if (testCase != AlgorithmName)
            {
                throw new Exception("");
            }

            if (SeriesData.ContainsKey(property))
            {
                return SeriesData[property];
            }
            else
            {
                throw new Exception("");
            }
        }
    }
}
