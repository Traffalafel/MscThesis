using MscThesis.Core.Formats;
using MscThesis.Core;
using System.Collections.Generic;
using System;
using MscThesis.Core.FitnessFunctions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace MscThesis.Runner.Results
{
    // 1 run of 1 optimizer on 1 problem of 1 size
    public class SingleRun<T> : Test<T> where T : InstanceFormat
    {
        private bool _saveSeries;
        private string _optimizerName;
        private Dictionary<Property, ObservableValue<double>> _itemData;
        private Dictionary<Property, ObservableCollection<(double, double)>> _seriesData;
        private IEnumerable<RunIteration<T>> _iterations;
        private FitnessFunction<T> _fitnessFunction;

        public SingleRun(
            IEnumerable<RunIteration<T>> iterations, 
            FitnessFunction<T> fitnessFunction, 
            string optimizerName, 
            ISet<Property> statisticsProperties, 
            int problemSize,
            bool saveSeries)
        {
            _saveSeries = saveSeries;
            _iterations = iterations;
            _fitnessFunction = fitnessFunction;
            _optimizerName = optimizerName;
            _instanceType = fitnessFunction.InstanceType;
            _comparisonStrategy = fitnessFunction.ComparisonStrategy;

            _itemData = new Dictionary<Property, ObservableValue<double>>();
            _seriesData = new Dictionary<Property, ObservableCollection<(double, double)>>();

            foreach (var property in statisticsProperties)
            {
                _seriesData.Add(property, new ObservableCollection<(double, double)>());
            }

            if (_saveSeries)
            {
                _seriesData[Property.BestFitness] = new ObservableCollection<(double, double)>();
                _seriesData[Property.PopulationSize] = new ObservableCollection<(double, double)>();
                _seriesData[Property.AvgFitness] = new ObservableCollection<(double, double)>();
            }
            _itemData[Property.BestFitness] = new ObservableValue<double>();
            _itemData[Property.NumberIterations] = new ObservableValue<double>();
            _itemData[Property.NumberFitnessCalls] = new ObservableValue<double>();
            _itemData[Property.AvgFitness] = new ObservableValue<double>();
            _itemData[Property.ProblemSize] = new ObservableValue<double>(problemSize);

            Initialize(new List<string> { optimizerName });
        }

        public override Task Execute(CancellationToken cancellationToken)
        {
            var numIterations = 1;

            foreach (var iteration in _iterations)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.CompletedTask; // stop execution
                }

                if (_saveSeries)
                {
                    foreach (var (key, value) in iteration.Statistics)
                    {
                        _seriesData[key].Add((numIterations, value));
                    }
                }

                var iterationFittest = iteration.Population.Fittest;
                if (iterationFittest == null || iterationFittest.Fitness == null)
                {
                    throw new Exception();
                }
                TryUpdateFittest(_optimizerName, iterationFittest);

                var avgFitness = iteration.Population.GetAverageFitness();

                var bestFitness = Fittest(_optimizerName).Value.Fitness.Value;

                if (_saveSeries)
                {
                    lock (SeriesLock)
                    {
                        _seriesData[Property.BestFitness].Add((numIterations, bestFitness));
                        _seriesData[Property.PopulationSize].Add((numIterations, iteration.Population.Size));
                        _seriesData[Property.AvgFitness].Add((numIterations, avgFitness));
                    }
                }

                if (_fittest[_optimizerName].Value != null)
                {
                    _itemData[Property.BestFitness].Value = _fittest[_optimizerName].Value.Fitness.Value;
                }
                _itemData[Property.NumberIterations].Value = numIterations;
                _itemData[Property.NumberFitnessCalls].Value = _fitnessFunction.GetNumCalls();
                _itemData[Property.AvgFitness].Value = Math.Round(avgFitness, 2);

                numIterations++;
            }

            _isTerminated = true;
            return Task.CompletedTask;
        }


        public override ISet<string> OptimizerNames => new HashSet<string> { _optimizerName };
        public override IEnumerable<ItemResult> Items => _itemData.Select(item => new ItemResult
        {
            OptimizerName = _optimizerName,
            Property = item.Key,
            Observable = item.Value
        });
        public override IEnumerable<SeriesResult> Series => _seriesData.Select(data => new SeriesResult
        {
            OptimizerName = _optimizerName,
            XAxis = Property.NumberIterations,
            Property = data.Key,
            Points = data.Value
        });
    }
}
