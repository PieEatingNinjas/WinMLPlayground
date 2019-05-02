using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinMLPlayground.ViewModels.Base;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinMLPlayground.Controls
{
    public sealed partial class ImageClassification : UserControl
    {
        public IClassificationViewModel ViewModel
        {
            get { return (IClassificationViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IClassificationViewModel), typeof(ImageClassification), new PropertyMetadata(null));

        public ImageClassification()
        {
            this.InitializeComponent();
        }
    }
}
