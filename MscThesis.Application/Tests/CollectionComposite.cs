using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public class CollectionComposite<T> : ITest<T> where T : InstanceFormat
    {
        public bool IsTerminated => throw new NotImplementedException();

        public IObservableValue<Individual<T>> Fittest => throw new NotImplementedException();

        public CollectionComposite(IEnumerable<ITest<T>> results)
        {
            throw new NotImplementedException();
        }

        public Task Execute()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetItemProperties(string optimizerName)
        {
            throw new NotImplementedException();
        }

        public IObservableValue<double> GetItemValue(string optimizerName, Property property)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetOptimizerNames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetSeriesProperties(string optimizerName)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<double> GetSeriesValues(string optimizerName, Property property)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetHistogramProperties(string optimizerName)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<double> GetHistogramValues(string optimizerName, Property property)
        {
            throw new NotImplementedException();
        }
    }
}
