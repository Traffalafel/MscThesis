using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Runner.Results
{
    internal class MultipleRunsComposite<T> : IDisplayableResult<T> where T : InstanceFormat
    {
        public string AlgorithmName;
        public Individual<T> Fittest { get; }
        public Dictionary<Property, double> ItemData { get; }

        public MultipleRunsComposite(IEnumerable<RunResult<T>> results)
        {
            ItemData = new Dictionary<Property, double>();
            var numRuns = 0;

            var values = new Dictionary<Property, List<double>>();
            foreach (var result in results)
            {
                if (AlgorithmName == null)
                {
                    AlgorithmName = result.AlgorithmName;
                }

                // Overwrite fittest if better
                var f = result.GetFittest();
                if (f > Fittest)
                {
                    Fittest = f;
                }

                foreach (var property in result.GetItemProperties(AlgorithmName))
                {
                    if (!values.ContainsKey(property))
                    {
                        values.Add(property, new List<double>());
                    }
                    values[property].Add(result.ItemData[property]);
                }
                numRuns++;
            }

            foreach (var property in values.Keys)
            {
                ItemData[property] = FindMedian(values[property]);
            }
        }

        private double FindMedian(List<double> values)
        {
            values.Sort();
            return values[values.Count / 2];
        }

        public Individual<T> GetFittest()
        {
            return Fittest;
        }

        public ICollection<Property> GetItemProperties()
        {
            return ItemData.Keys;
        }

        public IDictionary<string, double> GetItemValue(Property property)
        {
            if (ItemData.ContainsKey(property))
            {
                return new Dictionary<string, double>
                {
                    { AlgorithmName, ItemData[property] }
                };
            }
            else
            {
                return new Dictionary<string, double>();
            }
        }

        public ICollection<Property> GetSeriesProperties()
        {
            return new HashSet<Property>();
        }

        public IDictionary<string, List<double>> GetSeriesValues(Property property)
        {
            return new Dictionary<string, List<double>>();
        }

        public ICollection<string> GetCases()
        {
            throw new NotImplementedException();
        }

        public ICollection<Property> GetItemProperties(string testCase)
        {
            throw new NotImplementedException();
        }

        public double GetItemValue(string testCase, Property property)
        {
            throw new NotImplementedException();
        }

        public ICollection<Property> GetSeriesProperties(string testCase)
        {
            throw new NotImplementedException();
        }

        public List<double> GetSeriesValues(string testCase, Property property)
        {
            throw new NotImplementedException();
        }
    }
}
