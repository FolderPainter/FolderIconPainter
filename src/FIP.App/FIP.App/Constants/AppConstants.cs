using System;

namespace FIP.App.Constants
{
    public static class AppConstants
    {
        public static Guid DefaultCategoryId => new Guid("11111111-1111-1111-1111-111111111111");

        public static class AssetPaths
        {
            public const string SVGFolderIconTemplate = "Assets/win11-folder-template.svg";
            
            public const string DefaultIconsJSON = "Assets/DefaultIcons.json";

            public const string DefaultIconsFolder = "Assets/DefaultIcons/";
        }

        public static class StorageSettings
        {
            public const string StorageFolderName = "LocalStorage";

            public const string IconsFolderName = "FolderIcons";

            public const string CategoriesStorageFileName = "categories.json";

            public const string FolderIconsStorageFileName = "folderIcons.json";

            public const string LogsFileName = "logs.json";
        }

        public static class ColorSettings
        {
            public const string DefaultFolderColor = "#FCBD18";

            public const double MinContrastLightness = 0.7D;

            public const double MinContrastHueAngle = 19D;

            public const double MaxContrastHueAngle = 191D;
        }

        public static class IconSettings
        {
            public const int DefaultIconSize = 256;

            public const int SVGRenderingPpi = 72;
        }

        public static class GitHub
        {
            public const string BugReportUrl = @"";

        }

    }
}
