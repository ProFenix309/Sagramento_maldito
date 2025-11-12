using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 2f;

    public void LoadScene(string sceneName)
    {

        StartCoroutine(Transition(sceneName));

    }

    IEnumerator Transition(string sceneName)
    {
        // Activar animación
        transitionAnimator.SetBool("Start", true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación

        transitionAnimator.SetBool("Start", false);


        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
}

