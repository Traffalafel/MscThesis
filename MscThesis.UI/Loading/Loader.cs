using MscThesis.Core.Formats;
using MscThesis.Runner;
using MscThesis.Runner.Tests;
using MscThesis.Runner.Specification;
using MscThesis.UI.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace MscThesis.UI.Loading
{
    public class Loader
    {
        private TestSpecification _specification;
        private LoadedTest _test;

        public TestSpecification Specification => _specification;
        public ITest Test => _test;

        public Loader(string content, TestProvider provider)
        {
            content = content.Replace("\r", "");

            var lines = content.Split('\n');

            var c = 0;

            if (lines[c++] != "Specification:")
            {
                throw new Exception();
            }

            var jsonStr = string.Empty;
            while (lines[c] != "Type:")
            {
                // Parse items
                try
                {
                    var line = lines[c++];
                    jsonStr += line;
                }
                catch
                {
                    throw new Exception();
                }
            }

            try
            {
                _specification = JsonConvert.DeserializeObject<TestSpecification>(jsonStr);
            }
            catch (Exception e)
            {
                ;
            }

            var resultsContent = content.Replace(" ", "")
                                        .Split('\n')
                                        .Skip(c)
                                        .ToList();
            _test = new LoadedTest(resultsContent);
        }
    }
}
