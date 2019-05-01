using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinMLPlayground.Controls
{
    public sealed partial class ImageSelector : UserControl
    {
        public ImageSelector()
        {
            this.InitializeComponent();
        }

        public StorageFile SelectedImage
        {
            get { return (StorageFile)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }

        public static readonly DependencyProperty SelectedImageProperty =
            DependencyProperty.Register(nameof(SelectedImage), typeof(StorageFile), typeof(ImageSelector), new PropertyMetadata(null));

        private StorageFile SelectedListItem
        {
            get { return (StorageFile)GetValue(SelectedListItemProperty); }
            set
            {
                SetValue(SelectedListItemProperty, value);
                if (value != null)
                    SelectedImage = value;
            }
        }

        public static readonly DependencyProperty SelectedListItemProperty =
            DependencyProperty.Register(nameof(SelectedListItem), typeof(StorageFile), typeof(ImageSelector), new PropertyMetadata(null));

        private async void SelectImage()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            StorageFile selectedStorageFile = await fileOpenPicker.PickSingleFileAsync();
            SelectedListItem = null;
            SelectedImage = selectedStorageFile;
        }
    }
}
