namespace FIP.Core.Services
{
    public interface IFolderPainterService
    {
        string SettingIcon(string dir, string icoPath, string folderType = "Generic");
        string DeleteIcon(string dir);
        string RefreshIcons();
    }
}
