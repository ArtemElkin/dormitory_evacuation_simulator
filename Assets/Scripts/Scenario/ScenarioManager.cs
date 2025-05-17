using UnityEngine;
using System.Collections.Generic;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private ScenarioConfig _currentScenario;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Dictionary<string, GameObject> _interactablePrefabs;
    
    private List<BaseFire> _activeFires = new List<BaseFire>();
    private float _scenarioTimer;
    private bool _isScenarioActive;
    
    private void Start()
    {
        if (_currentScenario != null)
        {
            InitializeScenario();
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
    
    private void CreateFirePoint(FirePoint firePoint)
    {
        if (_firePrefab != null)
        {
            GameObject fireObject = Instantiate(_firePrefab, firePoint.transform.position, Quaternion.identity, firePoint.transform);
            BaseFire fire = fireObject.GetComponent<BaseFire>();
            if (fire != null)
            {
                _activeFires.Add(fire);
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