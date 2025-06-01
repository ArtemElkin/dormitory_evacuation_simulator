using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScenarioUI : MonoBehaviour
{
    [Header("Task UI")]
    [SerializeField] private GameObject _taskPanel;
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private Image _taskTimerFill;
    
    [Header("Evacuation Plan")]
    [SerializeField] private GameObject _evacuationPlanImage;
    
    [Header("Scenario Results")]
    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private TextMeshProUGUI _resultsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    
    [Header("Animation")]
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    
    [SerializeField] private ScenarioManager _scenarioManager;
    private CanvasGroup _taskPanelCanvasGroup;
    private CanvasGroup _resultsPanelCanvasGroup;
    private float _currentTaskTimeLimit;
    private float _currentTaskTimeRemaining;
    
    public GameObject EvacuationPlanImage => _evacuationPlanImage;
    
    private void Start()
    {
        if (_scenarioManager != null)
        {
            _scenarioManager.OnTaskChanged.AddListener(UpdateTaskUI);
            Debug.Log("OnTaskChanged.AddListener");
            _scenarioManager.OnScenarioCompleted.AddListener(() => ShowResults(true));
            _scenarioManager.OnScenarioFailed.AddListener(() => ShowResults(false));
        }
        
        _taskPanelCanvasGroup = _taskPanel.GetComponent<CanvasGroup>();
        _resultsPanelCanvasGroup = _resultsPanel.GetComponent<CanvasGroup>();
        
        // Настройка кнопок
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(RestartScenario);
        }
        
        if (_mainMenuButton != null)
        {
            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
        
        // Изначально скрываем панели
        _taskPanel.SetActive(false);
        Debug.Log("taskPanel.SetActive(false)");
        _resultsPanel.SetActive(false);
        if (_evacuationPlanImage != null)
        {
            _evacuationPlanImage.SetActive(false);
        }
        
        // Блокируем курсор в начале
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (_currentTaskTimeLimit > 0)
        {
            _currentTaskTimeRemaining -= Time.deltaTime;
            if (_taskTimerFill != null)
            {
                _taskTimerFill.fillAmount = _currentTaskTimeRemaining / _currentTaskTimeLimit;
            }
        }
    }
    
    private void UpdateTaskUI(string taskDescription)
    {
        if (_taskText != null)
        {
            _taskText.text = taskDescription;
        }
        
        // Получаем информацию о текущей задаче
        if (_scenarioManager != null && _scenarioManager.CurrentScenario != null)
        {
            var currentTask = _scenarioManager.CurrentScenario.tasks.Find(t => t.taskId == _scenarioManager.CurrentTaskId);
            if (currentTask != null)
            {
                _currentTaskTimeLimit = currentTask.timeLimit;
                _currentTaskTimeRemaining = currentTask.timeLimit;
                
                // Показываем или скрываем таймер в зависимости от наличия временного лимита
                if (_taskTimerFill != null)
                {
                    _taskTimerFill.gameObject.SetActive(_currentTaskTimeLimit > 0);
                    _taskTimerFill.fillAmount = 1f;
                }
            }
        }
        
        // Показываем панель с анимацией
        _taskPanel.SetActive(true);
        Debug.Log("taskPanel.SetActive(true)");
        StartCoroutine(FadeIn(_taskPanelCanvasGroup));
    }
    
    private void ShowResults(bool success)
    {
        // Останавливаем время и разблокируем курсор
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (_resultsText != null)
        {
            _resultsText.text = success ? "Успешная эвакуация" : "Эвакуация провалена";
            _resultsText.color = success ? Color.green : Color.red;
        }
        
        if (_timeText != null)
        {
            // Здесь можно добавить отображение времени прохождения
            _timeText.text = $"Общее время: {Time.time:F1} сек";
        }
        
        // Показываем панель результатов с анимацией
        _resultsPanel.SetActive(true);
        StartCoroutine(FadeIn(_resultsPanelCanvasGroup));
    }
    
    private void RestartScenario()
    {
        // Перезапуск текущего сценария
        if (_scenarioManager != null)
        {
            StartCoroutine(RestartWithFade());
        }
    }
    
    private void ReturnToMainMenu()
    {
        // Возвращаем нормальную скорость времени перед загрузкой меню
        Time.timeScale = 1f;
        // Возврат в главное меню
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;
        
        while (elapsedTime < _fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Используем unscaledDeltaTime для работы с нулевым timeScale
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 1f;
        
        while (elapsedTime < _fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Используем unscaledDeltaTime для работы с нулевым timeScale
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }
    
    private IEnumerator RestartWithFade()
    {
        // Скрываем панель результатов с анимацией
        yield return StartCoroutine(FadeOut(_resultsPanelCanvasGroup));
        _resultsPanel.SetActive(false);
        
        // Возвращаем нормальную скорость времени
        Time.timeScale = 1f;
        
        // Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Перезапускаем сценарий
        _scenarioManager.LoadScenario(_scenarioManager.CurrentScenario);
    }
    
    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении объекта
        if (_scenarioManager != null)
        {
            _scenarioManager.OnTaskChanged.RemoveListener(UpdateTaskUI);
            _scenarioManager.OnScenarioCompleted.RemoveAllListeners();
            _scenarioManager.OnScenarioFailed.RemoveAllListeners();
        }
        
        // Убеждаемся, что время восстановлено при уничтожении объекта
        Time.timeScale = 1f;
    }
} 