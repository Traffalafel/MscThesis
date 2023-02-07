using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Behaviors
{
    public class NumericValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            OnEntryTextChanged(entry, new TextChangedEventArgs(null, entry.Text));
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var text = args.NewTextValue;
            bool isValid = double.TryParse(text, out var _);
            var entry = (Entry)sender;
            entry.TextColor = isValid ? Colors.White : Colors.Red;
        }
    }
}
