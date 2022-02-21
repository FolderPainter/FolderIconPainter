namespace WebApp.Models
{
    public class FolderIconStrings
    {
        public string EmptyFolderIcon { get; set; }

        public string DefFolderIcon { get; set; }

        public bool IsNullOrWhiteSpace()
        {
            return string.IsNullOrWhiteSpace(EmptyFolderIcon)
                || string.IsNullOrWhiteSpace(DefFolderIcon);
        }
    }
}
