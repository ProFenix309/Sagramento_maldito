using UnityEngine;
using System.Collections;

public class TransparenciadelTexto : MonoBehaviour
{
    public float transitionTime = 2f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject Canvas;
    [SerializeField] string parametros;
    [SerializeField] bool animationComplete = true;


    public void Desvanecimiento()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        if (animationComplete)
        {
            animator.SetBool(parametros, true);

            // Esperar la duración
            yield return new WaitForSeconds(transitionTime);

            // Desactivar animación
            animator.SetBool(parametros, false);
            Canvas.SetActive(false);
            animationComplete = false;
        }
    }
}
