using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (other.gameObject.layer == 7)
        {
            gameManager.playerPosition = new();
            SceneManager.LoadScene(gameManager.loadAct + 1); 
        }
    }
}
