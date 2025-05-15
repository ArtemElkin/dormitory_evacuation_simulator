using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewScenario", menuName = "Fire Evacuation/Scenario Config")]
public class ScenarioConfig : ScriptableObject
{
    [Serializable]
    public class FirePoint
    {
        public Vector3 position;
        public float initialIntensity;
        public float spreadRate;
    }
    
    [Serializable]
    public class InteractableObject
    {
        public string objectId;
        public Vector3 position;
        public Quaternion rotation;
        public string interactionType;
    }
    
    [Header("Scenario Settings")]
    public string scenarioName;
    public string description;
    public float timeLimit = 300f; // 5 минут
    
    [Header("Fire Points")]
    public List<FirePoint> firePoints = new List<FirePoint>();
    
    [Header("Interactable Objects")]
    public List<InteractableObject> interactableObjects = new List<InteractableObject>();
    
    [Header("Environment Settings")]
    public float initialSmokeDensity = 0.2f;
    public float smokeIncreaseRate = 0.1f;
    public float maxSmokeDensity = 1f;
    
    [Header("Difficulty Settings")]
    public float playerHealthMultiplier = 1f;
    public float oxygenDepletionMultiplier = 1f;
    public float temperatureIncreaseMultiplier = 1f;
} 