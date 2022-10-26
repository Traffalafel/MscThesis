using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class TourMIMICTests
    {
        [Fact]
        public async void SingleRun()
        {
            var spec = new TestSpecification
            {
                NumRuns = 1,
                MaxParallelization = 1,
                Problem = new ProblemSpecification
                {
                    Name = "TSPLib",
                    Parameters = new Dictionary<Parameter, string>
                    {
                        [Parameter.ProblemName] = "burma14"
                    }
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "TourMIMIC",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "10*n"
                        }
                    },
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Max iterations",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.MaxIterations] = "100"
                        }
                    }
                }
            };

            var provider = new TestProvider(SettingsProvider.Default);
            var test = provider.Build(spec);

            using (var source = new CancellationTokenSource())
            {
                await test.Execute(source.Token);
            }
        }

    }
}
