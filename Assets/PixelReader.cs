using System;
using System.Drawing;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class PixelReader : MonoBehaviour
{
    public Action<Vector2Int> Invoker { get; set; }
    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindowDC(IntPtr window);

    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern uint GetPixel(IntPtr dc, int x, int y);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int ReleaseDC(IntPtr window, IntPtr dc);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetCursorPos(ref Point lpPoint);

    [SerializeField] private UnityEngine.Color[] _targetColors;
    [SerializeField] private TextMeshProUGUI _txtCoordinate;
    [SerializeField] private TextMeshProUGUI _txtColor;
    [SerializeField] private Image _debugImage;
    [SerializeField] private RectTransform _targetRect;

    private float _lastCooldownTime;

    private void Awake()
    {
        _lastCooldownTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > _lastCooldownTime + FrequencySlider.Instance.GetValue())
        {
            ReadColor();
            
            _lastCooldownTime = Time.time;
        }
    }

    private void ReadColor()
    {
        float initialHeight = 600f;
        
        Vector2 position = new Vector2(_targetRect.transform.position.x, Screen.height - _targetRect.transform.position.y);
        
        var point = new Point((int)position.x, (int)position.y);

        // var mousePoint = new Point();
        // GetCursorPos(ref mousePoint);
        // Debug.LogWarning(mousePoint);
        
        var color = GetColorAt(point.X, point.Y);
        string hex = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

        _debugImage.color = new Color32(color.R, color.G, color.B, color.A);
        _txtCoordinate.text = $"X:{point.X} Y:{point.Y}";
        _txtColor.text = $"{color} #{hex}";

        if (GameManager.Instance.IsStarted == false)
            return;
        
        foreach (var targetColor in _targetColors)
        {
            if (_debugImage.color.CompareRGB(targetColor) == false)
            {
                Invoker?.Invoke(new Vector2Int((int)_targetRect.transform.position.x, (int)_targetRect.transform.position.y));
                Debug.LogWarning($@"Need heal on {gameObject.name}");
                return;
            }
        }
    }

    public static Color GetColorAt(int x, int y)
    {
        IntPtr desk = GetDesktopWindow();
        IntPtr dc = GetWindowDC(desk);
        int a = (int) GetPixel(dc, x, y);
        ReleaseDC(desk, dc);
        return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
    }
}