using MscThesis.Core;
using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Runner.Results
{
    public interface IDisplayableResult<T> where T : InstanceFormat
    {
        public Individual<T> GetFittest();

        public ICollection<string> GetCases();
        public ICollection<Property> GetItemProperties(string testCase);
        public double GetItemValue(string testCase, Property property);

        public ICollection<Property> GetSeriesProperties(string testCase);
        public List<double> GetSeriesValues(string testCase, Property property);
    }
}
