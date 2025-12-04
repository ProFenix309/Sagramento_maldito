using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string nameSound;
    public AudioSource audioSource;
    public AudioClip clip;
    public float minDistance;
    public float maxDistance;
    public float spacialBlend = 1f;
    public bool loop = true;
    public float volume = 1f;
    public bool usePlayOneShot = false;

}
