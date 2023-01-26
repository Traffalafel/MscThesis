using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Framework.Tests
{
    public abstract class Test : ITest
    {
        private object _lock = new object { };

        protected bool _isTerminated;
        protected Type _instanceType = typeof(InstanceFormat);
        protected FitnessComparison _comparisonStrategy;
        protected Dictionary<string, ObservableValue<double?>> _bestFitness = new Dictionary<string, ObservableValue<double?>>();

        public object SeriesLock => _lock;
        public Type InstanceType => _instanceType;
        public bool IsTerminated => _isTerminated;
        public FitnessComparison Comparison => _comparisonStrategy;
        public abstract ISet<string> OptimizerNames { get; }
        public abstract IEnumerable<ItemResult> Items {get;}
        public abstract IEnumerable<SeriesResult> Series {get;}
        public event EventHandler<EventArgs> OptimizerDone;

        public double VariableValue { get; protected set; }

        public void SetLock(object newLock)
        {
            _lock = newLock;
        }

        public IObservableValue<double?> BestFitness(string optimizerName)
        {
            if (!_bestFitness.ContainsKey(optimizerName))
            {
                return null;
            }
            else
            {
                return _bestFitness[optimizerName];
            }
        }

        public void Initialize(IEnumerable<string> optimizerNames)
        {
            foreach (var optimizerName in optimizerNames)
            {
                if (!_bestFitness.ContainsKey(optimizerName))
                {
                    _bestFitness.Add(optimizerName, new ObservableValue<double?>());
                }
            }
        }

        protected void TriggerOptimizerDone()
        {
            OptimizerDone.Invoke(this, new EventArgs());
        }
        
        protected void TryUpdateFittest(ITest test)
        {
            foreach (var optimizerName in test.OptimizerNames)
            {
                var other = test.BestFitness(optimizerName).Value;
                TryUpdateFittest(optimizerName, other);
            }
        }

        protected void TryUpdateFittest(string optimizerName, double? other)
        {
            if (!_bestFitness.ContainsKey(optimizerName))
            {
                return;
            }

            if (other == null)
            {
                return;
            }
            if (_bestFitness[optimizerName].Value == null)
            {
                _bestFitness[optimizerName].Value = other.Value;
                return;
            }

            // Overwrite fittest if better
            if (_comparisonStrategy.IsFitter(other.Value, _bestFitness[optimizerName].Value.Value))
            {
                _bestFitness[optimizerName].Value = other;
            }
        }

        public abstract Task Execute(CancellationToken cancellationToken);
    }
}
