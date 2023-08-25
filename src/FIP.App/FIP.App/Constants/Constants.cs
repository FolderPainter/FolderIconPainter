﻿namespace FIP.App.Constants
{
    public static class AppConstants
    {
        public static class AssetPaths
        {
            public const string SVGFolderIconTemplate = "ms-appx:///Assets/win11-folder-template.svg";
        }

        public static class StorageSettings
        {
            public const string StorageFolderName = "LocalStorage";

            public const string CategoriesStorageFileName = "categories.json";

            public const string FolderIconsStorageFileName = "folderIcons.json";
        }

        public static class ColorSettings
        {
            public const string DefaultFolderColor = "#FCBD18";

            public const double MaxContrastLightness = 0.7D;

            public const double MinContrastHueAngle = 19D;

            public const double MaxContrastHueAngle = 191D;
        }
    }
}