
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Views;

public partial class ExpressionParameterSetup : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ExpressionParameterSetup), string.Empty);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(ExpressionParameterSetup), string.Empty);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ExpressionParameterSetup()
	{
		InitializeComponent();
	}

}