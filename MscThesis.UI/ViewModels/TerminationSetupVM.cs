using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
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
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new Exception();
            }
            return new TerminationSpecification
            {
                Name = Name,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }
    }

}
