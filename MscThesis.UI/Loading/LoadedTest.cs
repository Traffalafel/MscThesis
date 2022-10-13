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
        private HashSet<string> _optimizerNames;
        private List<ItemResult> _items;
        private List<SeriesResult> _series;

        public bool IsTerminated => true;

        public LoadedTest(string content)
        {
            _optimizerNames = new();
            _items = new();
            _series = new();

            content = content.Replace(" ", "");
            content = content.Replace("\r", "");
            var lines = content.Split('\n');

            var c = 0;
            var line = lines[c++];

            if (line != "Items:")
            {
                throw new Exception();
            }
            
            while (lines[c] != "Series:")
            {
                // Parse items
                try
                {
                    line = lines[c++];
                    var item = ParseItemLine(line);
                    _optimizerNames.Add(item.OptimizerName);
                    _items.Add(item);
                }
                catch
                {
                    throw new Exception();
                }
            }

            c++;
            while (c < lines.Length)
            {
                // Parse series
                try
                {
                    line = lines[c++];
                    var series = ParseSeriesLine(line);
                    _optimizerNames.Add(series.OptimizerName);
                    _series.Add(series);
                }
                catch
                {
                    throw new Exception();
                }
            }

        }

        private ItemResult ParseItemLine(string line)
        {
            var split = line.Split('\t');
            var def = split[0];
            var valueStr = split[1];
            var value = Convert.ToDouble(valueStr, CultureInfo.InvariantCulture);

            var defSplit = def.Split('.');
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

            var defSplit = def.Split('.');
            var optimizerName = defSplit[0];
            var yPropertyName = defSplit[1];
            var xPropertyName = defSplit[2];
            _ = Enum.TryParse(yPropertyName, out Property yProperty);
            _ = Enum.TryParse(xPropertyName, out Property xProperty);

            return new SeriesResult
            {
                OptimizerName = optimizerName,
                Property = yProperty,
                XAxis = xProperty,
                Points = new ObservableCollection<(double x, double y)>(points)
            };
        }

        public ISet<string> OptimizerNames => _optimizerNames;

        public IObservableValue<Individual<InstanceFormat>> Fittest => new ObservableValue<Individual<InstanceFormat>>();

        public IEnumerable<ItemResult> Items => _items;

        public IEnumerable<SeriesResult> Series => _series;

        public IEnumerable<HistogramResult> Histograms => throw new NotImplementedException();

        public Task Execute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
