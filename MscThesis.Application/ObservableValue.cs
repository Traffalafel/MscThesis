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

    }
}
