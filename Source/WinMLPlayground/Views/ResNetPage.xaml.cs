using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class ResNetPage : Page
    {
        private ResNetViewModel ViewModel => DataContext as ResNetViewModel;

        public ResNetPage()
        {
            InitializeComponent();
        }

        private async void SelectImage()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            StorageFile selectedStorageFile = await fileOpenPicker.PickSingleFileAsync();

            await ViewModel.Init(selectedStorageFile);
        }
    }
}
