using UnityEngine;
using System.Collections;

public class TransparenciadelTexto : MonoBehaviour
{
    public Animator transitionAnimator;
    public string ValueName;
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
        transitionAnimator.SetBool(ValueName, true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación
        transitionAnimator.SetBool(ValueName, false);
       
        gameObject.SetActive(false);
    }
}
