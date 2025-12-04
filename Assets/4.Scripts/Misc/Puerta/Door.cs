using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] Transform door, open, close, target;
    [SerializeField] float speed;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private void Start()
    {
        open.SetParent(null);
        close.SetParent(null);
        audioSource = GetComponent<AudioSource>();
    }
    public void Interact()
    {
        if (target == open)
        {
            target = close;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            target = open;
            audioSource.PlayOneShot(audioClip);
        }
    }
    private void Update()
    {
        if (transform.rotation != target.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, speed * Time.deltaTime);
        }
    }
}
