using System.ComponentModel;

namespace MscThesis.Runner
{
    public class ObservableValue<T> : IObservableValue<T>
    {
        private T _value;
        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
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

        public IObservableValue<string> ToStringObservable()
        {
            var newObs = new ObservableValue<string>(Value.ToString());
            PropertyChanged += (s,e) => newObs.Value = Value.ToString();
            return newObs;
        }
    }
}
