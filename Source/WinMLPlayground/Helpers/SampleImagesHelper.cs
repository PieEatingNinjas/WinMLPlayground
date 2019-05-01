using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

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
            var localizationDirectory = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\SampleImages");
            var files = await localizationDirectory.GetFilesAsync();

            files.ToList().ForEach(_Samples.Add);
        }
    }
}
