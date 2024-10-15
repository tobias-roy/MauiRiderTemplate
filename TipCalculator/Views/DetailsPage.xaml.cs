namespace TipCalculator.Views;

[QueryProperty(nameof(Name), "name")]
public partial class DetailsPage : ContentPage
{
    private string name;

    public DetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
            NameLabel.Text = value;
        }
    }
}