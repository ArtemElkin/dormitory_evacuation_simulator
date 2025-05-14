using UnityEngine;

public interface IFire
{
    float Intensity { get; set; }
    float DamagePerSecond { get; }
    
    void Spread();
    void Extinguish(float amount);
    void UpdateIntensity(float delta);
} 