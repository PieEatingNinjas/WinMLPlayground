using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace WinMLPlayground.Converters
{
    public class StorageFileToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is StorageFile sf && sf != null)
            {
                var bitmap = new BitmapImage();
                sf.OpenReadAsync().AsTask().ContinueWith(async task =>
                {
                   await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                   async () =>
                   {
                       await bitmap.SetSourceAsync(task.Result);
                   });
                });
                return bitmap;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
