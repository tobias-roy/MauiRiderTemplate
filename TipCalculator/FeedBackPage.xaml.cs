using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipCalculator;

public partial class FeedBackPage : ContentPage
{
    public FeedBackPage()
    {
        InitializeComponent();
        GoToDetailsPageBtn.Clicked += GoToDetailsPageBtn_Clicked;
    }
    
    private async void GoToDetailsPageBtn_Clicked(object sender, EventArgs e)
    {
        string myName = NameEntry.Text;
        if (myName == String.Empty || myName == null || myName.Length == 0)
        {
            await DisplayAlert("Error", "Please enter your name", "OK");
            return;
        }
        await Shell.Current.GoToAsync($"/detailspage?name={myName}");
    }
}