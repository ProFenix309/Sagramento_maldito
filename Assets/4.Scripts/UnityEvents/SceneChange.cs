using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
           SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1);
    }
}
