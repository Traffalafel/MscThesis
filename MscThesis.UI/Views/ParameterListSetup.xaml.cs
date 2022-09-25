using MscThesis.UI.ViewModels;
using System.Collections.ObjectModel;

namespace MscThesis.UI.Views;

public partial class ParameterListSetup : ContentView
{
    public static readonly BindableProperty ParametersProperty = BindableProperty.Create(nameof(Parameters), typeof(ObservableCollection<ParameterVM>), typeof(ParameterListSetup), new ObservableCollection<ParameterVM>());

    public ObservableCollection<ParameterVM> Parameters
    {
        get => (ObservableCollection<ParameterVM>)GetValue(ParametersProperty);
        set => SetValue(ParametersProperty, value);
    }

    public ParameterListSetup()
	{
		InitializeComponent();
	}
}