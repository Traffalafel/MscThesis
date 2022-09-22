using MscThesis.Core;
using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Runner.Results
{
    public interface IResult<T> where T : InstanceFormat
    {
        public Individual<T> GetFittest();

        public IEnumerable<string> GetOptimizerNames();
        public IEnumerable<Property> GetItemProperties(string optimizerName);
        public double GetItemValue(string optimizerName, Property property);

        public IEnumerable<Property> GetSeriesProperties(string optimizerName);
        public List<double> GetSeriesValues(string optimizerName, Property property);
    }
}
