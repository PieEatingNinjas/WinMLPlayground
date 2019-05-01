using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

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
            DependencyProperty.Register("SelectedImage", typeof(StorageFile), typeof(ImageSelector), new PropertyMetadata(null));

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

        // Using a DependencyProperty as the backing store for SelectedListItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedListItemProperty =
            DependencyProperty.Register("SelectedListItem", typeof(StorageFile), typeof(ImageSelector), new PropertyMetadata(null));


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
