using System;

using Windows.UI.Xaml.Controls;

using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class DenseNetPage : Page
    {
        private DenseNetViewModel ViewModel => DataContext as DenseNetViewModel;

        public DenseNetPage()
        {
            InitializeComponent();
        }
    }
}
