using UnityEngine;

[System.Serializable]
public class ScenarioConfig
{
    public int ScenarioType { get; set; }
    public Vector3[] FireStartPositions { get; set; }
    public float[] InitialFireIntensities { get; set; }
    public Vector3[] InteractiveObjectPositions { get; set; }
    public string[] InteractiveObjectTypes { get; set; }
    
    public ScenarioConfig()
    {
        FireStartPositions = new Vector3[0];
        InitialFireIntensities = new float[0];
        InteractiveObjectPositions = new Vector3[0];
        InteractiveObjectTypes = new string[0];
    }
} 