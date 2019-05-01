using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace WinMLPlayground.Helpers
{
    public static class SampleImagesHelper
    {
        static ObservableCollection<StorageFile> _Samples;
        public static ObservableCollection<StorageFile> Samples()
        {
            if(_Samples == null)
            {
                _Samples = new ObservableCollection<StorageFile>();
                LoadSampleImages();
            }
            return _Samples;
        }

        private static async Task LoadSampleImages()
        {
            var localizationDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\SampleImages");
            var files = await localizationDirectory.GetFilesAsync();
            foreach (var file in files)
            {
                //var bitmap = new BitmapImage();
                //using (var stream = await file.OpenReadAsync())
                //{
                //    await bitmap.SetSourceAsync(stream);
                //}
                _Samples.Add(file);
            }
        }
    }
}
