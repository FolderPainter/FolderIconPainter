using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FIP.App.Views.Dialogs
{
    public sealed partial class RenameCategoryDialog : ContentDialog
    {
        private CreateCustomIconViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<CreateCustomIconViewModel>();

        private IEnumerable<string> existingNames;

        public RenameCategoryDialog(IEnumerable<string> categoryNames)
        {
            this.InitializeComponent();
            existingNames = categoryNames;
        }
       
        private void NameTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (existingNames.Any(n => n == NameTextBox.Text) || String.IsNullOrEmpty(NameTextBox.Text))
            {
                ErrorInfoBar.Visibility = Visibility.Visible;
                IsPrimaryButtonEnabled = false;
            }
            else
            {
                ErrorInfoBar.Visibility = Visibility.Collapsed;
                IsPrimaryButtonEnabled = true;
            }
        }

        private void ContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.RenameCurrentCategory();
        }

        private void ContentDialogClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            ViewModel.RefreshCurrentCategoryName();
        }
    }
}
