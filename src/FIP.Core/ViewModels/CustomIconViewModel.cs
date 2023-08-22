using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace FIP.Core.ViewModels
{
    [Serializable]
    public class CustomIconViewModel : ObservableObject, IEquatable<CustomIconViewModel>
    {
        private string name;
        private string infoTip;
        private string color;

        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string InfoTip 
        { 
            get => infoTip; 
            set => SetProperty(ref infoTip, value);
        }

        /// <summary>
        /// HEX formatted color.
        /// </summary>
        public string Color 
        { 
            get => color; 
            set => SetProperty(ref color, value); 
        }

        public bool Equals(CustomIconViewModel other) =>
            Name == other.Name &&
            Color == other.Color;
    }
}
