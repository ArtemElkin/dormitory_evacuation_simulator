using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IPlayer
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
    
    // IPlayer interface implementation
    public float Health 
    { 
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            onHealthChanged?.Invoke(currentHealth / maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    
    public float Oxygen 
    { 
        get => currentOxygen;
        set
        {
            currentOxygen = Mathf.Clamp(value, 0, maxOxygen);
            onOxygenChanged?.Invoke(currentOxygen / maxOxygen);
        }
    }
    
    public float Temperature 
    { 
        get => currentTemperature;
        set
        {
            currentTemperature = Mathf.Clamp(value, 0, maxTemperature);
            onTemperatureChanged?.Invoke(currentTemperature / maxTemperature);
        }
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
        currentTemperature = 0f;
    }
    
    private void Update()
    {
        // Уменьшение кислорода со временем
        ConsumeOxygen(oxygenDepletionRate * Time.deltaTime);
        
        // Увеличение температуры в зависимости от близости к огню
        // TODO: Добавить проверку близости к огню
    }
    
    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
    
    public void ConsumeOxygen(float amount)
    {
        Oxygen -= amount;
        if (Oxygen <= 0)
        {
            TakeDamage(10f * Time.deltaTime); // Урон от удушья
        }
    }
    
    public void UpdateTemperature(float delta)
    {
        Temperature += delta;
        if (Temperature >= maxTemperature)
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