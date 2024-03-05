using CommunityToolkit.Mvvm.ComponentModel;
using FIP.Core.Models;

namespace FIP.Core.ViewModels
{
    public class CategoryViewModel : ObservableObject
    {
        public CategoryViewModel(Category model = null) => Model = model ?? new Category();

        private Category model;
        private bool isNewCategory;

        /// <summary>
        /// Gets or sets the underlying Category object.
        /// </summary>
        public Category Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    model = value;

                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public string Name
        {
            get => string.IsNullOrEmpty(Model.Name) ? "No category" : Model.Name;
            set => SetProperty(Model.Name, value, Model, (u, n) => u.Name = n);
        }

        public bool IsNewCategory 
        { 
            get => isNewCategory;
            set => SetProperty(ref isNewCategory, value); 
        }

        public override string ToString()
        {
            return IsNewCategory ? Name + " (New)" : Name;
        }
    }
}
