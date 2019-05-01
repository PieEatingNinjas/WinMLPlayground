using Windows.UI.Xaml.Controls;
using WinMLPlayground.ViewModels;

namespace WinMLPlayground.Views
{
    public sealed partial class SqueezeNetPage : Page
    {
        private SqueezeNetViewModel ViewModel => DataContext as SqueezeNetViewModel;

        public SqueezeNetPage()
        {
            InitializeComponent();
        }
    }
}
