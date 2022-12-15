using MscThesis.Core;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Results
{
    public abstract class Test<T> : ITest<T> where T : InstanceFormat
    {
        private object _lock = new object { };

        protected bool _isTerminated;
        protected Dictionary<string, ObservableValue<Individual<T>>> _fittest = new Dictionary<string, ObservableValue<Individual<T>>>();
        protected Type _instanceType = typeof(InstanceFormat);
        protected FitnessComparison _comparisonStrategy;

        public void Initialize(IEnumerable<string> optimizerNames)
        {
            foreach (var optimizerName in optimizerNames)
            {
                if (!_fittest.ContainsKey(optimizerName))
                {
                    _fittest.Add(optimizerName, new ObservableValue<Individual<T>>());
                }
            }
        }

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

        public IObservableValue<Individual<T>> Fittest(string optimizerName)
        {
            if (!_fittest.ContainsKey(optimizerName))
            {
                return null;
            }
            else
            {
                return _fittest[optimizerName];
            }
        }

        protected void TryUpdateFittest(ITest<T> test)
        {
            foreach (var optimizerName in test.OptimizerNames)
            {
                var other = test.Fittest(optimizerName)?.Value;
                TryUpdateFittest(optimizerName, other);
            }
        }

        protected void TriggerOptimizerDone()
        {
            OptimizerDone.Invoke(this, new EventArgs());
        }

        protected void TryUpdateFittest(string optimizerName, Individual<T> other)
        {
            if (!_fittest.ContainsKey(optimizerName))
            {
                return;
            }

            // Overwrite fittest if better
            if (_comparisonStrategy.IsFitter(other, _fittest[optimizerName].Value))
            {
                _fittest[optimizerName].Value = other;
            }
        }

        public abstract Task Execute(CancellationToken cancellationToken);
    }
}
