﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using App1.Core.Models;
using App1.Core.Services;
using App1.Helpers;
using App1.Services;
using App1.Views;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;

using Windows.UI.Xaml.Controls;

namespace App1.ViewModels
{
    public class ImageGalleryViewModel : ObservableObject
    {
        public const string ImageGallerySelectedIdKey = "ImageGallerySelectedIdKey";

        private ICommand _itemSelectedCommand;

        public ObservableCollection<SampleImage> Source { get; } = new ObservableCollection<SampleImage>();

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnItemSelected));

        public ImageGalleryViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await SampleDataService.GetImageGalleryDataAsync("ms-appx:///Assets");

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        private void OnItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            ImagesNavigationHelper.AddImageId(ImageGallerySelectedIdKey, selected.ID);
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(selected);
            NavigationService.Navigate<ImageGalleryDetailPage>(selected.ID);
        }
    }
}
