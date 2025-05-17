using UnityEngine;
using System;
[Serializable]
public class FirePoint : MonoBehaviour
{
    public float initialIntensity;
    public float spreadRate;
    private Transform _transform;
    public Vector3 Position
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }
    private void Awake()
    {
        _transform = transform;
    }
}