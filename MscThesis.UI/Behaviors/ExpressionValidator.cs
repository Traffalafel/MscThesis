using MscThesis.Framework.Factories.Expression;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Behaviors
{
    public class ExpressionValidator : Behavior<Entry>
    {

        private IExpressionFactory _factory;

        public ExpressionValidator()
        {
            _factory = new ExpressionFactory();
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
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
            bool isValid;
            try
            {
                _factory.BuildExpression(text);
                isValid = true;
            }
            catch (Exception)
            {
                isValid = false;
            }
            var entry = (Entry)sender;
            entry.TextColor = isValid ? Colors.White : Colors.Red;
        }

    }
}
