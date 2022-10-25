﻿using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Tests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MscThesis.Runner.Results
{
    public class MultipleSizesComposite<T> : TestComposite<T>, ITest<T> where T : InstanceFormat
    {
        private Property _xAxis;
        private HashSet<string> _optimizerNames;
        private Dictionary<string, Dictionary<Property, ObservableCollection<(double,double)>>> _points { get; }

        public override ISet<string> OptimizerNames => _optimizerNames;
        public override IEnumerable<ItemResult> Items => new List<ItemResult>();
        public override IEnumerable<SeriesResult> Series => _points.Select(opt =>
        {
            return opt.Value.Select(v => new SeriesResult
            {
                OptimizerName = opt.Key,
                Property = v.Key,
                XAxis = _xAxis,
                Points = v.Value
            });
        }).SelectMany(x => x);

        public MultipleSizesComposite(List<ITest<T>> tests, int maxParallel, Property xAxis) : base(tests, maxParallel)
        {
            _xAxis = xAxis;
            _optimizerNames = new HashSet<string>();
            _points = new Dictionary<string, Dictionary<Property, ObservableCollection<(double,double)>>>();

            var first = tests.First();
            foreach (var name in first.OptimizerNames)
            {
                _optimizerNames.Add(name);
                _points[name] = new Dictionary<Property, ObservableCollection<(double,double)>>();
            }

            foreach (var item in NotX(first.Items))
            {
                _points[item.OptimizerName].Add(item.Property, new ObservableCollection<(double,double)>());
            }
        }

        protected override void ConsumeResult(ITest<T> result)
        {
            var xItem = result.Items.FirstOrDefault(item => item.Property == _xAxis);
            var xValue = xItem.Observable.Value;

            foreach (var item in NotX(result.Items))
            {
                _optimizerNames.Add(item.OptimizerName);

                var observable = item.Observable;

                lock (SeriesLock)
                {
                    _points[item.OptimizerName][item.Property].Add((xValue, observable.Value));
                }
            }
        }

        private IEnumerable<ItemResult> NotX(IEnumerable<ItemResult> input)
        {
            return input.Where(item => item.Property != _xAxis);
        }

    }
}
