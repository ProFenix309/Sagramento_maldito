using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Playertransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 2f;

    private void Start()
    {
        StartTransition(false);
    }

    public void StartTransition(bool activatePlayer)
    {

        StartCoroutine(Transition(false));

    }

    IEnumerator Transition(bool activatePlayer)
    {
        // Activar animación
        transitionAnimator.SetBool("Start", true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación

        transitionAnimator.SetBool("Start", false);


        // Activar jugador




    }
}
