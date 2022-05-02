using System.Drawing;
using System.Runtime.InteropServices;

public class MouseInput : SingletonMonoBehaviour<MouseInput>
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point pos);

    public void SetPosition(int x, int y)
    {
        Point cursorPos = new Point();
        GetCursorPos(out cursorPos);
        
        SetCursorPos(x, y);
    }
}
