using UnityEngine;
using System.Collections.Generic;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private ScenarioConfig _currentScenario;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Dictionary<string, GameObject> _interactablePrefabs;
    
    private List<FireController> _activeFires = new List<FireController>();
    private float _scenarioTimer;
    private bool _isScenarioActive;
    
    private void Start()
    {
        if (_currentScenario != null)
        {
            InitializeScenario();
        }
    }
    
    private void Update()
    {
        if (_isScenarioActive)
        {
            _scenarioTimer -= Time.deltaTime;
            if (_scenarioTimer <= 0)
            {
                EndScenario(false);
            }
        }
    }
    
    public void LoadScenario(ScenarioConfig scenario)
    {
        _currentScenario = scenario;
        InitializeScenario();
    }
    
    private void InitializeScenario()
    {
        // Очистка предыдущего сценария
        ClearScenario();
        
        // Инициализация таймера
        _scenarioTimer = _currentScenario.timeLimit;
        _isScenarioActive = true;
        
        // Создание очагов возгорания
        foreach (var firePoint in _currentScenario.firePoints)
        {
            CreateFirePoint(firePoint);
        }
        
        // Размещение интерактивных объектов
        foreach (var interactable in _currentScenario.interactableObjects)
        {
            CreateInteractableObject(interactable);
        }
    }
    
    private void CreateFirePoint(ScenarioConfig.FirePoint firePoint)
    {
        if (_firePrefab != null)
        {
            GameObject fireObject = Instantiate(_firePrefab, firePoint.position, Quaternion.identity);
            FireController fireController = fireObject.GetComponent<FireController>();
            if (fireController != null)
            {
                _activeFires.Add(fireController);
            }
        }
    }
    
    private void CreateInteractableObject(ScenarioConfig.InteractableObject interactable)
    {
        if (_interactablePrefabs.TryGetValue(interactable.interactionType, out GameObject prefab))
        {
            GameObject obj = Instantiate(prefab, interactable.position, interactable.rotation);
            // TODO: Настроить компоненты интерактивного объекта
        }
    }
    
    private void ClearScenario()
    {
        // Удаление всех активных пожаров
        foreach (var fire in _activeFires)
        {
            if (fire != null)
            {
                Destroy(fire.gameObject);
            }
        }
        _activeFires.Clear();
        
        // TODO: Удаление всех интерактивных объектов
        
        _isScenarioActive = false;
    }
    
    public void EndScenario(bool success)
    {
        _isScenarioActive = false;
        // TODO: Показать результаты сценария
        Debug.Log($"Сценарий завершен. Успех: {success}");
    }
} 