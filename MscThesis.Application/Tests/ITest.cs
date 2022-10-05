using MscThesis.Core;
using MscThesis.Core.Formats;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public interface ITest<out T> where T : InstanceFormat
    {
        public bool IsTerminated { get; }
        public Task Execute();

        public ISet<string> OptimizerNames { get; }
        public IObservableValue<Individual<T>> Fittest { get; }
        public IEnumerable<ItemResult> Items { get; }
        public IEnumerable<SeriesResult> Series { get; }
        public IEnumerable<HistogramResult> Histograms { get; }
    }

    public abstract class Result
    {
        public string OptimizerName { get; set; }
        public Property Property { get; set; }
    }

    public class ItemResult : Result
    {
        public IObservableValue<double> Observable { get; set; }
    }

    public class SeriesResult : Result
    {
        public Property XAxis { get; set; }
        public ObservableCollection<(double x, double y)> Points { get; set; }
    }

    public class HistogramResult : Result
    {
        public ObservableCollection<double> Values { get; set; }
    }
}
