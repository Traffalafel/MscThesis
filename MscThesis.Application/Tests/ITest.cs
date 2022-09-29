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

        public IEnumerable<string> GetOptimizerNames();
        public IEnumerable<Property> GetItemProperties(string optimizerName);
        public IEnumerable<Property> GetSeriesProperties(string optimizerName);
        public IEnumerable<Property> GetHistogramProperties(string optimizerName);

        public IObservableValue<Individual<T>> Fittest { get; }
        public IObservableValue<double> GetItemValue(string optimizerName, Property property);
        public ObservableCollection<double> GetSeriesValues(string optimizerName, Property property);
        public ObservableCollection<double> GetHistogramValues(string optimizerName, Property property);
    }
}
