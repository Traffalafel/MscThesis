using System;
using System.Collections.Generic;

namespace MscThesis.Runner
{
    public class ObservableValue<T> : IObservableValue<T>
    {
        private T _value;
        private List<Action<T>> _observers = new List<Action<T>>();

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                foreach (var observer in _observers)
                {
                    observer.Invoke(_value);
                }
            }
        }

        public ObservableValue()
        {
            _value = default;
        }
        public ObservableValue(T value)
        {
            _value = value;
        }

        public void Subscribe(Action<T> observer)
        {
            _observers.Add(observer);
        }

    }
}
