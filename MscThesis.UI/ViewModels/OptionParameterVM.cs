using MscThesis.Core;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public class OptionParameterVM
    {
        private Parameter _parameter;
        private string _optionsStr;

        public string Value { get; set; } = string.Empty;
        public string Name
        {
            get => _parameter.ToString();
        }
        public Parameter Parameter
        {
            get => _parameter;
        }
        public string Options => _optionsStr;

        public OptionParameterVM(Parameter parameter, string optionsStr)
        {
            _parameter = parameter;
            _optionsStr = optionsStr;
        }
    }
}