using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class SqueezeNetPage : Page
    {
        private SqueezeNetViewModel ViewModel => DataContext as SqueezeNetViewModel;

        public SqueezeNetPage()
        {
            InitializeComponent();
        }

        private async  void SelectImage()
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
