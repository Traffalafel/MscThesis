using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MscThesis.UI.ViewModels
{

    public static class Utils
    {
        public static IDictionary<Parameter, string> ToSpecification(ObservableCollection<ParameterVM> vms)
        {
            try
            {
                return vms
                    .GroupBy(vm => vm.Parameter)
                    .ToDictionary(x => x.Key, x => x.First().Value);
            }
            catch (Exception e)
            {
                return new Dictionary<Parameter, string>();
            }
        }
    }

    public class ParameterVM
    {
        public string Name { get; set; }
        public Parameter Parameter
        {
            get
            {
                Enum.TryParse(Name, out Parameter param);
                return param;
            }
        }
        public string Value { get; set; } = string.Empty;
    }

    public class TerminationSetupVM
    {
        private string _name = string.Empty;
        private TestProvider _runner;

        public TerminationSetupVM(TestProvider runner)
        {
            _runner = runner;
        }

        public ObservableCollection<ParameterVM> Parameters { get; set; } = new();
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                var paramsNew = _runner.GetTerminationParameters(value);
                var vms = paramsNew.Select(p => new ParameterVM
                {
                    Name = p.ToString()
                });
                Parameters.Clear();
                foreach (var vm in vms)
                {
                    Parameters.Add(vm);
                }
            }
        }

        public TerminationSpecification ToSpecification()
        {
            return new TerminationSpecification
            {
                Name = Name,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }
    }

    public class OptimizerSetupVM
    {
        private TestProvider _runner;
        private string _algorithm = string.Empty;

        public OptimizerSetupVM(TestProvider runner)
        {
            _runner = runner;
        }

        public string Seed { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ParameterVM> Parameters { get; set; } = new();
        public string Algorithm
        {
            get => _algorithm;
            set
            {
                _algorithm = value;
                var paramsNew = _runner.GetAlgorithmParameters(value);
                var vms = paramsNew.Select(p => new ParameterVM
                {
                    Name = p.ToString()
                });
                Parameters.Clear();
                foreach (var vm in vms)
                {
                    Parameters.Add(vm);
                }
            }
        }

        public OptimizerSpecification ToSpecification()
        {
            return new OptimizerSpecification
            {
                Seed = string.IsNullOrWhiteSpace(Seed) ? null : Convert.ToInt32(Seed),
                Name = Name,
                Algorithm = _algorithm,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }

    }

    public class SetupVM
    {
        private string _problemName = string.Empty;

        private TestProvider _runner { get; set; } = new TestProvider();

        public string ProblemName {
            get => _problemName;
            set
            {
                _problemName = value;
                
                var optimizerNamesNew = _runner.GetAlgorithmNames(_problemName);
                PossibleAlgorithmNames.Clear();
                foreach (var optimizerName in optimizerNamesNew)
                {
                    PossibleAlgorithmNames.Add(optimizerName);
                }

                var terminationNamesNew = _runner.GetTerminationNames(_problemName);
                PossibleTerminationNames.Clear();
                foreach (var name in terminationNamesNew)
                {
                    PossibleTerminationNames.Add(name);
                }

                var newParameters = _runner.GetProblemParameters(_problemName);
                var vms = newParameters.Select(vm => new ParameterVM
                {
                    Name = vm.ToString()
                });
                ProblemParameters.Clear();
                foreach (var vm in vms)
                {
                    ProblemParameters.Add(vm);
                }
            } 
        }

        public string NumRuns { get; set; } = "1";

        public string ProblemSizeStart { get; set; }
        public string ProblemSizeStop { get; set; }
        public string ProblemSizeStep { get; set; }

        public string MaxParallelization { get; set; }
        public ObservableCollection<ParameterVM> ProblemParameters { get; set; } = new();
        public ObservableCollection<TerminationSetupVM> Terminations { get; set; } = new();
        public ObservableCollection<OptimizerSetupVM> Optimizers { get; set; } = new();

        public List<string> PossibleProblemNames => _runner.GetProblemNames();
        public ObservableCollection<string> PossibleAlgorithmNames { get; set; } = new();
        public ObservableCollection<string> PossibleTerminationNames { get; set; } = new();

        public void AddOptimizer()
        {
            var newOptimizer = new OptimizerSetupVM(_runner);
            Optimizers.Add(newOptimizer);
        }

        public void AddTerminationCriterion()
        {
            var newCriterion = new TerminationSetupVM(_runner);
            Terminations.Add(newCriterion);
        }

        public TestSpecification ToSpecification()
        {
            return new TestSpecification
            {
                NumRuns = Convert.ToInt32(NumRuns),
                Optimizers = Optimizers.Select(o => o.ToSpecification()).ToList(),
                MaxParallelization = Convert.ToDouble(MaxParallelization),
                Problem = new ProblemSpecification
                {
                    Name = _problemName,
                    Parameters = Utils.ToSpecification(ProblemParameters)
                },
                ProblemSizes = ParseProblemSizes(ProblemSizeStart, ProblemSizeStop, ProblemSizeStep).ToList(),
                Terminations = Terminations.Select(t => t.ToSpecification()).ToList()
            };
        }

        private IEnumerable<int> ParseProblemSizes(string startStr, string stopStr, string stepStr)
        {
            var start = Convert.ToInt32(startStr);
            var stop = Convert.ToInt32(stopStr);
            var step = Convert.ToInt32(stepStr);

            if (start <= 0 || stop <= 0)
            {
                throw new Exception("Start and stop must both be strictly positive.");
            }
            if (stop < start)
            {
                throw new Exception("Start must be lower than stop.");
            }
            if (step < 0)
            {
                throw new Exception("Cannot have negative step size.");
            }
            if (step == 0 && start != stop)
            {
                throw new Exception("Start and stop must be equal for step size zero.");
            }

            var c = start;
            do
            {
                yield return c;
                c += step;
            }
            while (c < stop);
        }

    }

}
