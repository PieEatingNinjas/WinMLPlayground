using System;

using Windows.UI.Xaml.Controls;

using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class AlexNetPage : Page
    {
        private AlexNetViewModel ViewModel => DataContext as AlexNetViewModel;

        public AlexNetPage()
        {
            InitializeComponent();
        }
    }
}
