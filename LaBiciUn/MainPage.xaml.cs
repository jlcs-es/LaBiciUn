using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LaBiciUn
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            Inicializar();
        }



        ////////////////////////////

        private async void Inicializar()
        {
            await StationsDataManager.GetData();
            MapaButton_Click(null, null);
        }




        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ContenidoFrame.CanGoBack)
            {
                ContenidoFrame.GoBack();
                e.Handled = true;
            }
        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }


        private async void SyncButton_Click(object sender, RoutedEventArgs e)
        {

            SyncButton.Visibility = Visibility.Collapsed;
            LoadingRing.Visibility = Visibility.Visible;
            await StationsDataManager.GetData();
            LoadingRing.Visibility = Visibility.Collapsed;
            SyncButton.Visibility = Visibility.Visible;

        }

        private void MapaButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = false;
            SubtitleTopBar.Text = "Mapa";
            ContenidoFrame.Navigate(typeof(Mapa));
        }

        private void EstacionesButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = false;
            SubtitleTopBar.Text = "Estaciones";
            ContenidoFrame.Navigate(typeof(Estaciones));
        }

        private void AcercadeButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = false;
            SubtitleTopBar.Text = "Acerca de";
            ContenidoFrame.Navigate(typeof(About));
        }


        private void ConfiguracionButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = false;
            SubtitleTopBar.Text = "Configuración";
            ContenidoFrame.Navigate(typeof(Configuracion));
        }

        //////////////////////////////






    }
}
