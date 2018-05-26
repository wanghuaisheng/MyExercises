using System;

using LearnUniversalApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LearnUniversalApp.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailViewModel ViewModel => DataContext as MasterDetailViewModel;

        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}
