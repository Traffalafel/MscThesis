using System;
using System.ComponentModel;

namespace MscThesis.Runner
{
    public interface IObservableValue<out T> : INotifyPropertyChanged
    {
        public T Value { get; }
        public IObservableValue<string> ToStringObservable();
    }
}
