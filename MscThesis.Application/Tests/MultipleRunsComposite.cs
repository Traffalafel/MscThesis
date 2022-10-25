using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Tests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MscThesis.Runner.Results
{
    internal class MultipleRunsComposite<T> : TestComposite<T> where T : InstanceFormat
    {
        private readonly ObservableValue<int> _numRuns;
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

        public MultipleRunsComposite(List<ITest<T>> results, int maxParallel) : base(results, maxParallel)
        {
            _numRuns = new ObservableValue<int>(0);
            _optimizerNames = new HashSet<string>();
            _itemValues = new Dictionary<string, Dictionary<Property, ObservableCollection<double>>>();
            _sums = new Dictionary<string, Dictionary<Property, double>>();
            _averages = new Dictionary<string, Dictionary<Property, ObservableValue<double>>>();
            _seriesValues = new Dictionary<string, Dictionary<Property, List<SeriesResult>>>();

            var first = results.First();
            foreach (var optimizerName in first.OptimizerNames)
            {
                _optimizerNames.Add(optimizerName);
                _itemValues.Add(optimizerName, new Dictionary<Property, ObservableCollection<double>>());
                _sums.Add(optimizerName, new Dictionary<Property, double>());
                _averages.Add(optimizerName, new Dictionary<Property, ObservableValue<double>>());
                _seriesValues.Add(optimizerName, new Dictionary<Property, List<SeriesResult>>());
            }

            foreach (var item in first.Items)
            {
                _itemValues[item.OptimizerName].Add(item.Property, new ObservableCollection<double>());
                _sums[item.OptimizerName].Add(item.Property, 0);
                _averages[item.OptimizerName].Add(item.Property, new ObservableValue<double>());
                _averages[item.OptimizerName][item.Property] = new ObservableValue<double>();
            }

            foreach (var result in results)
            {
                foreach (var series in result.Series)
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

        protected override void ConsumeResult(ITest<T> result)
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

            foreach (var series in result.Series)
            {
                var optimizerName = series.OptimizerName;
                var property = series.Property;
                _seriesValues[optimizerName][property].Add(series);
            }

            _numRuns.Value += 1;
        }
    }
}
