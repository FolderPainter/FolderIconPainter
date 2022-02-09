using System.ComponentModel;

namespace WebApp.Enums
{
    public enum ObjectPosition
    {
        [Description("center")]
        Center,
        [Description("top")]
        Top,
        [Description("bottom")]
        Bottom,
        [Description("left")]
        Left,
        [Description("left-top")]
        LeftTop,
        [Description("left-bottom")]
        LeftBottom,
        [Description("right")]
        Right,
        [Description("right-top")]
        RightTop,
        [Description("right-bottom")]
        RightBottom,
    }
}
