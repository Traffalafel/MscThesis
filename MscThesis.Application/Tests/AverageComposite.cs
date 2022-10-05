using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    internal class AverageComposite<T> : Test<T>, ITest<T> where T : InstanceFormat
    {
        private List<ITest<T>> _results;
        private int _batchSize;
        private ObservableValue<int> _numRuns;
        private HashSet<string> _optimizerNames;
        private Dictionary<string, Dictionary<Property, ObservableCollection<double>>> _values { get; }
        private Dictionary<string, Dictionary<Property, double>> _sums { get; }
        private Dictionary<string, Dictionary<Property, ObservableValue<double>>> _averages { get; }

        public ISet<string> OptimizerNames => _optimizerNames;

        public IEnumerable<ItemResult> Items => _averages.Select(opt =>
        {
            return opt.Value.Select(v => new ItemResult
            {
                OptimizerName = opt.Key,
                Property = v.Key,
                Observable = v.Value
            });
        }).SelectMany(x => x);

        public IEnumerable<SeriesResult> Series => new List<SeriesResult>();

        public IEnumerable<HistogramResult> Histograms => _values.Select(opt =>
        {
            return opt.Value.Select(v => new HistogramResult
            {
                OptimizerName = opt.Key,
                Property = v.Key,
                Values = v.Value
            });
        }).SelectMany(x => x);

        public AverageComposite(List<ITest<T>> results, int batchSize)
        {
            _results = results;
            _batchSize = batchSize;
            _numRuns = new ObservableValue<int>(0);
            _optimizerNames = new HashSet<string>();
            _values = new Dictionary<string, Dictionary<Property, ObservableCollection<double>>>();
            _sums = new Dictionary<string, Dictionary<Property, double>>();
            _averages = new Dictionary<string, Dictionary<Property, ObservableValue<double>>>();

            var first = results.First();
            foreach (var optimizerName in first.OptimizerNames)
            {
                _optimizerNames.Add(optimizerName);
                _values.Add(optimizerName, new Dictionary<Property, ObservableCollection<double>>());
                _sums.Add(optimizerName, new Dictionary<Property, double>());
                _averages.Add(optimizerName, new Dictionary<Property, ObservableValue<double>>());
            }

            foreach (var item in first.Items)
            {
                _values[item.OptimizerName].Add(item.Property, new ObservableCollection<double>());
                _sums[item.OptimizerName].Add(item.Property, 0);
                _averages[item.OptimizerName].Add(item.Property, new ObservableValue<double>());
                _averages[item.OptimizerName][item.Property] = new ObservableValue<double>();
            }

        }

        public async Task Execute()
        {
            var batches = _results.Select((result, idx) => (result, idx))
                                  .GroupBy(x => x.idx / _batchSize)
                                  .ToList();

            foreach (var batch in batches)
            {
                var remainingTasks = batch.Select(async x =>
                {
                    await x.result.Execute();
                    return x.result;
                }).ToList();

                while (remainingTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(remainingTasks);
                    var result = completedTask.Result;

                    TryUpdateFittest(result.Fittest?.Value);

                    foreach (var item in result.Items)
                    {
                        var optimizerName = item.OptimizerName;
                        var property = item.Property;
                        var observable = item.Observable;

                        _sums[optimizerName][property] += observable.Value;

                        var values = _values[optimizerName][property];
                        var sum = _sums[optimizerName][property];

                        values.Add(observable.Value);
                        var average = _averages[optimizerName][property];
                        average.Value = sum / values.Count;
                    }

                    _numRuns.Value += 1;
                    remainingTasks.Remove(completedTask);
                }

            }

            _isTerminated = true;
        }
    }
}
