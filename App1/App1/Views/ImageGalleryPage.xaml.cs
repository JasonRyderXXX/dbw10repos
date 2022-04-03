using System;

using App1.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App1.Views
{
    public sealed partial class ImageGalleryPage : Page
    {
        public ImageGalleryViewModel ViewModel { get; } = new ImageGalleryViewModel();

        public ImageGalleryPage()
        {
            InitializeComponent();
            Loaded += ImageGalleryPage_Loaded;
        }

        private async void ImageGalleryPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }
    }
}
