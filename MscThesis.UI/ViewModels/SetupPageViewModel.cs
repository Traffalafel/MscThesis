using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MscThesis.UI.ViewModels
{

    public static class Utils
    {
        public static IDictionary<Parameter, double> ToSpecification(ObservableCollection<ParameterVM> vms)
        {
            try
            {
                return vms
                    .GroupBy(vm => vm.Parameter)
                    .ToDictionary(x => x.Key, x => double.Parse(x.First().Value, CultureInfo.InvariantCulture));
            }
            catch (Exception e)
            {
                return new Dictionary<Parameter, double>();
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
        private TestRunner _runner;

        public TerminationSetupVM(TestRunner runner)
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
        private TestRunner _runner;
        private string _algorithm = string.Empty;

        public OptimizerSetupVM(TestRunner runner)
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

        private TestRunner _runner { get; set; } = new TestRunner();

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
        public string ProblemSize { get; set; }
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
                Problem = new ProblemSpecification
                {
                    Name = _problemName,
                    Parameters = Utils.ToSpecification(ProblemParameters)
                },
                ProblemSizes = new List<int> { Convert.ToInt32(ProblemSize) },
                Terminations = Terminations.Select(t => t.ToSpecification()).ToList()
            };
        }

    }

}
