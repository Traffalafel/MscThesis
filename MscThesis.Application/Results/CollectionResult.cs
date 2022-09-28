using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public class CollectionResult<T> : Result<T>, IResult<T> where T : InstanceFormat
    {
        public CollectionResult(IEnumerable<IResult<T>> results)
        {

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
    }
}
