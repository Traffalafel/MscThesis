using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class MultipleRunsTest
    {
        [Fact]
        public async void MultipleRunsOneOptimizer()
        {
            var spec = new TestSpecification
            {
                NumRuns = 30,
                ProblemSizes = { 20 },
                MaxParallelization = 1,
                Problem = new ProblemSpecification
                {
                    Name = "OneMax"
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "n"
                        }
                    }
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Stagnation",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.MaxIterations] = "10",
                            [Parameter.Epsilon] = "10E-6"
                        }
                    }
                }
            };

            var provider = new TestProvider(SettingsProvider.Empty);
            var test = provider.Run(spec);

            using var source = new CancellationTokenSource();
            await test.Execute(source.Token);
        }
    }
}
