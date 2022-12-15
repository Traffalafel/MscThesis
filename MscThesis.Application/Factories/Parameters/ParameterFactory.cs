using MscThesis.Core;
using MscThesis.Runner.Factories.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories.Parameters
{
    public class ParameterFactory : IParameterFactory
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
                if (varSpec != null)
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
