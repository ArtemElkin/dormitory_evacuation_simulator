using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenDepletionRate = 5f;
    
    [Header("Temperature Settings")]
    [SerializeField] private float maxTemperature = 100f;
    [SerializeField] private float currentTemperature;
    [SerializeField] private float temperatureIncreaseRate = 2f;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent<float> onOxygenChanged;
    public UnityEvent<float> onTemperatureChanged;
    public UnityEvent onPlayerDeath;
    
    private void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
        currentTemperature = 0f;
    }
    
    private void Update()
    {
        // Уменьшение кислорода со временем
        DecreaseOxygen(oxygenDepletionRate * Time.deltaTime);
        
        // Увеличение температуры в зависимости от близости к огню
        // TODO: Добавить проверку близости к огню
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        onHealthChanged?.Invoke(currentHealth / maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void DecreaseOxygen(float amount)
    {
        currentOxygen = Mathf.Max(0, currentOxygen - amount);
        onOxygenChanged?.Invoke(currentOxygen / maxOxygen);
        
        if (currentOxygen <= 0)
        {
            TakeDamage(10f * Time.deltaTime); // Урон от удушья
        }
    }
    
    public void IncreaseTemperature(float amount)
    {
        currentTemperature = Mathf.Min(maxTemperature, currentTemperature + amount);
        onTemperatureChanged?.Invoke(currentTemperature / maxTemperature);
        
        if (currentTemperature >= maxTemperature)
        {
            TakeDamage(5f * Time.deltaTime); // Урон от высокой температуры
        }
    }
    
    private void Die()
    {
        onPlayerDeath?.Invoke();
        // TODO: Добавить логику смерти игрока
    }
} 