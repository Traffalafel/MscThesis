using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Framework.Factories;
using MscThesis.Framework.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MscThesis.Framework.Tests
{
    internal class MultipleRunsComposite : TestComposite
    {
        private readonly bool _saveSeries;
        private readonly HashSet<string> _optimizerNames;

        private Dictionary<string, Dictionary<Property, ObservableCollection<double>>> _itemValues { get; }
        private Dictionary<string, Dictionary<Property, SeriesResult>> _seriesValues { get; }
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

        public override IEnumerable<SeriesResult> Series => _seriesValues.SelectMany(x => x.Value).Select(x => x.Value);

        public MultipleRunsComposite(Func<ITest> generate, int numRuns, int maxParallel, bool saveSeries, VariableSpecification varSpec) 
            : base(generate, numRuns, maxParallel)
        {
            _optimizerNames = new HashSet<string>();
            _itemValues = new Dictionary<string, Dictionary<Property, ObservableCollection<double>>>();
            _seriesValues = new Dictionary<string, Dictionary<Property, SeriesResult>>();
            _sums = new Dictionary<string, Dictionary<Property, double>>();
            _averages = new Dictionary<string, Dictionary<Property, ObservableValue<double>>>();
            _saveSeries = saveSeries;

            var empty = generate();
            _instanceType = empty.InstanceType;
            _comparisonStrategy = empty.Comparison;

            VariableValue = empty.VariableValue;

            foreach (var optimizerName in empty.OptimizerNames)
            {
                _optimizerNames.Add(optimizerName);
                _itemValues.Add(optimizerName, new Dictionary<Property, ObservableCollection<double>>());
                _sums.Add(optimizerName, new Dictionary<Property, double>());
                _averages.Add(optimizerName, new Dictionary<Property, ObservableValue<double>>());

                if (_saveSeries)
                {
                    _seriesValues.Add(optimizerName, new Dictionary<Property, SeriesResult>());
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
                        _seriesValues.Add(optimizerName, new Dictionary<Property, SeriesResult>());
                    }
                    if (!_seriesValues[optimizerName].ContainsKey(property))
                    {
                        series.IsScatter = true;
                        _seriesValues[optimizerName].Add(property, series);
                    }
                }
            }

        }

        protected override void ConsumeResult(ITest result)
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
                    foreach (var point in series.Points)
                    {
                        _seriesValues[optimizerName][property].Points.Add(point);
                    }
                }
            }
        }

    }
}
