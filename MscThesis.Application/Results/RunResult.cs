using MscThesis.Core.Formats;
using MscThesis.Core;
using System.Collections.Generic;
using System;

namespace MscThesis.Runner.Results
{
    // 1 run of 1 optimizer on 1 problem of 1 size
    public class RunResult<T> : IResult<T> where T : InstanceFormat
    {
        public string OptimizerName { get; }
        public Individual<T> Fittest { get; }
        public Dictionary<Property, List<double>> SeriesData { get; }
        public Dictionary<Property, double> ItemData { get; }

        public RunResult(IEnumerable<RunIteration<T>> iterations, FitnessFunction<T> fitnessFunction, string optimizerName)
        {
            OptimizerName = optimizerName;
            ItemData = new Dictionary<Property, double>();
            SeriesData = new Dictionary<Property, List<double>>();

            var bestFitnesses = new List<double>();
            var populationSizes = new List<double>();
            Individual<T> globalFittest = null;
            var numIterations = 0;
            foreach (var iteration in iterations)
            {
                foreach (var (key, value) in iteration.Statistics)
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

                var fittest = iteration.Population.GetFittest();
                if (fittest == null || fittest.Fitness == null)
                {
                    throw new Exception();
                }
                bestFitnesses.Add(fittest.Fitness.Value);
                if (globalFittest == null || globalFittest.Fitness < fittest.Fitness)
                {
                    globalFittest = fittest;
                }

                populationSizes.Add((double) iteration.Population.NumIndividuals);

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

        public IEnumerable<string> GetOptimizerNames()
        {
            return new List<string> { OptimizerName }; 
        }

        public IEnumerable<Property> GetItemProperties(string testCase)
        {
            if (testCase != OptimizerName)
            {
                return new List<Property>();
            }

            return ItemData.Keys;
        }

        public double GetItemValue(string testCase, Property property)
        {
            if (testCase != OptimizerName)
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

        public IEnumerable<Property> GetSeriesProperties(string testCase)
        {
            return SeriesData.Keys;
        }

        public List<double> GetSeriesValues(string testCase, Property property)
        {
            if (testCase != OptimizerName)
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
