using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiradaCamara : MonoBehaviour
{
    public float Velocidad = 100f;
    float RotacionX = 0f;

    public Transform Jugador;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        float MauseX = Input.GetAxis("Mouse X") * Velocidad * Time.deltaTime;
        float MauseY = Input.GetAxis("Mouse Y") * Velocidad * Time.deltaTime;


        RotacionX -= MauseY;
        RotacionX = Mathf.Clamp (RotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(RotacionX, 0f, 0f);
        Jugador.Rotate(Vector3.up * MauseX);

    }
}
