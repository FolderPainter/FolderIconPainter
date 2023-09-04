using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.Core.Models;
using FIP.Core.Services;
using System;

namespace FIP.Core.ViewModels
{
    [Serializable]
    public class CustomIconViewModel : ObservableObject
    {
        private IFolderIconService FolderIconService { get; } = Ioc.Default.GetRequiredService<IFolderIconService>();

        public CustomIconViewModel(CustomIcon model = null)
        {
            Model = model ?? new CustomIcon();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Color))
                {
                    IsColorChanged = true;
                }
            };
        }

        private CustomIcon model;
        private bool isNewCustomIcon;

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
                    OnPropertyChanged(nameof(SvgIconPath));
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

        public bool IsNewCustomIcon
        {
            get => isNewCustomIcon;
            set => SetProperty(ref isNewCustomIcon, value);
        }

        public bool IsColorChanged { get; set; }

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

        public string IconPath => FolderIconService.GetFolderIconPath(Model);

        public string SvgIconPath => FolderIconService.GetSvgFolderIconPath(Model);
    }
}
