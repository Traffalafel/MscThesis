using MscThesis.Core;
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

        public MultipleOptimizersComposite(List<ITest<T>> tests, int maxParallel) : base(tests, maxParallel)
        {
            _optimizerNames = new HashSet<string>();
            _items = new List<ItemResult>();
            _series = new List<SeriesResult>();

            foreach (var test in tests)
            {
                _optimizerNames.UnionWith(test.OptimizerNames);
                _items.AddRange(test.Items);
                _series.AddRange(test.Series);
            }
        }

        public override ISet<string> OptimizerNames => _optimizerNames;
        public override IEnumerable<ItemResult> Items => _items;
        public override IEnumerable<SeriesResult> Series => _series;

        protected override void ConsumeResult(ITest<T> result)
        {
            // Do nothing
        }
    }
}
