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
    }
}
