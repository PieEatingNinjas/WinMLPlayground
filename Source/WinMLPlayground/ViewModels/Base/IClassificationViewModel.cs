using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.Storage;

namespace WinMLPlayground.ViewModels.Base
{
    public interface IClassificationViewModel : INotifyPropertyChanged
    {
        StorageFile SelectedFile
        {
            get;  set;
        }

        ObservableCollection<string> Result
        {
            get; set;
        }
    }
}
