using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Results
{
    internal class MeanResult<T> : Result<T>, IResult<T> where T : InstanceFormat
    {
        private HashSet<string> _optimizerNames;
        private Dictionary<string, Dictionary<Property, double>> _medians { get; }

        public MeanResult(IEnumerable<IResult<T>> results)
        {
            _optimizerNames = new HashSet<string>();
            var values = new Dictionary<string, Dictionary<Property, List<double>>>();

            foreach (var result in results)
            {
                TryUpdateFittest(result.GetFittest());

                var names = result.GetOptimizerNames();
                foreach (var name in names)
                {
                    _optimizerNames.Add(name);
                    if (!values.ContainsKey(name))
                    {
                        values[name] = new Dictionary<Property, List<double>>();
                    }

                    foreach (var property in result.GetItemProperties(name))
                    {
                        if (!values[name].ContainsKey(property))
                        {
                            values[name].Add(property, new List<double>());
                        }
                        var value = result.GetItemValue(name, property);
                        values[name][property].Add(value);
                    }
                }
            }

            _medians = new Dictionary<string, Dictionary<Property, double>>();
            foreach (var name in _optimizerNames)
            {
                if (!_medians.ContainsKey(name))
                {
                    _medians.Add(name, new Dictionary<Property, double>());
                }

                foreach (var property in values[name].Keys)
                {
                    _medians[name][property] = FindMedian(values[name][property]);
                }
            }
        }

        private double FindMedian(List<double> values)
        {
            values.Sort();
            return values[values.Count / 2];
        }

        public Individual<T> GetFittest()
        {
            return _fittest;
        }

        public IEnumerable<string> GetOptimizerNames()
        {
            return _optimizerNames;
        }

        public IEnumerable<Property> GetItemProperties(string optimizerName)
        {
            if (_optimizerNames.Contains(optimizerName))
            {
                return _medians[optimizerName].Keys;
            }
            else
            {
                throw new Exception("");
            }
        }

        public double GetItemValue(string optimizerName, Property property)
        {
            if (!_optimizerNames.Contains(optimizerName))
            {
                throw new Exception();
            }
            if (!_medians[optimizerName].ContainsKey(property))
            {
                throw new Exception();
            }
            return _medians[optimizerName][property];
        }

        public IEnumerable<Property> GetSeriesProperties(string optimizerName)
        {
            return new List<Property>();
        }

        public List<double> GetSeriesValues(string optimizerName, Property property)
        {
            return new List<double>();
        }
    }
}
