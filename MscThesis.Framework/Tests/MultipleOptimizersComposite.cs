using MscThesis.Framework.Tests;
using System.Collections.Generic;

namespace MscThesis.Framework.Tests
{
    public class MultipleOptimizersComposite : TestComposite, ITest
    {
        private HashSet<string> _optimizerNames;
        private List<ItemResult> _items;
        private List<SeriesResult> _series;

        public MultipleOptimizersComposite(List<ITest> tests, int maxParallel) : base(tests, maxParallel)
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

        protected override void ConsumeResult(ITest result)
        {
            TriggerOptimizerDone();
        }
    }
}
