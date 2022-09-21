using MscThesis.Core;
using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Application.Results
{
    public interface IDisplayData<T> where T : InstanceFormat
    {
        public Individual<T> GetFittest();
        public Dictionary<(Property property, string algorithmName), double> GetValues();
        public Dictionary<(Property property, string algorithmName), (string xAxis, List<double> values)> GetSeriesValues();
    }
}
