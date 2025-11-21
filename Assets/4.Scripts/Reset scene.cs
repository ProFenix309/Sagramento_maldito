using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetscene : MonoBehaviour
{
    [SerializeField] string playerName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player_Original")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
