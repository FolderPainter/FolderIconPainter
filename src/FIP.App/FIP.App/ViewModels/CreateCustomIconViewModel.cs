using CommunityToolkit.Mvvm.ComponentModel;
using FIP.Core.ViewModels;
using System.Xml.Linq;

namespace FIP.App.ViewModels
{
    public class CreateCustomIconViewModel : ObservableObject
    {
        private CategoryViewModel currentCategory = new();
        private CustomIconViewModel newCustomIcon = new();

        
        public CategoryViewModel CurrentCategory { get => currentCategory; set => SetProperty(ref currentCategory, value); }

        public CustomIconViewModel NewCustomIcon { get => newCustomIcon; set => SetProperty(ref newCustomIcon, value); }
    }
}
