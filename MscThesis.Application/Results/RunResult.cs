using MscThesis.Core.Formats;
using MscThesis.Core;
using System.Collections.Generic;
using System;
using MscThesis.Core.FitnessFunctions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace MscThesis.Runner.Results
{
    // 1 run of 1 optimizer on 1 problem of 1 size
    public class RunResult<T> : Result<T>, IResult<T> where T : InstanceFormat
    {
        private string _optimizerName;
        private Dictionary<Property, ObservableValue<double>> _itemData;
        private Dictionary<Property, ObservableCollection<double>> _seriesData;
        private IEnumerable<RunIteration<T>> _iterations;
        private FitnessFunction<T> _fitnessFunction;

        public string OptimizerName => _optimizerName;
        public Dictionary<Property, ObservableCollection<double>> SeriesData => _seriesData;
        public Dictionary<Property, ObservableValue<double>> ItemData => _itemData;

        public async Task Execute()
        {
            var numIterations = 0;
            await Task.Run(() =>
            {
                foreach (var iteration in _iterations)
                {
                    foreach (var (key, value) in iteration.Statistics)
                    {
                        SeriesData[key].Add(value);
                    }

                    var fittest = iteration.Population.GetFittest();
                    if (fittest == null || fittest.Fitness == null)
                    {
                        throw new Exception();
                    }
                    SeriesData[Property.BestFitness].Add(fittest.Fitness.Value);
                    TryUpdateFittest(fittest);

                    SeriesData[Property.PopulationSize].Add(iteration.Population.NumIndividuals);

                    ItemData[Property.NumberIterations].Value = numIterations;
                    ItemData[Property.NumberFitnessCalls].Value = _fitnessFunction.GetNumCalls();

                    numIterations++;
                }
            });
        }

        public RunResult(IEnumerable<RunIteration<T>> iterations, FitnessFunction<T> fitnessFunction, string optimizerName)
        {
            _iterations = iterations;
            _fitnessFunction = fitnessFunction;
            _optimizerName = optimizerName;

            _itemData = new Dictionary<Property, ObservableValue<double>>();
            _seriesData = new Dictionary<Property, ObservableCollection<double>>();

            var first = iterations.First();
            foreach (var (key, value) in first.Statistics)
            {
                SeriesData.Add(key, new ObservableCollection<double> { value });
            }

            SeriesData[Property.BestFitness] = new ObservableCollection<double>();
            SeriesData[Property.PopulationSize] = new ObservableCollection<double>();
            ItemData[Property.NumberIterations] = new ObservableValue<double>();
            ItemData[Property.NumberFitnessCalls] = new ObservableValue<double>();
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

        public IObservableValue<double> GetItemValue(string testCase, Property property)
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

        public ObservableCollection<double> GetSeriesValues(string testCase, Property property)
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
