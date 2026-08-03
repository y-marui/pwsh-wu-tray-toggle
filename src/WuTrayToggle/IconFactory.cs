using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace WuTrayToggle;

internal static partial class IconFactory
{
    public static Icon Create(TrayState state)
    {
        using var bitmap = new Bitmap(64, 64);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (state == TrayState.Running)
            {
                using var pen = new Pen(Color.RoyalBlue, 8);
                g.DrawArc(pen, 10, 10, 44, 44, 0, 300);

                Point[] points =
                {
                    new(54, 32),
                    new(44, 42),
                    new(64, 42),
                };
                g.FillPolygon(Brushes.RoyalBlue, points);
            }
            else
            {
                using var pen = new Pen(Color.Red, 10);
                g.DrawLine(pen, 10, 10, 54, 54);
                g.DrawLine(pen, 54, 10, 10, 54);
            }
        }

        var hIcon = bitmap.GetHicon();
        try
        {
            using var handleIcon = Icon.FromHandle(hIcon);
            return (Icon)handleIcon.Clone();
        }
        finally
        {
            DestroyIcon(hIcon);
        }
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DestroyIcon(IntPtr hIcon);
}
