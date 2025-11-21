using UnityEngine;
using System.Collections;

public class Playertransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 2f;

    private void OnEnable()
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
        gameObject.SetActive(true);
        transitionAnimator.SetBool("End", true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación
        transitionAnimator.SetBool("End", false);
        gameObject.SetActive(false);
    }
}
