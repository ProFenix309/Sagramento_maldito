using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] Transform door, open, close, target;
    [SerializeField] float speed;
    [SerializeField] Audio_Manager sfx;
    private void Start()
    {
        open.SetParent(null);
        close.SetParent(null);
        sfx = GameObject.Find("Audio Manager").GetComponent<Audio_Manager>();
    }
    public void Interact()
    {
        if (target == open)
        {
            target = close;
            sfx.PlaySFX("Door");
        }
        else
        {
            target = open;
            sfx.PlaySFX("Door");
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
