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
        }
        else
        {
            target = open;
        }
    }

    void Update()
    {
        if (Vector3.Distance(armario.position, target.position) > 0.01f)
        {
            armario.position = Vector3.MoveTowards(
                armario.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }
}