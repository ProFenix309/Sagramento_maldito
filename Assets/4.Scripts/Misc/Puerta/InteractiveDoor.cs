using UnityEngine;

public class InteractiveDoor : MonoBehaviour
{
    //Variables de velocidad, angulo y direccion
    public float speed, angle;
    public Vector3 direction;

    //Validar estados de la puerta 
    public bool checkOpen;
    public bool open;

    void Start()
    {
        angle = transform.eulerAngles.y;
    }
    void Update()
    {
        //Operador ternario que cambia entre verdadero y falso
        open = transform.eulerAngles.y < 90 ? false : true;

        //Calcular y aplicar la ritacin a la puerta
        if (Mathf.Round(transform.eulerAngles.y) != angle)
        {
            transform.Rotate(direction * speed);
        }

        //Condicion de apertura de la puerta
        if (Input.GetKeyDown(KeyCode.E) && checkOpen == true && open == false)
        {
            angle = 90;
            direction = Vector3.up;
        }

        //Condicion de cierre de la puerta
        else if (Input.GetKeyDown(KeyCode.E) && checkOpen == true && open == true)
        {
            angle = 0;
            direction = Vector3.down;
        }
    }

    //Detectar si el personaje esta serca de la puerta
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkOpen = true;
        }
    }

    //Detectar si no esta serca de la puerta
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkOpen = false;
        }
    }
}
