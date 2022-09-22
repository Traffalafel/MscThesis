using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Runner.Results
{
    public class SeriesResult<T> : IResult<T> where T : InstanceFormat
    {
        public SeriesResult(IEnumerable<IResult<T>> results)
        {

        }

        public Individual<T> GetFittest()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetItemProperties(string optimizerName)
        {
            throw new NotImplementedException();
        }

        public double GetItemValue(string optimizer, Property property)
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

        public List<double> GetSeriesValues(string optimizer, Property property)
        {
            throw new NotImplementedException();
        }
    }
}
