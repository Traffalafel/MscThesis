using MscThesis.Core;
using MscThesis.Runner.Factories.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories.Parameters
{
    public class ParameterFactory
    {
        private IExpressionFactory _expressionFactory;

        public ParameterFactory(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Func<Parameter, int, VariableSpecification, double> BuildParameters(IDictionary<Parameter, string> spec)
        {
            return (parameter, n, varSpec) =>
            {
                if (varSpec != null && varSpec.Variable == parameter)
                {
                    return varSpec.Value;
                }

                if (!spec.ContainsKey(parameter))
                {
                    throw new Exception($"Parameter {parameter} not found in specification");
                }

                var expressionStr = spec[parameter];
                var expression = _expressionFactory.BuildExpression(expressionStr);
                return expression.Compute(n);
            };
        }
    }
}
