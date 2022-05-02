using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityRawInput;
using WindowsInput;
using WindowsInput.Native;

public class KeyboardInput : SingletonMonoBehaviour<KeyboardInput>
{
    private InputSimulator _inputSimulator = new InputSimulator();
    
    [DllImport ("User32.dll")]
    static extern int SetForegroundWindow(IntPtr point);

    private RawKey _targetKey;

    public void SetTargetKey(int value)
    {
        _targetKey = (RawKey)(48 + value);
        Debug.LogWarning($"Skill set as {_targetKey}");
    }
    
    private void Start()
    {
        var workInBackground = true;
        
        RawKeyInput.Start(workInBackground);
        
        RawKeyInput.OnKeyUp += HandleKeyUp;
        RawKeyInput.OnKeyDown += HandleKeyDown;
    }

    private void OnDestroy()
    {
        RawKeyInput.OnKeyUp -= HandleKeyUp;
        RawKeyInput.OnKeyDown -= HandleKeyDown;
        
        RawKeyInput.Stop();
    }

    private void HandleKeyUp(RawKey key)
    {
        
    }

    private void HandleKeyDown(RawKey key)
    {
        if (RawKeyInput.IsKeyDown(RawKey.End))
            GameManager.Instance.OnClick_StartStop();;
    }

    public void PressKey()
    {
        switch (_targetKey)
        {
            case RawKey.N1:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_1));
                break;
            case RawKey.N2:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_2));
                break;
            case RawKey.N3:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_3));
                break;
            case RawKey.N4:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_4));
                break;
            case RawKey.N5:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_5));
                break;
            case RawKey.N6:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_6));
                break;
            case RawKey.N7:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_7));
                break;
            case RawKey.N8:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_8));
                break;
            case RawKey.N9:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_9));
                break;
            case RawKey.N0:
                StartCoroutine(SimulateKey(VirtualKeyCode.VK_0));
                break;
        }
    }

    private IEnumerator SimulateKey(VirtualKeyCode keyCode)
    {
        while (true)
        {
            _inputSimulator.Keyboard.KeyDown(keyCode);

            yield return new WaitForSeconds(0.01f);
            
            _inputSimulator.Keyboard.KeyPress(keyCode);
            
            yield return new WaitForSeconds(0.01f);
            
            _inputSimulator.Keyboard.KeyUp(keyCode);
            
            break;
        }
    }
}
