﻿using System;

using LearnUniversalApp.ViewModels;

using Prism.Windows.Mvvm;

using Windows.UI.Xaml.Controls;

namespace LearnUniversalApp.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            ViewModel.Initialize(frame);
        }
    }
}
