namespace TipCalculator.Views;

public partial class FeedBackPage : ContentPage
{
    public FeedBackPage()
    {
        InitializeComponent();
        GoToDetailsPageBtn.Clicked += GoToDetailsPageBtn_Clicked;
    }

    private async void GoToDetailsPageBtn_Clicked(object sender, EventArgs e)
    {
        var myName = NameEntry.Text;
        if (myName == string.Empty || myName == null || myName.Length == 0)
        {
            await DisplayAlert("Error", "Please enter your name", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"/detailspage?name={myName}");
    }
}