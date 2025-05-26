using UnityEngine;
using Scripts.Interfaces;

public class Door : BaseInteractable
{
    [Header("Door Settings")]
    [SerializeField] private float _openAngle = 90f;
    [SerializeField] private float _openSpeed = 2f;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private string _lockedPrompt = "Door is locked";
    [SerializeField] private string _openPrompt = "Press E to open";
    [SerializeField] private string _closePrompt = "Press E to close";

    private bool _isOpen = false;
    private float _currentAngle = 0f;
    private Quaternion _initialRotation;
    private Quaternion _targetRotation;

    protected override void Start()
    {
        base.Start();
        _initialRotation = transform.rotation;
        _targetRotation = _initialRotation;
    }

    private void Update()
    {
        // Плавно поворачиваем дверь к целевой ротации
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * _openSpeed);
    }

    public override void Interact(IPlayer player)
    {
        if (_isLocked)
        {
            return;
        }

        _isOpen = !_isOpen;
        _targetRotation = _initialRotation * Quaternion.Euler(0, _isOpen ? _openAngle : 0, 0);
    }

    public override string InteractionPrompt
    {
        get
        {
            if (_isLocked)
            {
                return _lockedPrompt;
            }
            return _isOpen ? _closePrompt : _openPrompt;
        }
    }

    public override bool CanInteract => !_isLocked;

    public void SetLocked(bool locked)
    {
        _isLocked = locked;
        if (locked && _isOpen)
        {
            _isOpen = false;
            _targetRotation = _initialRotation;
        }
    }
}