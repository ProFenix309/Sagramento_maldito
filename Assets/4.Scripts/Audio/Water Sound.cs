using UnityEngine;

public class WaterSound : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        SoundWater();
    }

    public void SoundWater()
    {
        AudioManager.Instance.PlaySFX3D("Water", transform.position);
    }
}
