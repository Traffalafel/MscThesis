using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface ITestFactory<out T> where T : InstanceFormat
    {
        public ISet<string> Algorithms { get; }
        public ISet<string> Problems { get; }
        public ISet<string> Terminations { get; }

        public abstract IEnumerable<Test<InstanceFormat>> BuildTests(TestSpecification spec);
    }
}
