using System;
using System.ComponentModel;

namespace MscThesis.Runner
{
    public interface IObservableValue<out T>
    {
        public T Value { get; }
        public void Subscribe(Action<T> observer);
    }
}
