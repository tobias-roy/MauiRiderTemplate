using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipCalculator;

[QueryProperty(nameof(Name), "name")]
public partial class DetailsPage : ContentPage
{
    private String name;
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
    
    public DetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}