using UnityEngine;

public class FloatingBoard : MonoBehaviour
{
    public float floatHeight = -0.5f;  // Altura del rebote
    public float floatSpeed = 2f;      // Velocidad del rebote
    public float returnSpeed = 2f;     // Velocidad para volver a la posición original

    public bool isPlayerOnBoard = false;

    private Vector3 originalPosition;

    void Start()
    {
        // Guardamos la posición base
        originalPosition = transform.position;

        // Inicia bajada
        transform.position = new Vector3(
            originalPosition.x,
            originalPosition.y + floatHeight,
            originalPosition.z
        );
    }

    void Update()
    {
        if (isPlayerOnBoard)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

            transform.position = new Vector3(
                originalPosition.x,
                newY,
                originalPosition.z
            );
        }
        else
        {
            // Regresa lentamente a la posición original con Lerp
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(originalPosition.x, originalPosition.y + floatHeight, originalPosition.z),
                Time.deltaTime * returnSpeed
            );
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBoard = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBoard = false;
        }
    }
}
