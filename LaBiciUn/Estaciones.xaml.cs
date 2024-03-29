﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class Estaciones : Page
    {

        public List<Datum> ListaEstaciones { get { return StationsDataManager.parsedJson.data; } }

        public string ro { get { return StationsDataManager.parsedJson.ToString(); } }


        public Estaciones()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            StationsDataManager.updatedData += updateBindings;
        }

        private void updateBindings(object sender, EventArgs e)
        {
            //Debug.WriteLine("Actualizando información visual");
            this.Bindings.Update();
        }


    }
}
