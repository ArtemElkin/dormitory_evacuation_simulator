using UnityEngine;
using System.Collections.Generic;

public class ParticleSystemPool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _prefab;
    [SerializeField] private int _initialSize = 3;

    private List<ParticleSystem> _pool = new List<ParticleSystem>();

    private void Awake()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            CreateNewInstance();
        }
    }

    private ParticleSystem CreateNewInstance()
    {
        var ps = Instantiate(_prefab, transform);
        ps.gameObject.SetActive(false);
        _pool.Add(ps);
        return ps;
    }

    public ParticleSystem Get()
    {
        foreach (var ps in _pool)
        {
            if (!ps.gameObject.activeInHierarchy)
            {
                ps.gameObject.SetActive(true);
                return ps;
            }
        }
        return CreateNewInstance();
    }

    public void Release(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ps.transform.SetParent(transform);
        StartCoroutine(DeactivateWhenDone(ps));
    }

    private System.Collections.IEnumerator DeactivateWhenDone(ParticleSystem ps)
    {
        while (ps.IsAlive(true))
        {
            yield return null;
        }
        ps.gameObject.SetActive(false);
    }
} 