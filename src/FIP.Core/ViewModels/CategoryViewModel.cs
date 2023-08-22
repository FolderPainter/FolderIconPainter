using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace FIP.Core.ViewModels
{
    [Serializable]
    public class CategoryViewModel : ObservableObject
    {
        private string name;

        public Guid Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
