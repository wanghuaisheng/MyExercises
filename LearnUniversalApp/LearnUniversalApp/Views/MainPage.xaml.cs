using System;

using LearnUniversalApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LearnUniversalApp.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
