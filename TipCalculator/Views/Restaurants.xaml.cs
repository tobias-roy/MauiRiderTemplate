using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TipCalculator.Models;

namespace TipCalculator.Views;

public partial class Restaurants : ContentPage
{
    public ObservableCollection<Restaurant> RestraurantsList { get; set; }
    
    public Restaurants()
    {
        InitializeComponent();
        CreateRestaurants();
        BindingContext = this;
    }
    
    void CreateRestaurants()
    {
        RestraurantsList = new ObservableCollection<Restaurant>();
        RestraurantsList.Add(new Restaurant { Name = "Joe's Pizza", TipPct = "15%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/02/Enjoy-5-authentic-wine-samples-of-the-region-on-the-Taormina-Wine-and-Food-Tasting-Tour_48.jpg" });
        RestraurantsList.Add(new Restaurant { Name = "Luigi's Pasta", TipPct = "18%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/piedmont-old-town-restaurant-scaled.jpg" });
        RestraurantsList.Add(new Restaurant { Name = "Maria's Bistro", TipPct = "20%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_789017482.jpg" });
        RestraurantsList.Add(new Restaurant { Name = "Mario's Pizzeria", TipPct = "15%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_1054754711.jpg" });
        RestraurantsList.Add(new Restaurant { Name = "Louis B&B", TipPct = "18%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_531632635.jpg" });
    }
    
    public void AddRestaurant(object sender, EventArgs e)
    {
        RestraurantsList.Add(new Restaurant { Name = "New Restaurant", TipPct = "15%", ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_531632635.jpg" });
    }
    
    public void DeleteRestaurant(object sender, EventArgs e)
    {
        if (RestraurantsList.Count > 0)
        {
            RestraurantsList.RemoveAt(RestraurantsList.Count - 1);
        }
    }
}