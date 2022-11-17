using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MscThesis.Runner
{
    public class ResultExporter
    {
        public static string Export(ITest<InstanceFormat> test, TestSpecification specification)
        {
            var lines = new List<string>();

            lines.Add("Specification:");
            var specStr = JsonConvert.SerializeObject(specification, Formatting.Indented);
            lines.Add(specStr);

            // Export type
            lines.Add("Type:");
            lines.Add(test.InstanceType.Name);

            // Export fittest
            lines.Add("Fittest:");
            foreach (var optimizerName in test.OptimizerNames)
            {
                var observable = test.Fittest(optimizerName);
                InstanceFormat value;
                double fitness;
                if (observable.Value != null)
                {
                    value = observable.Value.Value;
                    fitness = observable.Value.Fitness.Value;
                }
                else
                {
                    value = new BitString();
                    fitness = 0;
                }
                lines.Add($"{optimizerName};{value};{fitness}");
            }

            // Export items
            lines.Add("Items:");
            foreach (var item in test.Items)
            {
                var value = item.Observable.Value;
                var s = $"{item.OptimizerName};{item.Property}\t{value.ToString(CultureInfo.InvariantCulture)}";
                lines.Add(s);
            }

            // Export series
            lines.Add("Series:");
            foreach (var series in test.Series)
            {
                var scatterStr = series.IsScatter ? ";Scatter" : string.Empty;
                var values = string.Join(';', series.Points.Select(p => $"({p.x.ToString(CultureInfo.InvariantCulture)},{p.y.ToString(CultureInfo.InvariantCulture)})"));
                var s = $"{series.OptimizerName};{series.Property};{series.XAxis}{scatterStr}\t{values}";
                lines.Add(s);
            }

            return string.Join('\n', lines);
        }
    }
}
