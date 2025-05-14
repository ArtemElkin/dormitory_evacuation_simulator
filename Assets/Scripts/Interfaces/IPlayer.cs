using UnityEngine;

public interface IPlayer
{
    float Health { get; set; }
    float Oxygen { get; set; }
    float Temperature { get; set; }
    
    void TakeDamage(float damage);
    void ConsumeOxygen(float amount);
    void UpdateTemperature(float delta);
} 