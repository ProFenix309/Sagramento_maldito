using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    Inventory inventory;
    PlayerController_Original playerController;
    PlayerMovement playerMovement;
    Camera_FPS_Controller cameraController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (GameObject.Find("Player Modular") && inventory == null)
        {
            Debug.Log(inventory = GameObject.Find("Player Modular").GetComponent<Inventory>());
            inventory = GameObject.Find("Player Modular").GetComponent<Inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

}
