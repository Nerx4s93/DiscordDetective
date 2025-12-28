using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PipeScript.UI;

internal class RichTextBoxNoSmoothScroll : RichTextBox
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    private const int WM_MOUSEWHEEL = 0x20A;
    private const int WM_VSCROLL = 0x115;

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_MOUSEWHEEL)
        {
            var scrollLines = SystemInformation.MouseWheelScrollLines;
            for (var i = 0; i < scrollLines; i++)
            {
                SendMessage(Handle, WM_VSCROLL, (int)m.WParam > 0 ? 0 : 1, IntPtr.Zero);
            }
            return;
        }
        base.WndProc(ref m);
    }
}
