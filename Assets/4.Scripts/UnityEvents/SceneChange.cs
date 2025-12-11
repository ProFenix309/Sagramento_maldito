using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
