using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Color _healthColor = new Color(1f, 0.2f, 0.2f);
    
    [Header("Oxygen Bar")]
    [SerializeField] private Image _oxygenBarFill;
    [SerializeField] private TextMeshProUGUI _oxygenText;
    [SerializeField] private Color _oxygenColor = new Color(0.2f, 0.6f, 1f);
    
    [Header("Temperature Bar")]
    [SerializeField] private Image _temperatureBarFill;
    [SerializeField] private TextMeshProUGUI _temperatureText;
    [SerializeField] private Color _temperatureColor = new Color(1f, 0.5f, 0.2f);
    
    private PlayerStats _playerStats;
    
    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        if (_playerStats != null)
        {
            // Подписываемся на события изменения характеристик
            _playerStats.onHealthChanged.AddListener(UpdateHealthBar);
            _playerStats.onOxygenChanged.AddListener(UpdateOxygenBar);
            _playerStats.onTemperatureChanged.AddListener(UpdateTemperatureBar);
            
            // Устанавливаем начальные цвета
            _healthBarFill.color = _healthColor;
            _oxygenBarFill.color = _oxygenColor;
            _temperatureBarFill.color = _temperatureColor;
            
            // Инициализируем бары
            UpdateHealthBar(_playerStats.Health / 100f);
            UpdateOxygenBar(_playerStats.Oxygen / 100f);
            UpdateTemperatureBar(_playerStats.Temperature / 100f);
        }
        else
        {
            Debug.LogError("PlayerStats not found in the scene!");
        }
    }
    
    private void OnDestroy()
    {
        if (_playerStats != null)
        {
            _playerStats.onHealthChanged.RemoveListener(UpdateHealthBar);
            _playerStats.onOxygenChanged.RemoveListener(UpdateOxygenBar);
            _playerStats.onTemperatureChanged.RemoveListener(UpdateTemperatureBar);
        }
    }
    
    private void UpdateHealthBar(float normalizedValue)
    {
        _healthBarFill.fillAmount = normalizedValue;
        _healthText.text = $"{(normalizedValue * 100):F0}%";
    }
    
    private void UpdateOxygenBar(float normalizedValue)
    {
        _oxygenBarFill.fillAmount = normalizedValue;
        _oxygenText.text = $"{(normalizedValue * 100):F0}%";
    }
    
    private void UpdateTemperatureBar(float normalizedValue)
    {
        _temperatureBarFill.fillAmount = normalizedValue;
        _temperatureText.text = $"{(normalizedValue * 100):F0}%";
    }
} 