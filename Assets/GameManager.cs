using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityRawInput;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Transform _parent;
    [SerializeField] private PixelReader _targetPrefab;
    [SerializeField] private TextMeshProUGUI _txtStartStop;
    [SerializeField] private TMP_Dropdown _skillDropdown;
    [SerializeField] private float _keyPressCooldown = 0.5f;
    
    private List<PixelReader> _targets = new List<PixelReader>();
    private bool _isStarted = false;

    private float _lastKeyPressedCooldown;

    public bool IsStarted
    {
        get => _isStarted;
    }

    private void Start()
    {
        _lastKeyPressedCooldown = Time.time;

        SetSkill();
    }

    public void SetSkill()
    {
        KeyboardInput.Instance.SetTargetKey(_skillDropdown.value);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var target in _targets)
            target.Invoker -= OnInvoker;
    }

    public void OnClick_StartStop()
    {
        if (_targets.Count == 0)
            return;
        
        _isStarted = !_isStarted;

        if (_isStarted)
            _txtStartStop.text = "STOP";
        else
            _txtStartStop.text = "START";
    }

    public void OnClick_AddTarget()
    {
        if (_targets.Count >= 8)
            return;

        var target = Instantiate(_targetPrefab, _parent);
        target.Invoker += OnInvoker;
        
        _targets.Add(target);
    }

    public void OnClick_RemoveTarget()
    {
        if (_targets.Count <= 0)
            return;

        var deletedTarget = _targets[_targets.Count - 1];
        deletedTarget.Invoker -= OnInvoker;

        _targets.Remove(deletedTarget);
        
        Destroy(deletedTarget.gameObject);
    }

    private void OnInvoker(Vector2Int targetMousePosition)
    {
        if (_isStarted == false)
            return;

        if (Time.time > _lastKeyPressedCooldown + _keyPressCooldown)
        {
            Debug.LogWarning("Trying to heal.");
            
            MouseInput.Instance.SetPosition(targetMousePosition.x, targetMousePosition.y);
            KeyboardInput.Instance.PressKey();
            
            _lastKeyPressedCooldown = Time.time;
        }
    }
    
}
