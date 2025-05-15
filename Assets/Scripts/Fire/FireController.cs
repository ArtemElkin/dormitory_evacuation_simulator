using UnityEngine;
using System.Collections.Generic;

public class FireController : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private float _maxIntensity = 100f;
    [SerializeField] private float _currentIntensity;
    [SerializeField] private float _spreadRate = 5f;
    [SerializeField] private float _damagePerSecond = 10f;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private Light _fireLight;
    
    [Header("Spread Settings")]
    [SerializeField] private float _spreadRadius = 2f;
    [SerializeField] private LayerMask _spreadableLayers;
    
    private List<FireController> _spreadFires = new List<FireController>();
    private bool _isSpreading = false;
    
    private void Start()
    {
        _currentIntensity = _maxIntensity;
        if (_fireParticles != null)
        {
            _fireParticles.Play();
        }
    }
    
    private void Update()
    {
        if (_isSpreading)
        {
            SpreadFire();
        }
        
        // Нанесение урона игроку при контакте
        Collider[] colliders = Physics.OverlapSphere(transform.position, _spreadRadius);
        foreach (Collider collider in colliders)
        {
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(_damagePerSecond * Time.deltaTime);
                playerStats.IncreaseTemperature(10f * Time.deltaTime);
            }
        }
    }
    
    public void StartSpreading()
    {
        _isSpreading = true;
    }
    
    private void SpreadFire()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _spreadRadius, _spreadableLayers);
        foreach (Collider collider in colliders)
        {
            if (!_spreadFires.Contains(collider.GetComponent<FireController>()))
            {
                // TODO: Создать новый очаг возгорания
            }
        }
    }
    
    public void Extinguish(float amount)
    {
        _currentIntensity = Mathf.Max(0, _currentIntensity - amount);
        
        if (_currentIntensity <= 0)
        {
            ExtinguishCompletely();
        }
    }
    
    private void ExtinguishCompletely()
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
        // TODO: Добавить эффект тушения
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spreadRadius);
    }
} 