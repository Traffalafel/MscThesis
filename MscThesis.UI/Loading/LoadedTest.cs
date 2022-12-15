using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner;
using MscThesis.Runner.Results;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MscThesis.UI.Models
{
    internal class LoadedTest : ITest<InstanceFormat>
    {
        private object _seriesLock = new object { };
        private HashSet<string> _optimizerNames;
        private List<ItemResult> _items;
        private List<SeriesResult> _series;
        private Dictionary<string, Individual<InstanceFormat>> _fittest;
        private Type _instanceType = typeof(InstanceFormat);


        public event EventHandler<EventArgs> OptimizerDone;

        public bool IsTerminated => true;
        public object SeriesLock => _seriesLock;
        public double VariableValue { get; } = 0.0d;
 
        public LoadedTest(List<string> lines)
        {
            _optimizerNames = new();
            _items = new();
            _series = new();
            _fittest = new();

            var c = 0;

            if (lines[c] != "Type:")
            {
                throw new Exception();
            }
            c++;

            Func<string, InstanceFormat> createInstanceFunc;
            if (lines[c] == "BitString")
            {
                _instanceType = typeof(BitString);
                createInstanceFunc = BitString.FromString;
            }
            else if (lines[c] == "Tour")
            {
                _instanceType = typeof(Tour);
                createInstanceFunc = Tour.FromNodesString;
            } 
            else
            {
                throw new Exception();
            }

            c++;
            if (lines[c] != "Fittest:")
            {
                throw new Exception();
            }

            c++;
            while (lines[c] != "Items:")
            {
                try
                {
                    var parsed = ParseFittestLine(lines[c], createInstanceFunc);
                    _fittest.Add(parsed.optimizerName, parsed.individual);
                    c++;
                }
                catch (Exception e)
                {
                    throw new Exception();
                }
            }

            c++;
            while (lines[c] != "Series:")
            {
                // Parse items
                try
                {
                    var line = lines[c];
                    c++;
                    var item = ParseItemLine(line);
                    _optimizerNames.Add(item.OptimizerName);
                    _items.Add(item);
                }
                catch
                {
                    ; // ignore
                }
            }

            c++;
            while (c < lines.Count)
            {
                // Parse series
                try
                {
                    var line = lines[c];
                    c++;
                    var series = ParseSeriesLine(line);
                    _optimizerNames.Add(series.OptimizerName);
                    _series.Add(series);
                }
                catch
                {
                    ; // ignore
                }
            }

        }

        private (string optimizerName, Individual<InstanceFormat> individual) ParseFittestLine(string line, Func<string, InstanceFormat> createInstanceFunc)
        {
            var split = line.Split(';');
            var optimizerName = split[0];
            var valueStr = split[1];
            var fitnessStr = split[2];

            var instance = createInstanceFunc(valueStr);
            var fitness = Convert.ToInt32(fitnessStr);
            var individual = new IndividualImpl<InstanceFormat>(instance, fitness);
            return (optimizerName, individual);
        }

        private ItemResult ParseItemLine(string line)
        {
            var split = line.Split('\t');
            var def = split[0];
            var valueStr = split[1];
            var value = Convert.ToDouble(valueStr, CultureInfo.InvariantCulture);

            var defSplit = def.Split(';');
            var optimizerName = defSplit[0];
            var propertyName = defSplit[1];
            _ = Enum.TryParse(propertyName, out Property property);

            return new ItemResult
            {
                OptimizerName = optimizerName,
                Property = property,
                Observable = new ObservableValue<double>(value)
            };
        }

        private SeriesResult ParseSeriesLine(string line)
        {
            var split = line.Split('\t');
            var def = split[0];
            var valueStr = split[1];

            var values = valueStr.Replace("(", "")
                                 .Replace(")", "")
                                 .Split(';');
            var points = values.Select(v =>
            {
                var s = v.Split(',');
                var x = Convert.ToDouble(s[0], CultureInfo.InvariantCulture);
                var y = Convert.ToDouble(s[1], CultureInfo.InvariantCulture);
                return (x, y);
            }).ToList();

            var defSplit = def.Split(';');
            var optimizerName = defSplit[0];
            var yPropertyName = defSplit[1];
            var xParameterName = defSplit[2];

            var isScatter = defSplit.Length == 4;

            _ = Enum.TryParse(yPropertyName, out Property yProperty);
            _ = Enum.TryParse(xParameterName, out Parameter xProperty);

            return new SeriesResult
            {
                OptimizerName = optimizerName,
                Property = yProperty,
                XAxis = xProperty,
                Points = new ObservableCollection<(double x, double y)>(points),
                IsScatter = isScatter
            };
        }

        public ISet<string> OptimizerNames => _optimizerNames;
        public IEnumerable<ItemResult> Items => _items;
        public IEnumerable<SeriesResult> Series => _series;
        public Type InstanceType => _instanceType;
        public FitnessComparison Comparison => FitnessComparison.Maximization; // not used, so arbitrary value is chosen

        public Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public IObservableValue<Individual<InstanceFormat>> Fittest(string optimizerName)
        {
            if (!_fittest.ContainsKey(optimizerName))
            {
                return null;
            }

            return new ObservableValue<Individual<InstanceFormat>>(_fittest[optimizerName]);
        }

        public void SetLock(object newLock)
        {
            _seriesLock = newLock;
        }
    }
}
