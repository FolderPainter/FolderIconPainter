using Windows.Win32.Foundation;

namespace FIP.App.Helpers;

internal partial class NativeMethods
{
    internal static bool IsAppPackaged { get; } = GetCurrentPackageName() != null;
    internal static string? GetCurrentPackageName()
    {
        unsafe
        {
            uint packageFullNameLength = 0;

            var result = Windows.Win32.PInvoke.GetCurrentPackageFullName(&packageFullNameLength, null);

            if (result == WIN32_ERROR.ERROR_INSUFFICIENT_BUFFER)
            {
                char* packageFullName = stackalloc char[(int)packageFullNameLength];

                result = Windows.Win32.PInvoke.GetCurrentPackageFullName(&packageFullNameLength, packageFullName);

                if (result == 0) // S_OK or ERROR_SUCCESS
                {
                    return new string(packageFullName, 0, (int)packageFullNameLength);
                }
            }
        }

        return null;
    }
}
