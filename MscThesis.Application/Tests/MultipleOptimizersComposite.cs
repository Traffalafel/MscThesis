using MscThesis.Core.Formats;
using MscThesis.Runner.Tests;
using System.Collections.Generic;

namespace MscThesis.Runner.Results
{
    public class MultipleOptimizersComposite<T> : TestComposite<T>, ITest<T> where T : InstanceFormat
    {
        private HashSet<string> _optimizerNames;
        private List<ItemResult> _items;
        private List<SeriesResult> _series;
        private List<HistogramResult> _hisograms;

        public MultipleOptimizersComposite(List<ITest<T>> tests, int maxParallel) : base(tests, maxParallel)
        {
            _optimizerNames = new HashSet<string>();
            _items = new List<ItemResult>();
            _series = new List<SeriesResult>();
            _hisograms = new List<HistogramResult>();

            foreach (var test in tests)
            {
                _optimizerNames.UnionWith(test.OptimizerNames);
                _items.AddRange(test.Items);
                _series.AddRange(test.Series);
                _hisograms.AddRange(test.Histograms);
            }
        }

        public ISet<string> OptimizerNames => _optimizerNames;
        public IEnumerable<ItemResult> Items => _items;
        public IEnumerable<SeriesResult> Series => _series;
        public IEnumerable<HistogramResult> Histograms => _hisograms;

        protected override void ConsumeResult(ITest<T> result)
        {
            // Do nothing
        }
    }
}
