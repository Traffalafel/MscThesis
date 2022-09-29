using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    internal class MeanComposite<T> : Test<T>, ITest<T> where T : InstanceFormat
    {
        private ObservableValue<int> _numRuns;
        private List<ITest<T>> _results;
        private HashSet<string> _optimizerNames;
        private Dictionary<string, Dictionary<Property, ObservableCollection<double>>> _values { get; }
        private Dictionary<string, Dictionary<Property, ObservableValue<double>>> _medians { get; }

        public MeanComposite(List<ITest<T>> results)
        {
            _results = results;
            _numRuns = new ObservableValue<int>(0);
            _optimizerNames = new HashSet<string>();
            _values = new Dictionary<string, Dictionary<Property, ObservableCollection<double>>>();
            _medians = new Dictionary<string, Dictionary<Property, ObservableValue<double>>>();

            var first = results.First();
            foreach (var optimizerName in first.GetOptimizerNames())
            {
                _optimizerNames.Add(optimizerName);
                _values.Add(optimizerName, new Dictionary<Property, ObservableCollection<double>>());
                _medians.Add(optimizerName, new Dictionary<Property, ObservableValue<double>>());

                foreach (var property in first.GetItemProperties(optimizerName))
                {
                    _values[optimizerName].Add(property, new ObservableCollection<double>());
                    _medians[optimizerName].Add(property, new ObservableValue<double>());
                    _medians[optimizerName][property] = new ObservableValue<double>();
                }
            }

        }

        public async Task Execute()
        {
            await Task.Run(async () =>
            {
                foreach (var result in _results)
                {
                    await result.Execute();

                    TryUpdateFittest(result.Fittest?.Value);

                    var names = result.GetOptimizerNames();
                    foreach (var name in names)
                    {
                        _optimizerNames.Add(name);
                        if (!_values.ContainsKey(name))
                        {
                            _values[name] = new Dictionary<Property, ObservableCollection<double>>();
                        }

                        foreach (var property in result.GetItemProperties(name))
                        {
                            if (!_values[name].ContainsKey(property))
                            {
                                _values[name].Add(property, new ObservableCollection<double>());
                            }
                            var observable = result.GetItemValue(name, property);
                            _values[name][property].Add(observable.Value);
                        }

                        foreach (var property in _values[name].Keys)
                        {
                            _medians[name][property].Value = FindMedian(_values[name][property]);
                        }
                    }
                }

                _numRuns.Value += 1;
            });
        }

        private double FindMedian(ObservableCollection<double> values)
        {
            var list = values.ToList();
            list.Sort();
            return list[list.Count / 2];
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

        public IObservableValue<double> GetItemValue(string optimizerName, Property property)
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

        public ObservableCollection<double> GetSeriesValues(string optimizerName, Property property)
        {
            return new ObservableCollection<double>();
        }

        public IEnumerable<Property> GetHistogramProperties(string optimizerName)
        {
            if (_optimizerNames.Contains(optimizerName))
            {
                return _values[optimizerName].Keys;
            }
            else
            {
                throw new Exception("");
            }
        }

        public ObservableCollection<double> GetHistogramValues(string optimizerName, Property property)
        {
            if (!_optimizerNames.Contains(optimizerName))
            {
                throw new Exception();
            }
            if (!_values[optimizerName].ContainsKey(property))
            {
                throw new Exception();
            }
            return _values[optimizerName][property];
        }
    }
}
