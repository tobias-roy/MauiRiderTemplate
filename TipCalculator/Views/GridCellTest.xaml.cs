using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipCalculator.Views;

public partial class GridCellTest : ContentPage
{
    public GridCellTest()
    {
        InitializeComponent();
    }
    
    public async void NavigateToAboutPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new About());
    }
}