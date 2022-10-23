﻿using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MscThesis.UI.Loading
{
    public class ResultExporter
    {
        public static string Export(ITest<InstanceFormat> test, TestSpecification specification)
        {
            var lines = new List<string>();

            lines.Add("Specification:");
            var specStr = JsonConvert.SerializeObject(specification, Formatting.Indented);
            lines.Add(specStr);

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
                var values = string.Join(';', series.Points.Select(p => $"({p.x.ToString(CultureInfo.InvariantCulture)},{p.y.ToString(CultureInfo.InvariantCulture)})"));
                var s = $"{series.OptimizerName};{series.Property};{series.XAxis}\t{values}";
                lines.Add(s);
            }

            return string.Join('\n', lines);
        }
    }
}
