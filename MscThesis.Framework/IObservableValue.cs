using System;
using System.ComponentModel;

namespace MscThesis.Framework
{
    public interface IObservableValue<out T> : INotifyPropertyChanged
    {
        public T Value { get; }
        public IObservableValue<string> ToStringObservable();
    }
}
