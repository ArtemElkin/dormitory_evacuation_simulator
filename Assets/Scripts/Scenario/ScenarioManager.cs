using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    [System.Serializable]
    public class InteractablePrefab
    {
        public string type;
        public GameObject prefab;
    }

    [SerializeField] private ScenarioConfig _currentScenario;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private List<InteractablePrefab> _interactablePrefabs = new List<InteractablePrefab>();
    [SerializeField] private Light[] _roomLights; // Ссылки на освещение в комнатах
    
    [Header("Lighting")]
    [SerializeField] private LightmapData[] _lightmapsWithLights;
    [SerializeField] private LightmapData[] _lightmapsWithoutLights;
    
    private Dictionary<string, GameObject> _interactablePrefabsDict;
    private List<BaseFire> _activeFires = new List<BaseFire>();
    private float _scenarioTimer;
    private bool _isScenarioActive;
    private string _currentTaskId;
    private float _taskTimer;
    private bool _isTaskTimed;
    
    public UnityEvent<string> OnTaskChanged = new UnityEvent<string>();
    public UnityEvent OnScenarioCompleted = new UnityEvent();
    public UnityEvent OnScenarioFailed = new UnityEvent();
    
    public ScenarioConfig CurrentScenario => _currentScenario;
    public string CurrentTaskId => _currentTaskId;
    
    private void Awake()
    {
        // Инициализация словаря префабов
        _interactablePrefabsDict = new Dictionary<string, GameObject>();
        foreach (var prefab in _interactablePrefabs)
        {
            if (!string.IsNullOrEmpty(prefab.type) && prefab.prefab != null)
            {
                _interactablePrefabsDict[prefab.type] = prefab.prefab;
            }
        }
    }
    
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
            _scenarioTimer += Time.deltaTime;
            
            if (_isTaskTimed)
            {
                _taskTimer -= Time.deltaTime;
                if (_taskTimer <= 0)
                {
                    HandleTaskTimeout();
                }
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
        //ClearScenario();
        
        // Установка начальной позиции игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = _currentScenario.playerStartPosition;
            player.transform.rotation = Quaternion.Euler(_currentScenario.playerStartRotation);
        }
        
        // Настройка освещения
        if (_currentScenario.startWithLightsOff)
        {
            SwitchToDarkLightmaps();
        }
        else
        {
            SwitchToLightLightmaps();
        }
        
        // Запуск сценария с задержкой
        Invoke("StartScenario", _currentScenario.fireStartDelay);
        
        
    }
    
    private void SwitchToDarkLightmaps()
    {
        if (_lightmapsWithoutLights != null && _lightmapsWithoutLights.Length > 0)
        {
            LightmapSettings.lightmaps = _lightmapsWithoutLights;
            LightmapSettings.lightmapsMode = LightmapsMode.CombinedDirectional;
        }
    }
    
    private void SwitchToLightLightmaps()
    {
        if (_lightmapsWithLights != null && _lightmapsWithLights.Length > 0)
        {
            LightmapSettings.lightmaps = _lightmapsWithLights;
            LightmapSettings.lightmapsMode = LightmapsMode.CombinedDirectional;
        }
    }
    
    private void StartScenario()
    {
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
        // Установка начального задания
        SetCurrentTask(_currentScenario.initialTaskId);
    }
    
    private void CreateFirePoint(ScenarioConfig.FireSpawnPoint firePoint)
    {
        if (_firePrefab != null)
        {
            GameObject fireObject = Instantiate(_firePrefab, firePoint.position, Quaternion.identity);
            BaseFire fire = fireObject.GetComponent<BaseFire>();
            if (fire != null)
            {
                _activeFires.Add(fire);
            }
        }
    }
    
    private void CreateInteractableObject(ScenarioConfig.InteractableObject interactable)
    {
        if (_interactablePrefabsDict.TryGetValue(interactable.interactionType, out GameObject prefab))
        {
            GameObject obj = Instantiate(prefab, interactable.position, Quaternion.Euler(interactable.rotation));
            // Настройка компонентов интерактивного объекта
            var interactableComponent = obj.GetComponent<BaseInteractable>();
            if (interactableComponent != null)
            {
                // Здесь можно настроить специфические параметры объекта
            }
        }
    }
    
    private void SetCurrentTask(string taskId)
    {
        var task = _currentScenario.tasks.Find(t => t.taskId == taskId);
        if (task != null)
        {
            _currentTaskId = taskId;
            OnTaskChanged.Invoke(task.description);
            Debug.Log("SetCurrentTask");
            // Настройка таймера задания
            _isTaskTimed = task.timeLimit > 0;
            if (_isTaskTimed)
            {
                _taskTimer = task.timeLimit;
            }
        }
    }
    
    private void HandleTaskTimeout()
    {
        // Обработка истечения времени задания
        // Например, для второго сценария - переход к эвакуации
        if (_currentScenario.scenarioType == ScenarioConfig.ScenarioType.ElectricalFire)
        {
            // Активация пожара
            foreach (var fire in _activeFires)
            {
                fire.Activate();
            }
            // Переход к заданию эвакуации
            SetCurrentTask("evacuate");
        }
    }
    
    public void CompleteCurrentTask()
    {
        var currentTask = _currentScenario.tasks.Find(t => t.taskId == _currentTaskId);
        if (currentTask != null && !string.IsNullOrEmpty(currentTask.nextTaskId))
        {
            SetCurrentTask(currentTask.nextTaskId);
        }
        else
        {
            // Завершение сценария
            EndScenario(true);
        }
    }
    
    private void TurnOffLights()
    {
        foreach (var light in _roomLights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }
    
    private void ClearScenario()
    {
        foreach (var fire in _activeFires)
        {
            if (fire != null)
            {
                Destroy(fire.gameObject);
            }
        }
        _activeFires.Clear();
        
        // Удаление всех интерактивных объектов
        var interactables = FindObjectsOfType<BaseInteractable>();
        foreach (var interactable in interactables)
        {
            Destroy(interactable.gameObject);
        }
        
        _isScenarioActive = false;
        _scenarioTimer = 0f;
        _taskTimer = 0f;
        _isTaskTimed = false;
    }
    
    public void EndScenario(bool success)
    {
        _isScenarioActive = false;
        if (success)
        {
            OnScenarioCompleted.Invoke();
        }
        else
        {
            OnScenarioFailed.Invoke();
        }
        Debug.Log($"Сценарий завершен. Успех: {success}");
    }
} 