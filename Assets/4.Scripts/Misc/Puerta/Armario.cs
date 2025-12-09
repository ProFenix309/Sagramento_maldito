using UnityEngine;

public class Armario : MonoBehaviour, Interactable
{
    [SerializeField] Transform armario, close, open, target;
    [SerializeField] float speed;

    private void Start()
    {
        open.SetParent(null);
        close.SetParent(null);
        target = close;
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
            AudioManager.Instance.StopMusic("Armario");
        }
    }

    void Update()
    {
        if (Vector3.Distance(armario.position, target.position) > 0.01f)
        {
            armario.position = Vector3.MoveTowards(armario.position, target.position, speed * Time.deltaTime);
        }
    }
    private void PlaySFXSound()
    {
        AudioManager.Instance.PlaySFX3D("Armario", transform.position);
    }

}