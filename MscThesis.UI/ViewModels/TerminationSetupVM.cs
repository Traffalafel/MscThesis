﻿using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public partial class TerminationSetupVM : ObservableObject
    {

        [ObservableProperty]
        bool parametersExist = false;

        private string _name = string.Empty;
        private TestProvider _runner;
        private Action _algorithmChanged;

        public TerminationSetupVM(TestProvider runner, Action algorithmChanged)
        {
            _runner = runner;
            _algorithmChanged = algorithmChanged;
        }

        public ObservableCollection<ExpressionParameterVM> Parameters { get; set; } = new();
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                var paramsNew = _runner.GetTerminationParameters(value);
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
