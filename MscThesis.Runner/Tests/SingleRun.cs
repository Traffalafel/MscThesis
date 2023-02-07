using MscThesis.Core;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace MscThesis.Runner.Tests
{
    // 1 run of 1 optimizer on 1 problem of 1 size
    public class SingleRun : Test
    {
        private bool _saveSeries;
        private string _optimizerName;
        private Dictionary<Property, ObservableValue<double>> _itemData;
        private Dictionary<Property, ObservableCollection<(double, double)>> _seriesData;
        private Run _iterations;
        private Func<int> _getNumCalls;

        public SingleRun(
            Run iterations, 
            Func<int> getNumCalls,
            FitnessComparison fitnessComparison,
            string optimizerName, 
            ISet<Property> statisticsProperties, 
            double variableValue,
            bool saveSeries)
        {
            _saveSeries = saveSeries;
            _iterations = iterations;
            _getNumCalls = getNumCalls;
            _optimizerName = optimizerName;
            _comparisonStrategy = fitnessComparison;
            VariableValue = variableValue;

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
            }
            _itemData[Property.BestFitness] = new ObservableValue<double>();
            _itemData[Property.NumberIterations] = new ObservableValue<double>();
            _itemData[Property.NumberFitnessCalls] = new ObservableValue<double>();
            _itemData[Property.CpuTimeSeconds] = new ObservableValue<double>(0.0d);

            // Terminations
            _itemData[Property.OptimumReached] = new ObservableValue<double>(0.0d);
            _itemData[Property.Stagnation] = new ObservableValue<double>(0.0d);
            _itemData[Property.MaxIterations] = new ObservableValue<double>(0.0d);

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

                var fitnesses = iteration.Population.Select(i => i.Fitness);
                var iterationFittest = _comparisonStrategy.GetFittest(fitnesses);
                TryUpdateFittest(_optimizerName, iterationFittest);
                var bestFitness = BestFitness(_optimizerName).Value;

                var populationSize = iteration.Population.Count();

                if (_saveSeries)
                {
                    lock (SeriesLock)
                    {
                        if (bestFitness != null)
                        {
                            _seriesData[Property.BestFitness].Add((numIterations, bestFitness.Value));
                        }
                        _seriesData[Property.PopulationSize].Add((numIterations, populationSize));
                    }
                }

                if (_bestFitness[_optimizerName].Value != null)
                {
                    _itemData[Property.BestFitness].Value = _bestFitness[_optimizerName].Value.Value;
                }
                _itemData[Property.NumberIterations].Value = numIterations;
                _itemData[Property.NumberFitnessCalls].Value = _getNumCalls();

                if (iteration.CpuTime != null)
                {
                    _itemData[Property.CpuTimeSeconds].Value += iteration.CpuTime.TotalSeconds;
                }

                numIterations++;
            }

            _itemData[_iterations.TerminationReason].Value++;

            var cpuTime = _itemData[Property.CpuTimeSeconds].Value;
            Console.WriteLine($"Optimizer: {_optimizerName}; Variable: {VariableValue}; CPU time: {cpuTime}; Termination: {_iterations.TerminationReason}");

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
            XAxis = Parameter.NumberIterations,
            Property = data.Key,
            Points = data.Value
        });
    }
}
