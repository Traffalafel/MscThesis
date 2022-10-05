using MscThesis.Core.Formats;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MscThesis.Runner.Results
{
    public class CollectionComposite<T> : Test<T>, ITest<T> where T : InstanceFormat
    {
        private List<ITest<T>> _tests;
        private int _batchSize;
        private HashSet<string> _optimizerNames;

        private List<ItemResult> _items;
        private List<SeriesResult> _series;
        private List<HistogramResult> _hisograms;

        public CollectionComposite(List<ITest<T>> tests, int batchSize)
        {
            _tests = tests;
            _batchSize = batchSize;
            _optimizerNames = new HashSet<string>();
            _items = new List<ItemResult>();
            _series = new List<SeriesResult>();
            _hisograms = new List<HistogramResult>();

            foreach (var test in _tests)
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

        public async Task Execute()
        {
            var batches = _tests.Select((result, idx) => (result, idx))
                                  .GroupBy(x => x.idx / _batchSize)
                                  .ToList();

            foreach (var batch in batches)
            {
                var tasks = batch.Select(x => Task.Run(() => x.result.Execute()));

                await Task.WhenAll(tasks);
            }
        }
    }
}
