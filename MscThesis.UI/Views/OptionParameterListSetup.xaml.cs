using MscThesis.UI.ViewModels;
using System.Collections.ObjectModel;

namespace MscThesis.UI.Views;

public partial class OptionParameterListSetup : ContentView
{
    public static readonly BindableProperty ParametersProperty = BindableProperty.Create(nameof(Parameters), typeof(ObservableCollection<OptionParameterVM>), typeof(OptionParameterListSetup), new ObservableCollection<OptionParameterVM>());

    public ObservableCollection<OptionParameterVM> Parameters
    {
        get => (ObservableCollection<OptionParameterVM>)GetValue(ParametersProperty);
        set => SetValue(ParametersProperty, value);
    }

    public OptionParameterListSetup()
    {
        InitializeComponent();
    }
}