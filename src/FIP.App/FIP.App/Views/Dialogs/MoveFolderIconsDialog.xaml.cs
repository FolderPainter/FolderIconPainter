using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.ViewModels;
using FIP.Core.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace FIP.App.Views.Dialogs
{
    public sealed partial class MoveFolderIconsDialog : ContentDialog
    {
        private CustomIconsViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<CustomIconsViewModel>();

        private readonly List<CategoryViewModel> AvailableCategories;

        public MoveFolderIconsDialog()
        {
            this.InitializeComponent();

            AvailableCategories = ViewModel.Categories.Where(c => c.Id != ViewModel.CurrentCategory.Model.Id)
                .Select(c => new CategoryViewModel(c)).ToList();
        }

        private void ContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.MoveSelectedFolderIcons();
        }

        private void ContentDialogClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            //ViewModel.CategoryToMove = null;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CategoryToMove = (sender as ComboBox).SelectedItem as CategoryViewModel;
        }
    }
}
