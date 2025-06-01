using UnityEngine;
using System.Collections.Generic;

public abstract class BaseFire : MonoBehaviour, IFire
{
    [Header("Fire Settings")]
    [SerializeField] protected float _maxIntensity = 100f;
    [SerializeField] protected float _currentIntensity;
    [SerializeField] protected float _spreadRate = 5f;
    [SerializeField] protected float _damagePerSecond = 10f;
    [SerializeField] protected float _smokeProductionRate = 0.2f;
    
    [Header("Effects")]
    [SerializeField] protected ParticleSystem _fireParticles;
    [SerializeField] protected Light _fireLight;
    
    [Header("Spread Settings")]
    [SerializeField] protected float _spreadRadius = 2f;
    [SerializeField] protected LayerMask _spreadableLayers;
    
    protected List<BaseFire> _spreadFires = new List<BaseFire>();
    protected bool _isSpreading = false;
    protected bool _isActive = false;
    protected ParticleSystem _smokeParticles;
    
    public virtual float Intensity 
    { 
        get => _currentIntensity;
        set
        {
            _currentIntensity = Mathf.Clamp(value, 0, _maxIntensity);
            var main = _fireParticles.main;
            main.startSize = Mathf.Clamp(0.1f, 1, _currentIntensity / _maxIntensity);
        }
    }
    
    public virtual float DamagePerSecond => _damagePerSecond;
    
    protected virtual void Start()
    {
        _currentIntensity = _maxIntensity;
        if (_fireParticles != null)
        {
            _fireParticles.Stop();
        }
        _fireParticles = GetComponentInChildren<ParticleSystem>();
        if (_fireParticles != null)
        {
            _fireParticles.Stop();
        }
    }
    
    protected virtual void Update()
    {
        if (_isSpreading)
        {
            Spread();
        }
        
        // Нанесение урона игроку при контакте
        Collider[] colliders = Physics.OverlapSphere(transform.position, _spreadRadius);
        foreach (Collider collider in colliders)
        {
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(_damagePerSecond * Time.deltaTime);
                playerStats.UpdateTemperature(10f * Time.deltaTime);
            }
        }
    }
    
    public virtual void StartSpreading()
    {
        _isSpreading = true;
    }
    
    public abstract void Spread();
    
    public virtual void Extinguish(float amount)
    {
        Intensity -= amount;
        if (Intensity <= 0)
        {
            ExtinguishCompletely();
        }
    }
    
    protected virtual void ExtinguishCompletely()
    {
        if (_fireParticles != null)
        {
            _fireParticles.Stop();
        }
        if (_fireLight != null)
        {
            _fireLight.enabled = false;
        }
        _isSpreading = false;
    }
    
    public virtual void UpdateIntensity(float delta)
    {
        Intensity += delta;
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spreadRadius);
    }
    
    public virtual void Activate()
    {
        _isActive = true;
        if (_fireParticles != null)
        {
            _fireParticles.Play();
        }
    }
    
    public virtual void Deactivate()
    {
        _isActive = false;
        if (_fireParticles != null)
        {
            _fireParticles.Stop();
        }
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if (!_isActive) return;
        
        if (other.CompareTag("Player"))
        {
            // Нанесение урона игроку
            var player = other.GetComponent<IPlayer>();
            if (player != null)
            {
                player.TakeDamage(_damagePerSecond * Time.deltaTime);
            }
        }
    }
} 