using CommunityToolkit.Mvvm.ComponentModel;
using FIP.Core.Models;
using System;

namespace FIP.Core.ViewModels
{
    [Serializable]
    public class CustomIconViewModel : ObservableObject
    {
        public CustomIconViewModel(CustomIcon model = null) => Model = model ?? new CustomIcon();

        private CustomIcon model;

        /// <summary>
        /// Gets or sets the underlying Customer object.
        /// </summary>
        public CustomIcon Model
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

        public Guid CategoryId
        {
            get => Model.CategoryId;
            set => SetProperty(Model.CategoryId, value, Model, (u, n) => u.CategoryId = n);
        }

        public string Name
        {
            get => Model.Name;
            set => SetProperty(Model.Name, value, Model, (u, n) => u.Name = n);
        }

        public string InfoTip
        {
            get => Model.InfoTip;
            set => SetProperty(Model.InfoTip, value, Model, (u, n) => u.InfoTip = n);
        }

        /// <summary>
        /// HEX formatted color.
        /// </summary>
        public string Color
        {
            get => Model.Color;
            set => SetProperty(Model.Color, value, Model, (u, n) => u.Color = n);
        }
    }
}
