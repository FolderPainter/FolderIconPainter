using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace WebApp.Services
{
    public class IconService
    {
        //for custom
        public string GettingIcons(string dir)
        {
            try
            {
                if (string.IsNullOrEmpty(dir))
                {
                    dir = "Select an Icon";
                }
                else
                {
                    dir = "Select an Icon: " + dir;
                }

                // ELECTRON DIALOG
                //using (OpenFileDialog open = new OpenFileDialog())
                //{
                //    open.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                //    open.Filter = "Image Files(*.icon)|*.ico";
                //    open.Title = dir;

                //    if (open.ShowDialog() == DialogResult.OK)
                //    {
                //        pictureBox.Image = Bitmap.FromHicon(new Icon(open.FileName, new Size(256, 256)).Handle);
                //        return open.FileName;
                //    }
                //}
            }
            catch (Exception)
            {
            }

            return null;
        }

        public string SettingIcons(string dir, string icoPath, string folderType = "Generic")
        {
            var res = "done.";

            try
            {
                //deleting existing files
                res = RettingIcons(dir);

                //copying Icon file //overwriting
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

                res = RefreshIcons(dir);
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

        public string RefreshIcons(string dir)
        {
            try
            {
                // Attempt 01 
                Directory.Move(dir, dir + "_Processing");
                Directory.Move(dir + "_Processing", dir);

                // Attempt 02
                string localIconCachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\IconCache.db";
                if (File.Exists(localIconCachePath))
                {
                    File.Delete(localIconCachePath);
                }

                // Attempt 03
                string dirCachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Windows\Explorer\";
                DirectoryInfo di = new DirectoryInfo(dirCachePath);
                FileInfo[] files = di.GetFiles("iconcache*.db");
                foreach (FileInfo file in files)
                {
                    File.Delete(file.FullName);
                }

                //// Attempt 04.01
                //using (Process process = new Process())
                //{
                //    ProcessStartInfo startInfo = new ProcessStartInfo();
                //    startInfo.FileName = "cmd.exe";
                //    startInfo.Arguments = "/C ie4uinit.exe -ClearIconCache";
                //    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //    process.StartInfo = startInfo;
                //    process.Start();
                //}

                //// Attempt 04.02
                //using (Process process = new Process())
                //{
                //    ProcessStartInfo startInfo = new ProcessStartInfo();
                //    startInfo.FileName = "cmd.exe";
                //    startInfo.Arguments = "/C ie4uinit.exe -ClearIconCache";
                //    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //    process.StartInfo = startInfo;
                //    process.Start();
                //}

                //// Attempt 05
                //foreach (Process p in Process.GetProcesses())
                //{
                //    if (p.MainModule.ModuleName.Contains("explorer") == true)
                //    {
                //        p.Kill();
                //    }
                //}
                //Process.Start("explorer.exe");

                // Attempt 06
                SHChangeNotify(0x08000000, 0x0000, (IntPtr)null, (IntPtr)null);//SHCNE_ASSOCCHANGED SHCNF_IDLIST
                return "done";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
