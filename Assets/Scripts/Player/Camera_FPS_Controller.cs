using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Camera_FPS_Controller : MonoBehaviour
{

    public Transform player;

    public float mouseX;
    public float mouseY;

    //Sencivilidad a la que se muenve el mause 
    public float sencitivily;

    public float xRotation;

    // Update is called once per frame
    void Update()
    {
        //Detectar el movimiento del mause
        mouseX = Input.GetAxis("Mouse X") * sencitivily * Time.deltaTime; //Time.deltatime estandariza los frimes para que
        mouseY = Input.GetAxis("Mouse Y") * sencitivily * Time.deltaTime; //todos las gamas de PC lo ejecute de manera igual    

        //Toma la rotacion en Y para rotar la camara
        player.Rotate(player.transform.up * mouseX);

        //Con estas lineas podemos limitar rotacion del mause
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -50, 50);

        //Se le está dando la rotacion a la camara en eje X
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void Start()
    {
        //Es para ocultar el cursor en la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }
}
