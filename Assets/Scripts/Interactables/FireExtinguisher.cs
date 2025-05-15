using UnityEngine;
using Scripts.Interfaces;

public class FireExtinguisher : BaseInteractable
{
    [Header("Extinguisher Settings")]
    [SerializeField] private float _extinguishingPower = 10f;
    [SerializeField] private float _useDuration = 10f;
    [SerializeField] private ParticleSystem _extinguisherEffect;
    [SerializeField] private Transform _nozzleTransform; // Точка выхода пены
    
    private bool _isBeingUsed = false;
    private float _currentUseTime = 0f;
    private bool _isPickedUp = false;
    
    protected override void Start()
    {
        base.Start();
        if (_extinguisherEffect != null)
        {
            _extinguisherEffect.Stop();
        }
    }
    
    public override void Interact(IPlayer player)
    {
        if (!_isBeingUsed && _isPickedUp)
        {
            StartExtinguishing();
        }
    }
    
    private void Update()
    {
        if (_isBeingUsed)
        {
            _currentUseTime += Time.deltaTime;
            
            if (_currentUseTime >= _useDuration)
            {
                StopExtinguishing();
            }
            
            // Проверяем, есть ли огонь перед игроком
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 5f))
            {
                IFire fire = hit.collider.GetComponent<IFire>();
                if (fire != null)
                {
                    fire.Extinguish(_extinguishingPower * Time.deltaTime);
                }
            }
        }
    }
    
    private void StartExtinguishing()
    {
        _isBeingUsed = true;
        _currentUseTime = 0f;
        if (_extinguisherEffect != null)
        {
            _extinguisherEffect.Play();
        }
    }
    
    private void StopExtinguishing()
    {
        _isBeingUsed = false;
        if (_extinguisherEffect != null)
        {
            _extinguisherEffect.Stop();
        }
    }
    
    // Вызывается при подборе предмета
    public void OnPickup()
    {
        _isPickedUp = true;
        if (_extinguisherEffect != null)
        {
            _extinguisherEffect.transform.SetParent(_nozzleTransform);
            _extinguisherEffect.transform.localPosition = Vector3.zero;
            _extinguisherEffect.transform.localRotation = Quaternion.identity;
        }
    }
    
    // Вызывается при выбрасывании предмета
    public void OnDrop()
    {
        _isPickedUp = false;
        StopExtinguishing();
        if (_extinguisherEffect != null)
        {
            _extinguisherEffect.transform.SetParent(transform);
            _extinguisherEffect.transform.localPosition = Vector3.zero;
            _extinguisherEffect.transform.localRotation = Quaternion.identity;
        }
    }
} 