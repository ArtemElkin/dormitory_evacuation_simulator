using UnityEngine;
using UnityEngine.UI;
using Scripts.Interfaces;

public class EvacuationPlan : BaseInteractable
{
    [SerializeField] private string _studyPrompt = "Press E to study evacuation plan";
    [SerializeField] private string _alreadyStudiedPrompt = "You have already studied this plan";
    
    private bool _isResearched = false;
    private ScenarioManager _scenarioManager;
    private GameObject _evacuationPlanImage;
    
    protected override void Start()
    {
        base.Start();
        _interactionPrompt = _studyPrompt;
        
        _scenarioManager = FindObjectOfType<ScenarioManager>();
        
        // Получаем ссылку на изображение из ScenarioUI
        var scenarioUI = FindObjectOfType<ScenarioUI>();
        if (scenarioUI != null)
        {
            _evacuationPlanImage = scenarioUI.EvacuationPlanImage;
            if (_evacuationPlanImage != null)
            {
                _evacuationPlanImage.SetActive(false);
            }
        }
    }
    
    public override void Interact(IPlayer player)
    {
        if (!_isResearched)
        {
            _isResearched = true;
            _interactionPrompt = _alreadyStudiedPrompt;
            
            // Проверяем, что текущая задача связана с планом эвакуации
            if (_scenarioManager != null && _scenarioManager.CurrentScenario != null)
            {
                var currentTask = _scenarioManager.CurrentScenario.tasks.Find(t => t.taskId == _scenarioManager.CurrentTaskId);
                if (currentTask != null && currentTask.taskId == "find_evacuation_plan")
                {
                    _scenarioManager.CompleteCurrentTask();
                }
            }
        }
    }
    
    private void Update()
    {
        if (_isResearched && _evacuationPlanImage != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _evacuationPlanImage.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                _evacuationPlanImage.SetActive(false);
            }
        }
    }
} 