using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace WebApp.Services
{
    public class IconService
    {

        public int FoldersQuantity { get; set; }

        public int CurrentProgress { get; set; }

        public string SettingIcons(string dir, string icoPath, string folderType = "Generic")
        {
            var res = "done.";

            try
            {
                //deleting existing files
                res = RettingIcons(dir);

                ////copying Icon file //overwriting
                File.Copy(icoPath, dir + @"\Icon.ico", true);
                //System.IO.File.Copy(filePath, TempIconSaveLocation + GetDateTime() + ".ico", true);

                //writing configuration file
                string[] lines = { "[.ShellClassInfo]", "IconResource=Icon.ico,0", "[ViewState]", "Mode=", "Vid=", "FolderType=" + folderType };
                File.WriteAllLines(dir + @"\desktop.ini", lines);

                //configure file 2            
                string[] linesLinux = { "desktop.ini", "Icon.ico" };
                File.WriteAllLines(dir + @"\.hidden", linesLinux);

                //making system files
                File.SetAttributes(dir + @"\desktop.ini", File.GetAttributes(dir + @"\desktop.ini") | FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly);
                File.SetAttributes(dir + @"\Icon.ico", File.GetAttributes(dir + @"\Icon.ico") | FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly);
                File.SetAttributes(dir + @"\.hidden", File.GetAttributes(dir + @"\.hidden") | FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly);

                File.SetAttributes(dir, File.GetAttributes(dir) | FileAttributes.ReadOnly);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
            }
            return res;
        }

        public string RettingIcons(string dir)
        {
            try
            {
                // desktop.ini
                if (File.Exists(dir + @"\desktop.ini"))
                {
                    File.SetAttributes(dir + @"\desktop.ini", File.GetAttributes(dir + @"\desktop.ini") | FileAttributes.Normal); //Normal file

                    FileInfo fileInfo = new FileInfo(dir + @"\desktop.ini");
                    fileInfo.IsReadOnly = false;

                    File.Delete(dir + @"\desktop.ini");
                }

                // Icon.ico
                if (File.Exists(dir + @"\Icon.ico"))
                {
                    File.SetAttributes(dir + @"\Icon.ico", File.GetAttributes(dir + @"\Icon.ico") | FileAttributes.Normal); //Normal file

                    FileInfo fileInfo = new FileInfo(dir + @"\Icon.ico");
                    fileInfo.IsReadOnly = false;

                    File.Delete(dir + @"\Icon.ico");
                }

                // .hidden
                if (File.Exists(dir + @"\.hidden"))
                {
                    File.SetAttributes(dir + @"\.hidden", File.GetAttributes(dir + @"\.hidden") | FileAttributes.Normal); //Normal file

                    FileInfo fileInfo = new FileInfo(dir + @"\.hidden");
                    fileInfo.IsReadOnly = false;

                    File.Delete(dir + @"\.hidden");
                }
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RefreshIcons()
        {
            try
            {
                //SHCNE_ASSOCCHANGED SHCNF_IDLIST
                SHChangeNotify(0x08000000, 0x0000, (IntPtr)null, (IntPtr)null);
                RefreshIconCache();
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);

        #region RefreshIconCache

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr SendMessageTimeout(int windowHandle, int Msg, int wParam,
           String lParam, SendMessageTimeoutFlags flags, int timeout, out int result);

        [Flags] enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8
        }

        private void RefreshIconCache()
        {
            // get the the original Shell Icon Size registry string value
            RegistryKey k = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);
            Object OriginalIconSize = k.GetValue("Shell Icon Size");

            // set the Shell Icon Size registry string value
            k.SetValue("Shell Icon Size", (Convert.ToInt32(OriginalIconSize) + 1).ToString());
            k.Flush(); 
            k.Close();

            // broadcast WM_SETTINGCHANGE to all window handles
            int res = 0;
            SendMessageTimeout(0xffff, 0x001A, 0, "", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5000, out res);

            //SendMessageTimeout(HWD_BROADCAST,WM_SETTINGCHANGE,0,"",SMTO_ABORTIFHUNG,5 seconds, return result to res)
            // set the Shell Icon Size registry string value to original value
            k = Registry.CurrentUser.OpenSubKey("Control Panel").OpenSubKey("Desktop").OpenSubKey("WindowMetrics", true);
            k.SetValue("Shell Icon Size", OriginalIconSize);
            k.Flush(); 
            k.Close();

            SendMessageTimeout(0xffff, 0x001A, 0, "", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5000, out res);
        }

        #endregion
    }
}
