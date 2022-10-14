
using MscThesis.UI.ViewModels;
using System.Collections.ObjectModel;

namespace MscThesis.UI.Views;

public partial class OptionParameterSetup : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(OptionParameterSetup), string.Empty);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(OptionParameterSetup), string.Empty);
    public static readonly BindableProperty OptionsProperty = BindableProperty.Create(nameof(Options), typeof(string), typeof(OptionParameterSetup), string.Empty);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set {
            SetValue(ValueProperty, value);
            try
            {
                var setupVm = (SetupVM)Parent.Parent.Parent.Parent.BindingContext;
                setupVm.ReloadProblemInformation();
            }
            catch (Exception e)
            {
                ;
            }
        }
    }

    public string OptionsStr
    {
        get => (string)GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public List<string> Options => OptionsStr.Split(',').ToList();

    public OptionParameterSetup()
	{
		InitializeComponent();
	}

}