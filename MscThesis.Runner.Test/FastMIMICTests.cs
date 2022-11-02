using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class FastMIMICTests
    {
        [Fact]
        public async void SingleRun()
        {
            var spec = new TestSpecification
            {
                NumRuns = 1,
                ProblemSizes = { 100 },
                MaxParallelization = 1,
                Problem = new ProblemSpecification
                {
                    Name = "OneMax"
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "FastMIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "n",
                            [Parameter.NumSampledPositions] = "sqrt(n)"
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

            var provider = new TestProvider(SettingsProvider.Default);
            var test = provider.Build(spec);

            using (var source = new CancellationTokenSource())
            {
                await test.Execute(source.Token);
            }
        }

        [Fact]
        public async void MultipleRuns()
        {
            var spec = new TestSpecification
            {
                NumRuns = 20,
                ProblemSizes = new List<int>
                {
                    100
                },
                Problem = new ProblemSpecification
                {
                    Name = "JumpOffsetSpike",
                    Parameters = new Dictionary<Parameter, string>
                    {
                        { Parameter.GapSize, "0.1*n" }
                    }
                },
                MaxParallelization = 20,
                Optimizers = new List<OptimizerSpecification>()
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "0.5*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "1*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "2*n" }
                        },
                    },
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Stagnation",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.MaxIterations, "10" },
                            { Parameter.Epsilon, "10E-6" }
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
