using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Behaviors
{
    public class NameValidator : Behavior<Picker>
    {
        public static SetupVM VM { get; set; }

        protected override void OnAttachedTo(Picker picker)
        {
            picker.SelectedIndexChanged += OnEntryTextChanged;
            OnEntryTextChanged(picker, EventArgs.Empty);
            base.OnAttachedTo(picker);
        }

        protected override void OnDetachingFrom(Picker picker)
        {
            picker.SelectedIndexChanged -= OnEntryTextChanged;
            VM.InputValid(picker);
            base.OnDetachingFrom(picker);
        }

        void OnEntryTextChanged(object sender, EventArgs args)
        {
            var picker = (Picker)sender;
            var name = picker.SelectedItem?.ToString();
            bool isValid = !string.IsNullOrWhiteSpace(name);
            picker.TextColor = isValid ? Colors.White : Colors.Red;

            if (isValid)
            {
                VM.InputValid(picker);
            }
            else
            {
                VM.InputInvalid(picker);
            }
        }
    }
}
