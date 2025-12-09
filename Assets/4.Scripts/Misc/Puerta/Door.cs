using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] Transform door, open, close, target;
    [SerializeField] float speed;
    private void Start()
    {
        open.SetParent(null);
        close.SetParent(null);
    }
    public void Interact()
    {
        if (target == open)
        {
            target = close;
            PlaySFXSound();
        }
        else
        {
            target = open;
            PlaySFXSound();
        }
    }

    private void Update()
    {
        if (transform.rotation != target.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, speed * Time.deltaTime);
        }
    }

    private void PlaySFXSound()
    {
        AudioManager.Instance.PlaySFX3D("Door",transform.position);
    }
}
