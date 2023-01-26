using MscThesis.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using Newtonsoft.Json;
using MscThesis.Framework.Specification;
using Newtonsoft.Json.Linq;

namespace MscThesis.Framework.Test
{
    public class IntegrationTests
    {
        private static readonly string _specificationDirectoryPath = "Specifications";
        private static readonly string _appsettingsPath = "appsettings.json";

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

            var settingsContent = File.ReadAllText(_appsettingsPath);
            var settingsJson = JObject.Parse(settingsContent);

            var tspLibPath = settingsJson["TSPLibDirectoryPath"].Value<string>();

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
