using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;
        Location location;

        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetPeopleAsync();
            listView.ItemTapped += listViewItemSelected;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            listView.ItemTapped -= listViewItemSelected;
        }



        private void listViewItemSelected(object sender, ItemTappedEventArgs e)
        {
            location = (Location)e.Item;
            if(location != null)
            {
                cityEntry.Text = location.LocationName;
            }

        }

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cityEntry.Text))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                if (weatherData != null)
                {
                    var weatherPage = new WeatherPage((weatherData));
                    await Navigation.PushAsync(weatherPage);
                }
                else
                {
                    await DisplayAlert("Warning", "No data found for location", "OK");
                }
            }
        }

        async void OnAddToFavoritesButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cityEntry.Text))
            {
                List<Location> data = await App.Database.GetPeopleAsync(); ;
                bool matches = data.Exists(p => p.LocationName == cityEntry.Text);
                if (matches)
                {
                    await DisplayAlert("Warning", "Item already in your favorites", "OK");
                }
                else
                {
                    await App.Database.SaveLocationAsync(new Location
                    {
                        LocationName = cityEntry.Text,
                    });
                }
                listView.ItemsSource = await App.Database.GetPeopleAsync();
            }
        }

        async void OnDeleteFavoritesButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cityEntry.Text))
            {
                await App.Database.DeleteLocationAsync(location);
                cityEntry.Text = string.Empty;
            }
            listView.ItemsSource = await App.Database.GetPeopleAsync();
        }

        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={cityEntry.Text}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }

    }
}
