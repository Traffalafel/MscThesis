using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public partial class OptimizerSetupVM : ObservableObject
    {

        [ObservableProperty]
        bool parametersExist = false;

        private TestProvider _runner;
        private Action _algorithmChanged;
        private string _algorithm = string.Empty;

        public OptimizerSetupVM(TestProvider runner, Action algorithmChanged)
        {
            _runner = runner;
            _algorithmChanged = algorithmChanged;
        }

        public string Name { get; set; }
        public ObservableCollection<ExpressionParameterVM> Parameters { get; set; } = new();
        public string Algorithm
        {
            get => _algorithm;
            set
            {
                _algorithm = value;
                var paramsNew = _runner.GetAlgorithmParameters(value);
                var vms = paramsNew.Select(p => new ExpressionParameterVM
                {
                    Name = p.ToString()
                });
                Parameters.Clear();
                foreach (var vm in vms)
                {
                    Parameters.Add(vm);
                }
                ParametersExist = paramsNew.Count() > 0;
                _algorithmChanged.Invoke();
            }
        }

        private string GetName()
        {
            var name = Algorithm;
            if (Parameters.Any())
            {
                var paramStrings = Parameters.Select(vm => $"{vm.Name}:{vm.Value}");
                name += $"_{string.Join('_', paramStrings)}";
            }
            return name;
        }

        public OptimizerSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_algorithm))
            {
                throw new Exception();
            }
            return new OptimizerSpecification
            {
                Name = GetName(),
                Algorithm = _algorithm,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }

    }

}
