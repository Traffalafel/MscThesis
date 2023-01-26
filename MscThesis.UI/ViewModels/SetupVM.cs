using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core;
using MscThesis.Framework;
using MscThesis.Framework.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{

    public partial class SetupVM : ObservableObject
    {
        private static readonly string NoneParameter = "None";
        private static readonly string ProblemSizeParameter = "ProblemSize";

        [ObservableProperty]
        bool customProblemSizesAllowed = false;

        [ObservableProperty]
        bool parallelizationEnabled = false;

        [ObservableProperty]
        bool multipleSizes = false;

        [ObservableProperty]
        bool variableSelected = false;

        [ObservableProperty]
        bool specifyProblemSize = false;

        [ObservableProperty]
        string selectedVariable = NoneParameter;

        [ObservableProperty]
        string problemInformationName = string.Empty;

        [ObservableProperty]
        string problemInformationDescription = string.Empty;

        [ObservableProperty]
        bool expressionParamsExist = false;
        [ObservableProperty]
        bool optionParamsExist = false;
        [ObservableProperty]
        bool anyOptimizersExist = false;
        [ObservableProperty]
        bool anyTerminationsExist = false;

        private string _problemName = string.Empty;

        private TestProvider _provider { get; set; }

        public SetupVM(string tspLibPath)
        {
            _provider = new TestProvider(tspLibPath);

            UpdateParameterOptions();
        }

        public string ProblemName {
            get => _problemName;
            set
            {
                _problemName = value;
                
                var optimizerNamesNew = _provider.GetAlgorithmNames(_problemName);
                PossibleAlgorithmNames.Clear();
                foreach (var optimizerName in optimizerNamesNew)
                {
                    PossibleAlgorithmNames.Add(optimizerName);
                }

                var terminationNamesNew = _provider.GetTerminationNames(_problemName);
                PossibleTerminationNames.Clear();
                foreach (var name in terminationNamesNew)
                {
                    PossibleTerminationNames.Add(name);
                }

                try
                {
                    var problemSpec = CreateProblemSpec();
                    var definition = _provider.GetProblemDefinition(problemSpec);

                    CustomProblemSizesAllowed = definition.CustomSizesAllowed;
                    if (!CustomProblemSizesAllowed)
                    {
                        SpecifyProblemSize = false;
                        if (VariableParameterOptions.Contains(ProblemSizeParameter))
                        {
                            VariableParameterOptions.Remove(ProblemSizeParameter);
                        }
                    }
                    else
                    {
                        SpecifyProblemSize = SelectedVariable != ProblemSizeParameter;
                        if (!VariableParameterOptions.Contains(ProblemSizeParameter))
                        {
                            VariableParameterOptions.Add(ProblemSizeParameter);
                        }
                    }

                    var expressionVMs = definition.ExpressionParameters.Select(vm => new ExpressionParameterVM
                    {
                        Name = vm.ToString()
                    });
                    ProblemExpressionParameters.Clear();
                    foreach (var vm in expressionVMs)
                    {
                        ProblemExpressionParameters.Add(vm);
                    }
                    ExpressionParamsExist = expressionVMs.Count() > 0;

                    var optionVMs = definition.OptionParameters.Select(kv =>
                    {
                        return new OptionParameterVM(kv.Key, string.Join(',', kv.Value));
                    });
                    OptionParamsExist = optionVMs.Count() > 0;
                    
                    ProblemOptionParameters.Clear();
                    foreach (var vm in optionVMs)
                    {
                        ProblemOptionParameters.Add(vm);
                    }

                    ReloadProblemInformation();
                    UpdateParameterOptions();
                }
                catch
                {
                    ; // do nothing
                }
            } 
        }

        public string Variable
        {
            get
            {
                return SelectedVariable;
            }
            set 
            {
                SelectedVariable = value;
                VariableSelected = SelectedVariable != NoneParameter;
                SpecifyProblemSize = SelectedVariable != ProblemSizeParameter;
            }
        }

        public void ReloadProblemInformation()
        {
            var spec = CreateProblemSpec();
            var info = _provider.GetProblemInformation(spec);
            ProblemInformationName = info.Name;
            ProblemInformationDescription = info.Description;
        }

        public string NumRuns { get; set; } = "1";
        public string MaxParallelization { get; set; } = "1";
        public string ProblemSize { get; set; }
        public string VariableStart { get; set; }
        public string VariableStop { get; set; }
        public string VariableStep { get; set; }

        public ObservableCollection<ExpressionParameterVM> ProblemExpressionParameters { get; set; } = new();
        public ObservableCollection<OptionParameterVM> ProblemOptionParameters { get; set; } = new();
        public ObservableCollection<string> VariableParameterOptions { get; set; } = new();

        public ObservableCollection<TerminationSetupVM> Terminations { get; set; } = new();
        public ObservableCollection<OptimizerSetupVM> Optimizers { get; set; } = new();

        public List<string> PossibleProblemNames => _provider.GetProblemNames();
        public ObservableCollection<string> PossibleAlgorithmNames { get; set; } = new();
        public ObservableCollection<string> PossibleTerminationNames { get; set; } = new();

        public void UpdateParameterOptions()
        {
            var parameters = new HashSet<string>();
            foreach (var param in ProblemExpressionParameters)
            {
                parameters.Add(param.Parameter.ToString());
            }
            foreach (var optimizer in Optimizers)
            {
                foreach (var param in optimizer.Parameters)
                {
                    parameters.Add(param.Parameter.ToString());
                }
            }
            foreach (var termination in Terminations)
            {
                foreach (var param in termination.Parameters)
                {
                    parameters.Add(param.Parameter.ToString());
                }
            }

            var toRemove = new HashSet<string>();

            foreach (var p in VariableParameterOptions)
            {
                if (!parameters.Contains(p))
                {
                    toRemove.Add(p);
                }
            }
            foreach (var p in toRemove)
            {
                VariableParameterOptions.Remove(p);
            }
            foreach (var p in parameters)
            {
                if (!VariableParameterOptions.Contains(p))
                {
                    VariableParameterOptions.Add(p);
                }
            }
            VariableParameterOptions.Add(NoneParameter);
            VariableParameterOptions.Add(ProblemSizeParameter);
        }

        public void AddOptimizer()
        {
            var newOptimizer = new OptimizerSetupVM(_provider, UpdateParameterOptions);
            Optimizers.Add(newOptimizer);
            AnyOptimizersExist = true;
        }

        public void AddTerminationCriterion()
        {
            var newCriterion = new TerminationSetupVM(_provider, UpdateParameterOptions);
            Terminations.Add(newCriterion);
            AnyTerminationsExist = true;
        }

        private ProblemSpecification CreateProblemSpec()
        {
            return new ProblemSpecification
            {
                Name = _problemName,
                Parameters = Utils.Join(Utils.ToSpecification(ProblemExpressionParameters),
                                        Utils.ToSpecification(ProblemOptionParameters))          
            };
        }

        public TestSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_problemName))
            {
                throw new Exception();
            }

            var canParse = Enum.TryParse(SelectedVariable, out Parameter variable);
            return new TestSpecification
            {
                NumRuns = Convert.ToInt32(NumRuns),
                MaxParallelization = Convert.ToDouble(MaxParallelization),
                Variable = canParse ? variable : null,
                VariableSteps = new StepsSpecification
                {
                    Start = Convert.ToInt32(VariableStart),
                    Stop = VariableStop != string.Empty ? Convert.ToInt32(VariableStop) : null,
                    Step = VariableStep != string.Empty ? Convert.ToInt32(VariableStep) : null
                },
                Problem = CreateProblemSpec(),
                Optimizers = Optimizers.Select(o => o.ToSpecification()).ToList(),
                Terminations = Terminations.Select(t => t.ToSpecification()).ToList()
            };
        }

    }

}
