using MscThesis.Core;
using MscThesis.Framework.Factories.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Framework.Factories.Parameters
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
            var expressions = spec.ToDictionary(kv => kv.Key, kv => _expressionFactory.BuildExpression(kv.Value));

            return (parameter, n, varSpec) =>
            {
                if (varSpec != null && varSpec.Variable == parameter)
                {
                    return varSpec.Value;
                }

                if (!expressions.ContainsKey(parameter))
                {
                    throw new Exception($"Parameter {parameter} not found in specification");
                }
                return expressions[parameter].Compute(n);
            };
        }
    }
}
