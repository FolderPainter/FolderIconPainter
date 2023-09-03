using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using FIP.App.Constants;
using FIP.App.Helpers;
using FIP.Core.Models;
using FIP.Core.Services;
using FIP.Core.ViewModels;
using Microsoft.Graphics.Canvas.Svg;
using Svg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Bitmap = System.Drawing.Bitmap;

namespace FIP.App.ViewModels
{
    public class CustomIconsViewModel : ObservableObject
    {
        private ICategoryStorageService CategoryStorageService { get; } = Ioc.Default.GetRequiredService<ICategoryStorageService>();

        private ICustomIconStorageService CustomIconStorageService { get; } = Ioc.Default.GetRequiredService<ICustomIconStorageService>();

        private IFolderIconService FolderIconService { get; } = Ioc.Default.GetRequiredService<IFolderIconService>();

        public IEnumerable<Category> Categories { get => CategoryStorageService.Categories; }

        public ObservableCollection<CustomIconViewModel> CustomIconViewModels;
        public CanvasSvgDocument CanvasSVG;
        private CategoryViewModel currentCategory = new();
        private CustomIconViewModel newCustomIcon = new();
        private Color buttonTitleColor;
        private Color pickedColor;
        private List<CustomIconViewModel> selectedCustomIcons;

        private CustomIconViewModel selectedCustomIcon;

        public CustomIconsViewModel()
        {
            CurrentCategory = new CategoryViewModel(new Category { Id = Guid.Empty });
            CustomIconViewModels = new ObservableCollection<CustomIconViewModel>();

            PropertyChanged += CreateCustomIconPropertyChanged;
        }

        public string CreateOrEditButtonContent => NewCustomIcon.IsNewCustomIcon ? "Create folder icon" : "Edit folder icon";

        public CategoryViewModel CurrentCategory
        {
            get => currentCategory;
            set => SetProperty(ref currentCategory, value);
        }

        public CustomIconViewModel NewCustomIcon
        {
            get => newCustomIcon;
            set
            {
                SetProperty(ref newCustomIcon, value);
                OnPropertyChanged(nameof(CreateOrEditButtonContent));
            }
        }

        public Color PickedColor
        {
            get => pickedColor;
            set => SetProperty(ref pickedColor, value);
        }

        public Color ButtonTitleColor
        {
            get => buttonTitleColor;
            set => SetProperty(ref buttonTitleColor, value);
        }

        public List<CustomIconViewModel> SelectedCustomIcons
        {
            get => selectedCustomIcons;
            set => SetProperty(ref selectedCustomIcons, value);
        }

        public CustomIconViewModel SelectedCustomIcon
        {
            get => selectedCustomIcon;
            set => SetProperty(ref selectedCustomIcon, value);
        }

        private void CreateCustomIconPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(CurrentCategory))
            {
                RefreshCustomIconViewModels();
                NewCustomIcon.CategoryId = CurrentCategory.Model.Id;
            }
        }

        public void RefreshCustomIconViewModels()
        {
            var customIcons = CustomIconStorageService.GetCustomIconsByCategoryId(CurrentCategory.Model.Id);
            CustomIconViewModels.Clear();
            if (customIcons.Any())
            {
                foreach (var icon in customIcons)
                {
                    CustomIconViewModels.Add(new CustomIconViewModel(icon));
                }
            }
        }

        public void ClearSelectedCustomIcons()
        {
            SelectedCustomIcons.Clear();
            OnPropertyChanged(nameof(SelectedCustomIcons));
        }

        public void RefreshCurrentCategoryName()
        {
            if (CurrentCategory.Model.Id == Guid.Empty)
            {
                CurrentCategory.Name = String.Empty;
            }
            else
            {
                CurrentCategory.Name = CategoryStorageService.GetCategoryById(CurrentCategory.Model.Id).Name;
            }
        }

        public void RenameCurrentCategory()
        {
            CategoryStorageService.PostCategory(CurrentCategory.Model);
        }

        private Bitmap SaveSvgDocumentToBitmap(SvgDocument svgDocument)
        {
            if (svgDocument is null)
                return new Bitmap(0, 0);

            svgDocument.ShapeRendering = SvgShapeRendering.Auto;
            svgDocument.Ppi = AppConstants.IconSettings.SVGRenderingPpi;
            Bitmap svgBitmap = svgDocument.Draw(AppConstants.IconSettings.DefaultIconSize, AppConstants.IconSettings.DefaultIconSize);

            var svgBitmapGraphics = System.Drawing.Graphics.FromImage(svgBitmap);
            svgBitmapGraphics.SmoothingMode = SmoothingMode.Default;
            svgBitmapGraphics.DrawImage(svgBitmap, 0, 0,
                AppConstants.IconSettings.DefaultIconSize,
                AppConstants.IconSettings.DefaultIconSize);

            return svgBitmap;
        }

        private async Task<bool> CreateFolderIconAsync()
        {
            if (CanvasSVG == null)
                return false;

            if (NewCustomIcon.IsColorChanged || NewCustomIcon.IsNewCustomIcon)
            {
                var svgString = CanvasSVG.GetXml();
                SvgDocument newSVG = SvgDocument.FromSvg<SvgDocument>(svgString);
                var svgBitmap = SaveSvgDocumentToBitmap(newSVG);

                if (FolderIconService.FolderIconExists(NewCustomIcon.Model))
                {
                    await FolderIconService.DeleteFolderIconAsync(NewCustomIcon.Model);
                }

                return await FolderIconService.CreateFolderIconAsync(NewCustomIcon.Model, svgBitmap);
            }
            return true;
        }

        public async Task CreateCustomIconAsync()
        {
            if (await CreateFolderIconAsync())
            {
                if (CurrentCategory.IsNewCategory)
                {
                    CurrentCategory = new CategoryViewModel(CategoryStorageService.AddCategory(CurrentCategory.Model));
                }
                else
                {
                    NewCustomIcon.CategoryId = Guid.Empty;
                }

                CustomIconStorageService.PostCustomIcon(NewCustomIcon.Model);

                if (NewCustomIcon.IsNewCustomIcon)
                {
                    NewCustomIcon.IsNewCustomIcon = false;
                    CustomIconViewModels.Add(NewCustomIcon);
                }
                else
                {
                    CustomIconViewModels.Insert(CustomIconViewModels.IndexOf(NewCustomIcon), new CustomIconViewModel
                    {
                        Model = new CustomIcon
                        {
                            Id = NewCustomIcon.Model.Id,
                            Name = NewCustomIcon.Name,
                            InfoTip = NewCustomIcon.InfoTip,
                            Color = NewCustomIcon.Color,
                            CategoryId = NewCustomIcon.CategoryId,
                        }
                    });
                    CustomIconViewModels.Remove(NewCustomIcon);
                    //RefreshCustomIconViewModels();
                }

                NewCustomIcon = new CustomIconViewModel
                {
                    IsNewCustomIcon = true,
                    Model = new CustomIcon
                    {
                        Name = NewCustomIcon.Name,
                        InfoTip = NewCustomIcon.InfoTip,
                        Color = NewCustomIcon.Color,
                        CategoryId = NewCustomIcon.CategoryId,
                    }
                };
            }
        }

        public async Task DeleteSelectedCustomIcons()
        {
            try
            {
                foreach (var item in SelectedCustomIcons)
                {
                    if (await FolderIconService.DeleteFolderIconAsync(item.Model))
                    {
                        CustomIconStorageService.DeleteCustomIconById(item.Model.Id);
                    }
                }
                RefreshCustomIconViewModels();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
