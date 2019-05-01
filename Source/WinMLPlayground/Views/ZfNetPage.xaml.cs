using System;

using Windows.UI.Xaml.Controls;

using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class ZfNetPage : Page
    {
        private ZfNetViewModel ViewModel => DataContext as ZfNetViewModel;

        public ZfNetPage()
        {
            InitializeComponent();
        }
    }
}
