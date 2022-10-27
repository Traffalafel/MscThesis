using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    internal class MultipleRunsComposite<T> : Test<T> where T : InstanceFormat
    {
        private readonly ITestCase<T> _generator;
        private readonly bool _saveSeries;
        private readonly int _size;
        private readonly int _maxParallel;
        private readonly int _numRuns;
        private readonly HashSet<string> _optimizerNames;

        private Dictionary<string, Dictionary<Property, ObservableCollection<double>>> _itemValues { get; }
        private Dictionary<string, Dictionary<Property, List<SeriesResult>>> _seriesValues { get; }
        private Dictionary<string, Dictionary<Property, double>> _sums { get; }
        private Dictionary<string, Dictionary<Property, ObservableValue<double>>> _averages { get; }


        public override ISet<string> OptimizerNames => _optimizerNames;
        public override IEnumerable<ItemResult> Items => _averages.Select(opt =>
        {
            return opt.Value.Select(v => new ItemResult
            {
                OptimizerName = opt.Key,
                Property = v.Key,
                Observable = v.Value
            });
        }).SelectMany(x => x);

        public override IEnumerable<SeriesResult> Series => _seriesValues.SelectMany(x => x.Value).SelectMany(x => x.Value);

        public MultipleRunsComposite(ITestCase<T> generator, int size, int numRuns, int maxParallel, bool saveSeries)
        {
            _generator = generator;
            _size = size;
            _optimizerNames = new HashSet<string>();
            _numRuns = numRuns;
            _maxParallel = maxParallel;
            _itemValues = new Dictionary<string, Dictionary<Property, ObservableCollection<double>>>();
            _sums = new Dictionary<string, Dictionary<Property, double>>();
            _averages = new Dictionary<string, Dictionary<Property, ObservableValue<double>>>();
            _saveSeries = saveSeries;

            var empty = generator.CreateRun(size, saveSeries);
            _instanceType = empty.InstanceType;
            _comparisonStrategy = empty.ComparisonStrategy;

            _seriesValues = new Dictionary<string, Dictionary<Property, List<SeriesResult>>>();

            foreach (var optimizerName in empty.OptimizerNames)
            {
                Initialize(empty.OptimizerNames);
                _optimizerNames.Add(optimizerName);
                _itemValues.Add(optimizerName, new Dictionary<Property, ObservableCollection<double>>());
                _sums.Add(optimizerName, new Dictionary<Property, double>());
                _averages.Add(optimizerName, new Dictionary<Property, ObservableValue<double>>());

                if (_saveSeries)
                {
                    _seriesValues.Add(optimizerName, new Dictionary<Property, List<SeriesResult>>());
                }
            }

            foreach (var item in empty.Items)
            {
                _itemValues[item.OptimizerName].Add(item.Property, new ObservableCollection<double>());
                _sums[item.OptimizerName].Add(item.Property, 0);
                _averages[item.OptimizerName].Add(item.Property, new ObservableValue<double>());
                _averages[item.OptimizerName][item.Property] = new ObservableValue<double>();
            }

            if (_saveSeries)
            {
                foreach (var series in empty.Series)
                {
                    var optimizerName = series.OptimizerName;
                    var property = series.Property;
                    if (!_seriesValues.ContainsKey(optimizerName))
                    {
                        _seriesValues.Add(optimizerName, new Dictionary<Property, List<SeriesResult>>());
                    }
                    if (!_seriesValues[optimizerName].ContainsKey(property))
                    {
                        _seriesValues[optimizerName].Add(property, new List<SeriesResult>());
                    }
                    _seriesValues[optimizerName][property].Add(series);
                }
            }

        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            var numConcurrent = _numRuns > _maxParallel ? _maxParallel : _numRuns;
            var numRemaining = _numRuns - numConcurrent;

            var tasks = Enumerable.Range(0, numConcurrent)
                                  .Select(async _ =>
                                  {
                                      var test = CreateTest();
                                      await test.Execute(cancellationToken);
                                      return test;
                                  })
                                  .ToList();

            while (tasks.Count() > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                var result = completedTask.Result;

                if (cancellationToken.IsCancellationRequested)
                {
                    return; // stop execution
                }

                TryUpdateFittest(result);
                ConsumeResult(result);

                tasks.Remove(completedTask);

                if (numRemaining > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var test = CreateTest();
                        await test.Execute(cancellationToken);
                        return test;
                    }));
                    numRemaining--;
                }
            }

            _isTerminated = true;
        }

        private ITest<T> CreateTest()
        {
            var test = _generator.CreateRun(_size, _saveSeries);
            foreach (var name in test.OptimizerNames)
            {
                var observable = test.Fittest(name);
                observable.PropertyChanged += (s, e) =>
                {
                    if (_comparisonStrategy.IsFitter(observable.Value, _fittest[name].Value))
                    {
                        _fittest[name].Value = observable.Value;
                    }
                };
            }
            test.SetLock(SeriesLock);
            return test;
        }

        private void ConsumeResult(ITest<T> result)
        {
            foreach (var item in result.Items)
            {
                var optimizerName = item.OptimizerName;
                var property = item.Property;
                var observable = item.Observable;

                _sums[optimizerName][property] += observable.Value;

                var values = _itemValues[optimizerName][property];
                var sum = _sums[optimizerName][property];

                values.Add(observable.Value);
                var average = _averages[optimizerName][property];
                average.Value = sum / values.Count;
            }

            if (_saveSeries)
            {
                foreach (var series in result.Series)
                {
                    var optimizerName = series.OptimizerName;
                    var property = series.Property;
                    _seriesValues[optimizerName][property].Add(series);
                }
            }
        }

    }
}
