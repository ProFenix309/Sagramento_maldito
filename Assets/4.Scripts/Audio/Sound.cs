using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 0.7f;
    
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    
    public bool loop = false;
    public bool is3D = false;
    
    [Range(0f, 500f)]
    public float minDistance = 1f;
    
    [Range(0f, 500f)]
    public float maxDistance = 50f;
    
    [HideInInspector]
    public AudioSource source;
}