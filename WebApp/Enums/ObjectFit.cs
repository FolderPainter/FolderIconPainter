using System.ComponentModel;

namespace WebApp.Enums
{
    public enum ObjectFit
    {
        [Description("none")]
        None,
        [Description("cover")]
        Cover,
        [Description("contain")]
        Contain,
        [Description("fill")]
        Fill,
        [Description("scale-down")]
        ScaleDown
    }
}
