using System.Collections.ObjectModel;
using TipCalculator.Models;

namespace TipCalculator.Views;

public partial class RestaurantsPage : ContentPage
{
    public RestaurantsPage()
    {
        InitializeComponent();
        CreateRestaurants();
        BindingContext = this;
    }

    public ObservableCollection<Restaurant> RestaurantsList { get; set; }

    private void CreateRestaurants()
    {
        RestaurantsList = new ObservableCollection<Restaurant>();
        RestaurantsList.Add(new Restaurant
        {
            Name = "Joe's Pizza", TipPct = "15%",
            ImageUrl =
                "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/02/Enjoy-5-authentic-wine-samples-of-the-region-on-the-Taormina-Wine-and-Food-Tasting-Tour_48.jpg"
        });
        RestaurantsList.Add(new Restaurant
        {
            Name = "Luigi's Pasta", TipPct = "18%",
            ImageUrl =
                "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/piedmont-old-town-restaurant-scaled.jpg"
        });
        RestaurantsList.Add(new Restaurant
        {
            Name = "Maria's Bistro", TipPct = "20%",
            ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_789017482.jpg"
        });
        RestaurantsList.Add(new Restaurant
        {
            Name = "Mario's Pizzeria", TipPct = "15%",
            ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_1054754711.jpg"
        });
        RestaurantsList.Add(new Restaurant
        {
            Name = "Louis B&B", TipPct = "18%",
            ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_531632635.jpg"
        });
    }

    public void AddRestaurant(object sender, EventArgs e)
    {
        RestaurantsList.Add(new Restaurant
        {
            Name = "New Restaurant", TipPct = "15%",
            ImageUrl = "https://i0.wp.com/www.touristitaly.com/wp-content/uploads/2023/11/shutterstock_531632635.jpg"
        });
    }

    public void DeleteRestaurant(object sender, EventArgs e)
    {
        if (RestaurantsList.Count > 0) RestaurantsList.RemoveAt(RestaurantsList.Count - 1);
    }
}