using MscThesis.Core;
using MscThesis.Runner.Factories;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Tests;
using MscThesis.Runner.Specification;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;

namespace MscThesis.Runner
{
    public class TestProvider
    {
        private List<ITestFactory> _factories;

        public TestProvider(string tspLibPath)
        {
            var tspLib = new TspLib95(tspLibPath);

            _factories = new List<ITestFactory>
            {
                new BitStringTestFactory(),
                new TourTestFactory(tspLib)
            };
        }

        public ITest Build(TestSpecification spec)
        {
            var variable = spec.Variable ?? Parameter.ProblemSize;
            var problemName = spec.Problem.Name;
            var factory = GetTestFactory(problemName);

            var problemDef = factory.GetProblemDefinition(spec.Problem);
            if (!problemDef.CustomSizesAllowed)
            {
                spec.ProblemSize = problemDef.ProblemSize;
                if (variable == Parameter.ProblemSize)
                {
                    if (spec.VariableSteps == null)
                    {
                        spec.VariableSteps = new StepsSpecification
                        {
                            Start = problemDef.ProblemSize.Value
                        };
                    }
                    else
                    {
                        spec.VariableSteps.Start = problemDef.ProblemSize.Value;
                    }
                }
            }

            return factory.BuildTest(spec);
        }

        public List<string> GetProblemNames()
        {
            var names = new List<string>();
            foreach (var factory in _factories)
            {
                names.AddRange(factory.Problems);
            }
            return names;
        }

        public ProblemDefinition GetProblemDefinition(ProblemSpecification spec)
        {
            var problemName = spec.Name;
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory.GetProblemDefinition(spec);
                }
            }
            return new ProblemDefinition
            {
                CustomSizesAllowed = false,
                ExpressionParameters = new List<Parameter>(),
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
            };
        }

        public ProblemInformation GetProblemInformation(ProblemSpecification spec)
        {
            try
            {
                var factory = GetTestFactory(spec.Name);
                return factory.GetProblemInformation(spec);
            }
            catch
            {
                return new ProblemInformation();
            }
        }

        public List<string> GetTerminationNames(string problemName)
        {
            var factory = GetTestFactory(problemName);
            if (factory != null)
            {
                return factory.Terminations.ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetAlgorithmNames(string problemName)
        {
            var factory = GetTestFactory(problemName);
            if (factory != null)
            {
                return factory.Algorithms.ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Algorithms.Contains(algorithmName))
                {
                    return factory.GetAlgorithmParameters(algorithmName);
                }
            }
            return new List<Parameter>();
        }

        public IEnumerable<Parameter> GetTerminationParameters(string terminationName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Terminations.Contains(terminationName))
                {
                    return factory.GetTerminationParameters(terminationName);
                }
            }
            return new List<Parameter>();
        }

        private ITestFactory GetTestFactory(string problemName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory;
                }
            }
            return null;
        }

    }

}
