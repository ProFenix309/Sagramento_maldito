using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    PlayerController_Original player;
    void Update()
    {
        if (GameObject.Find("Player_Original(Clone)"))
        {
            player = GameObject.Find("Player_Original(Clone)").GetComponent<PlayerController_Original>();
        }
        if(player == null )
        {
            gameObject.SetActive(false);
        }
    }
}
