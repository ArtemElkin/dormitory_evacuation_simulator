using UnityEngine;
using System.Collections.Generic;

public class DefaultFire : BaseFire
{
    [Header("Default Fire Settings")]
    [SerializeField] private float _spreadChance = 0.5f;
    [SerializeField] private GameObject _firePrefab;
    
    public override void Spread()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _spreadRadius, _spreadableLayers);
        foreach (Collider collider in colliders)
        {
            var fire = collider.GetComponent<DefaultFire>();
            if (fire == null || _spreadFires.Contains(fire))
                continue;

            if (Random.value < _spreadChance)
            {
                // Создаем новый очаг возгорания
                Vector3 spreadPosition = collider.transform.position;
                GameObject newFire = Instantiate(_firePrefab, spreadPosition, Quaternion.identity);
                DefaultFire fireComponent = newFire.GetComponent<DefaultFire>();
                
                if (fireComponent != null)
                {
                    _spreadFires.Add(fireComponent);
                    fireComponent.StartSpreading();
                }
            }
        }
    }
    
    protected override void ExtinguishCompletely()
    {
        base.ExtinguishCompletely();
        // Дополнительные эффекты тушения для обычного огня
        // Например, дым, пар и т.д.
    }
} 