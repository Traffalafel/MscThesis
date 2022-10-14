using MscThesis.Core;

namespace MscThesis.UI.ViewModels
{
    public class ExpressionParameterVM
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

}
