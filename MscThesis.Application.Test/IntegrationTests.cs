using MscThesis.Runner.Test;
using MscThesis.Runner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using Newtonsoft.Json;
using MscThesis.Runner.Specification;
using System.Configuration;

namespace MscThesis.Application.Test
{
    public class IntegrationTests
    {
        private static readonly string _specificationDirectoryPath = "Specifications";

        public static IEnumerable<object[]> SpecificationFilePaths
        {
            get
            {
                var files = Directory.EnumerateFiles(_specificationDirectoryPath);
                return files.Select(file => new object[] { file });
            }
        }

        [Theory]
        [MemberData(nameof(SpecificationFilePaths))]
        public async void FromSpecification(string specificationFilePath)
        {
            var content = File.ReadAllText(specificationFilePath);
            var spec = JsonConvert.DeserializeObject<TestSpecification>(content);

            var tspLibPath = ConfigurationManager.AppSettings["TSPLibDirectoryPath"];

            var provider = new TestProvider(tspLibPath);
            var test = provider.Build(spec);

            using (var source = new CancellationTokenSource())
            {
                await test.Execute(source.Token);
            }

            foreach (var optimizer in spec.Optimizers)
            {
                Assert.Contains(test.OptimizerNames, name => name == optimizer.Name);
            }
        }
    }
}
