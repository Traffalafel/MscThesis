using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories
{
    public abstract class TestFactory<T> : ITestFactory<T> where T : InstanceFormat
    {
        public ISet<string> Optimizers => _optimizers.Keys.ToHashSet();
        public ISet<string> Problems => _problems.Keys.ToHashSet();
        public ISet<string> Terminations => _terminations.Keys.ToHashSet();

        protected Dictionary<string, OptimizerFactory<T>> _optimizers;
        protected Dictionary<string, ProblemFactory<T>> _problems;
        protected Dictionary<string, TerminationFactory<T>> _terminations;

        public IEnumerable<Test<InstanceFormat>> BuildTests(TestSpecification spec)
        {
            foreach (var optimizerSpec in spec.Optimizers)
            {
                var optimizerFactory = GetOptimizerFactory(optimizerSpec);
                var optimizer = optimizerFactory.BuildOptimizer(optimizerSpec);

                var problemFactory = GetProblemFactory(spec.Problem);
                var problem = problemFactory.BuildProblem(spec.Problem);

                var terminations = new List<TerminationCriterion<T>>();
                foreach (var terminationSpec in optimizerSpec.TerminationCriteria)
                {
                    var terminationFactory = GetTerminationFactory(terminationSpec);
                    var termination = terminationFactory.BuildCriterion(terminationSpec);
                    terminations.Add(termination);
                }

                var name = optimizerSpec.Name;
                yield return new TestImpl<T>(name, optimizer, problem, terminations); ;
            }
        }

        protected OptimizerFactory<T> GetOptimizerFactory(OptimizerSpecification spec)
        {
            return _optimizers[spec.Algorithm];
        }

        protected ProblemFactory<T> GetProblemFactory(ProblemSpecification spec)
        {
            return _problems[spec.Name];
        }

        protected TerminationFactory<T> GetTerminationFactory(TerminationSpecification spec)
        {
            return _terminations[spec.Name];
        }
    }
}
