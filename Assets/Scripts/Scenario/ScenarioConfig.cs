using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewScenario", menuName = "Fire Evacuation/Scenario Config")]
public class ScenarioConfig : ScriptableObject
{
    public enum ScenarioType
    {
        ShortCircuit,    // Короткое замыкание
        ElectricalFire,  // Пожар от электроприбора
        CarelessFire     // Неосторожное обращение с огнем
    }

    [Serializable]
    public class InteractableObject
    {
        public int objectId;
        public Vector3 position;
        public Vector3 rotation;
        public string interactionType;
    }

    [Serializable]
    public class Task
    {
        public string taskId;
        public string description;
        public string nextTaskId; // ID следующего задания
        public float timeLimit;   // Ограничение по времени (если есть)
    }

    [Serializable]
    public class FireSpawnPoint
    {
        public Vector3 position;
        public float initialIntensity = 1f;
        public float spreadRate = 0.1f;
    }

    [Header("Scenario Settings")]
    public string scenarioName;
    public string description;
    public ScenarioType scenarioType;
    public Vector3 playerStartPosition;
    public Vector3 playerStartRotation;

    [Header("Tasks")]
    public List<Task> tasks = new List<Task>();
    public string initialTaskId;

    [Header("Fire Points")]
    public List<FireSpawnPoint> firePoints = new List<FireSpawnPoint>();
    public float fireStartDelay; // Задержка до начала пожара

    [Header("Interactable Objects")]
    public List<InteractableObject> interactableObjects = new List<InteractableObject>();

    [Header("Environment Settings")]
    public float initialSmokeDensity = 0.2f;
    public float smokeIncreaseRate = 0.1f;
    public float maxSmokeDensity = 1f;
    public bool startWithLightsOff = false; // Для третьего сценария

    [Header("Difficulty Settings")]
    public float playerHealthMultiplier = 1f;
    public float oxygenDepletionMultiplier = 1f;
    public float temperatureIncreaseMultiplier = 1f;
} 