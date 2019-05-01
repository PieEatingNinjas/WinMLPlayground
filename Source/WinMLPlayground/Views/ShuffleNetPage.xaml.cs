using System;

using Windows.UI.Xaml.Controls;

using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class ShuffleNetPage : Page
    {
        private ShuffleNetViewModel ViewModel => DataContext as ShuffleNetViewModel;

        public ShuffleNetPage()
        {
            InitializeComponent();
        }
    }
}
