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
    public class SingleRun<T> : Test<T>, ITest<T> where T : InstanceFormat
    {
        private string _optimizerName;
        private Dictionary<Property, ObservableValue<double>> _itemData;
        private Dictionary<Property, ObservableCollection<(double, double)>> _seriesData;
        private IEnumerable<RunIteration<T>> _iterations;
        private FitnessFunction<T> _fitnessFunction;

        public SingleRun(IEnumerable<RunIteration<T>> iterations, FitnessFunction<T> fitnessFunction, string optimizerName, ISet<Property> statisticsProperties, int problemSize)
        {
            _iterations = iterations;
            _fitnessFunction = fitnessFunction;
            _optimizerName = optimizerName;

            _itemData = new Dictionary<Property, ObservableValue<double>>();
            _seriesData = new Dictionary<Property, ObservableCollection<(double, double)>>();

            foreach (var property in statisticsProperties)
            {
                _seriesData.Add(property, new ObservableCollection<(double, double)>());
            }
            _seriesData[Property.BestFitness] = new ObservableCollection<(double, double)>();
            _seriesData[Property.PopulationSize] = new ObservableCollection<(double, double)>();
            _seriesData[Property.AvgFitness] = new ObservableCollection<(double, double)>();
            _itemData[Property.BestFitness] = new ObservableValue<double>();
            _itemData[Property.NumberIterations] = new ObservableValue<double>();
            _itemData[Property.NumberFitnessCalls] = new ObservableValue<double>();
            _itemData[Property.AvgFitness] = new ObservableValue<double>();
            _itemData[Property.ProblemSize] = new ObservableValue<double>(problemSize);
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var numIterations = 0;
            await Task.Run(() =>
            {
                foreach (var iteration in _iterations)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return; // stop execution
                    }

                    foreach (var (key, value) in iteration.Statistics)
                    {
                        _seriesData[key].Add((numIterations, value));
                    }

                    var fittest = iteration.Population.GetFittest();
                    if (fittest == null || fittest.Fitness == null)
                    {
                        throw new Exception();
                    }
                    _seriesData[Property.BestFitness].Add((numIterations, fittest.Fitness.Value));
                    TryUpdateFittest(fittest);

                    var avgFitness = iteration.Population.GetAverageFitness();

                    _seriesData[Property.PopulationSize].Add((numIterations, iteration.Population.Size));
                    _seriesData[Property.AvgFitness].Add((numIterations, avgFitness));

                    _itemData[Property.BestFitness].Value = fittest.Fitness.Value;
                    _itemData[Property.NumberIterations].Value = numIterations;
                    _itemData[Property.NumberFitnessCalls].Value = _fitnessFunction.GetNumCalls();
                    _itemData[Property.AvgFitness].Value = avgFitness;

                    numIterations++;
                }
            });
        }


        public ISet<string> OptimizerNames => new HashSet<string> { _optimizerName };

        public IEnumerable<ItemResult> Items => _itemData.Select(item => new ItemResult
        {
            OptimizerName = _optimizerName,
            Property = item.Key,
            Observable = item.Value
        });

        public IEnumerable<SeriesResult> Series => _seriesData.Select(series => new SeriesResult
        {
            OptimizerName = _optimizerName,
            XAxis = Property.NumberIterations,
            Property = series.Key,
            Points = series.Value
        });

        public IEnumerable<HistogramResult> Histograms => new List<HistogramResult>();
    }
}
