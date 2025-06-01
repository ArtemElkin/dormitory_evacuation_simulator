using UnityEngine;
using Scripts.Interfaces;

public class ExitDoor : BaseInteractable
{
    [SerializeField] private string _interactPrompt = "Press E to exit";
    [SerializeField] private string _lockedPrompt = "Door is locked";
    
    private bool _isUnlocked = false;
    private ScenarioManager _scenarioManager;
    
    protected override void Start()
    {
        base.Start();
        _interactionPrompt = _lockedPrompt;
        _scenarioManager = FindObjectOfType<ScenarioManager>();
    }
    
    public override void Interact(IPlayer player)
    {
        if (!_isUnlocked)
        {
            _isUnlocked = true;
            _interactionPrompt = _interactPrompt;
            
            // Проверяем, что текущая задача связана с эвакуацией
            if (_scenarioManager != null && _scenarioManager.CurrentScenario != null)
            {
                var currentTask = _scenarioManager.CurrentScenario.tasks.Find(t => t.taskId == _scenarioManager.CurrentTaskId);
                if (currentTask != null && currentTask.taskId == "evacuate")
                {
                    _scenarioManager.CompleteCurrentTask();
                }
            }
        }
        else
        {
            // Здесь можно добавить анимацию открытия двери
            // и переход на следующий уровень или завершение игры
            if (_scenarioManager != null)
            {
                _scenarioManager.EndScenario(true);
            }
        }
    }
} 