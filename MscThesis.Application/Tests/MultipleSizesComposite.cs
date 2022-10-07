using MscThesis.Core;
using MscThesis.Core.Formats;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public class MultipleSizesComposite<T> : Test<T>, ITest<T> where T : InstanceFormat
    {
        private List<ITest<T>> _tests;
        private Property _xAxis;
        private int _batchSize;
        private HashSet<string> _optimizerNames;
        private Dictionary<string, Dictionary<Property, ObservableCollection<(double,double)>>> _points { get; }

        public ISet<string> OptimizerNames => _optimizerNames;
        public IEnumerable<ItemResult> Items => new List<ItemResult>();
        public IEnumerable<SeriesResult> Series => _points.Select(opt =>
        {
            return opt.Value.Select(v => new SeriesResult
            {
                OptimizerName = opt.Key,
                Property = v.Key,
                XAxis = _xAxis,
                Points = v.Value
            });
        }).SelectMany(x => x);
        public IEnumerable<HistogramResult> Histograms => new List<HistogramResult>();

        public MultipleSizesComposite(List<ITest<T>> tests, Property xAxis, int batchSize)
        {
            _tests = tests;
            _xAxis = xAxis;
            _batchSize = batchSize;
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

        public async Task Execute(CancellationToken cancellationToken)
        {
            var batches = _tests.Select((result, idx) => (result, idx))
                                  .GroupBy(x => x.idx / _batchSize)
                                  .ToList();

            foreach (var batch in batches)
            {
                var remainingTasks = batch.Select(async x =>
                {
                    await x.result.Execute(cancellationToken);
                    return x.result;
                }).ToList();

                while (remainingTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(remainingTasks);
                    var result = completedTask.Result;

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return; // stop execution
                    }

                    TryUpdateFittest(result.Fittest?.Value);

                    var xItem = result.Items.FirstOrDefault(item => item.Property == _xAxis);
                    var xValue = xItem.Observable.Value;

                    foreach (var item in NotX(result.Items))
                    {
                        _optimizerNames.Add(item.OptimizerName);

                        var observable = item.Observable;
                        _points[item.OptimizerName][item.Property].Add((xValue, observable.Value));
                    }

                    remainingTasks.Remove(completedTask);
                }

                _isTerminated = true;
            }
        }

        private IEnumerable<ItemResult> NotX(IEnumerable<ItemResult> input)
        {
            return input.Where(item => item.Property != _xAxis);
        }

    }
}
