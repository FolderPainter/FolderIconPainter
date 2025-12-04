using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.App.ViewModels;
using FIP.Core.Models;
using FIP.Core.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.UI;

namespace FIP.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllFolderIconsPage : Page
    {
        private AllFolderIconsViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<AllFolderIconsViewModel>();

        
        private readonly List<GroupInfoList> CustomIconsGroups;

        private List<GroupInfoList> FilteredCustomIconsGroups { get; set; }

        public AllFolderIconsPage()
        {
            CustomIconsGroups = Task.Run(async () => await ViewModel.GetCustomIconsAsync()).Result;

            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //NavigationRootPageArgs args = (NavigationRootPageArgs)e.Parameter;
            //var menuItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.NavigationRootPage.NavigationView.MenuItems.First();
            //menuItem.IsSelected = true;

            itemsCVS.Source = new ObservableCollection<GroupInfoList>(CustomIconsGroups);
        }

        private void FilterFolderIcons()
        {
            if (string.IsNullOrEmpty(GridViewFilter.Text))
            {
                itemsCVS.Source = new ObservableCollection<GroupInfoList>(FilteredCustomIconsGroups);
                return;
            }

            var suggestions = new List<GroupInfoList>();

            var querySplit = GridViewFilter.Text.Split(" ");

            foreach (var group in FilteredCustomIconsGroups)
            {
                var matchingItems = group.Where(
                    item =>
                    {
                        // Idea: check for every word entered (separated by space) if it is in the name, 
                        // e.g. for query "split button" the only result should "SplitButton" since its the only query to contain "split" and "button"
                        // If any of the sub tokens is not in the string, we ignore the item. So the search gets more precise with more words
                        bool flag = true;
                        foreach (string queryToken in querySplit)
                        {
                            if (item.CustomIconVM.Name == null)
                            {
                                return false;
                            }

                            // Check if token is not in string
                            if (item.CustomIconVM.Name.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                            {
                                // Token is not in string, so we ignore this item.
                                flag = false;
                            }
                        }
                        return flag;
                    });

                if (matchingItems.Any())
                {
                    suggestions.Add(new GroupInfoList(matchingItems) { Key = group.Key, Title = group.Title });
                }
            }
            if (suggestions.Count > 0)
            {
                itemsCVS.Source = new ObservableCollection<GroupInfoList>(suggestions);
                //controlsSearchBox.ItemsSource = suggestions.OrderByDescending(i => i.Title.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Title);
            }
            else
            {
                itemsCVS.Source = null;
            }
        }

        private void GridViewFilterTextChanged(object sender, TextChangedEventArgs args)
        {
            FilterFolderIcons();
        }

        private void GridViewSearchBoxLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void ListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItems.Any())
            {
                FilteredCustomIconsGroups = CustomIconsGroups.Where(g => CategoryList.SelectedItems.Any(c => (c as CategoryViewModel).Model.Id == g.Key)).ToList();
            }
            else
            {
                FilteredCustomIconsGroups = CustomIconsGroups;
            }

            FilterFolderIcons();
        }
    }
}
