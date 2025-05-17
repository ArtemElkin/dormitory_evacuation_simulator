using UnityEngine;
using Scripts.Interfaces;

// Не забудь добавить этот using, если класс ParticleSystemPool в другом namespace
// using YourNamespace;

public class FireExtinguisher : MonoBehaviour, IPickable
{
    [Header("Extinguisher Settings")]
    [SerializeField] private float _extinguishingPower = 10f;
    [SerializeField] private float _useDuration = 10f;
    [SerializeField] private ParticleSystemPool _foamPool;
    [SerializeField] private Transform _nozzleTransform;

    private bool _isBeingUsed = false;
    private float _currentUseTime = 0f;
    private bool _isPickedUp = false;
    private ParticleSystem _currentEffect;

    public void OnPickup() => _isPickedUp = true;

    public void OnDrop()
    {
        _isPickedUp = false;
        StopUse();
    }

    public void Use()
    {
        if (!_isBeingUsed && _isPickedUp)
        {
            _isBeingUsed = true;
            _currentUseTime = 0f;
            _currentEffect = _foamPool.Get();
            _currentEffect.transform.SetParent(_nozzleTransform);
            _currentEffect.transform.localPosition = Vector3.zero;
            _currentEffect.transform.localRotation = Quaternion.identity;
            _currentEffect.Play();
        }
    }

    public void StopUse()
    {
        _isBeingUsed = false;
        if (_currentEffect != null)
        {
            _currentEffect.transform.SetParent(null);
            _foamPool.Release(_currentEffect);
            _currentEffect = null;
        }
    }

    private void Update()
    {
        if (_isBeingUsed)
        {
            _currentUseTime += Time.deltaTime;
            if (_currentUseTime >= _useDuration)
            {
                StopUse();
            }
            // Эффект всегда следует за соплом
            if (_currentEffect != null)
            {
                _currentEffect.transform.position = _nozzleTransform.position;
                _currentEffect.transform.rotation = _nozzleTransform.rotation;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 5f))
            {
                var fire = hit.collider.GetComponent<IFire>();
                if (fire != null)
                {
                    fire.Extinguish(_extinguishingPower * Time.deltaTime);
                }
            }
        }
    }
} 