using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Activador_Por_Teclas : MonoBehaviour
{
    [SerializeField] float transitionTime = 2f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject Canvas;
    [SerializeField] bool animationComplete = true;

    [SerializeField] string animatorParameters;

    [SerializeField] List<KeyCode> keys;

    private void Update()
    {
        InteractionKey();
    }

    public void InteractionKey()
    {
        // Verificar si alguna tecla de la lista fue presionada
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(Transition());
                break;
            }
        }
    }

    IEnumerator Transition()
    {
        if (animationComplete)
        {
            animator.SetBool(animatorParameters, true);

            // Esperar la duración
            yield return new WaitForSeconds(transitionTime);

            // Desactivar animación
            animator.SetBool(animatorParameters, false);
            Canvas.SetActive(false);
            animationComplete = false;
        }
    }
}