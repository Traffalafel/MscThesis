namespace MscThesis.UI.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public async void NavigateNewRun(object sender, EventArgs e)
	{
		var task = Shell.Current.GoToAsync(nameof(SetupPage));
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			;
		}
	}

	public async void NavigateLoadResults(object sender, EventArgs e)
	{
        var fileResult = await FilePicker.Default.PickAsync();

		if (fileResult == null)
		{
			return;
		}

        await Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
        {
            ["ResultsFilePath"] = fileResult.FullPath
        });
    }

}