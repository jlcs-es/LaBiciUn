using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LaBiciUn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Mapa : Page
    {

        public List<Datum> ListaEstaciones { get { return StationsDataManager.parsedJson.data; } }

        private Geopoint center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = 37.991985,
                    Longitude = -1.129554
                
                });
               

        public Mapa()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            centrarMapa();

            StationsDataManager.updatedData += updateBindings;
            
        }

        private void centrarMapa()
        {
            center = StationsDataManager.ciudades[StationsDataManager.City].Centro;
            myMap.Center = center;
            myMap.ZoomLevel = 15;
        }

        private void updateBindings(object sender, EventArgs e)
        {
            //Debug.WriteLine("Actualizando información visual");
            this.Bindings.Update();
        }

        private async void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            var status = await Geolocator.RequestAccessAsync();
            if (status == GeolocationAccessStatus.Allowed)
            {
                var locator = new Geolocator();
                var position = await locator.GetGeopositionAsync();
                var lat = position.Coordinate.Point.Position.Latitude;
                var lon = position.Coordinate.Point.Position.Longitude;
                center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = lat,
                    Longitude = lon
                });
                
                await myMap.TrySetViewAsync(center, 15);
                MapControl.SetLocation(posicion, center);
                posicion.Visibility = Visibility.Visible;
            }

        }
    }
}
