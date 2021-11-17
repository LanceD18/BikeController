using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BikeController.Core.Models;
using BikeController.Core.ViewModels;
using HackUTDBikeQueryApp.Core.Util;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Visibility = System.Windows.Visibility;

namespace BikeController.WPF.Views
{
    /// <summary>
    /// Interaction logic for iWalletView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(BikeControllerViewModel))]
    public partial class BikeControllerVIew : MvxWpfView
    {
        public BikeControllerVIew()
        {
            InitializeComponent();

            MapUtil.UpdateBikeMap += UpdateBikeMap;
        }

        private void BikeMap_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBikeMap();
        }

        private async void UpdateBikeMap()
        {
            BikeLocationModel currentBike = (DataContext as BikeControllerViewModel)?.CurrentBike;

            if (currentBike != null)
            {
                BikeMap.Visibility = Visibility.Visible;
            }
            else
            {
                BikeMap.Visibility = Visibility.Hidden;
                return;
            }

            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = currentBike.Latitude, Longitude = currentBike.Longitude };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.
            await BikeMap.TrySetViewAsync(cityCenter, 12);
        }
    }
}
