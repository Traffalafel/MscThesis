using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public interface ITest
    {
        public bool IsTerminated { get; }
        public Task Execute(CancellationToken cancellationToken);

        public ISet<string> OptimizerNames { get; }
        public IEnumerable<ItemResult> Items { get; }
        public IEnumerable<SeriesResult> Series { get; }
        IObservableValue<double?> BestFitness(string optimizername);
        public Type InstanceType { get; }
        public FitnessComparison Comparison { get; }
        public object SeriesLock { get; }
        public void SetLock(object newLock);
        public event EventHandler<EventArgs> OptimizerDone;
        public double VariableValue { get; }
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
        public Parameter XAxis { get; set; }
        public ObservableCollection<(double x, double y)> Points { get; set; }
        public bool IsScatter { get; set; } = false;
    }

    public class HistogramResult : Result
    {
        public ObservableCollection<double> Values { get; set; }
    }
}
