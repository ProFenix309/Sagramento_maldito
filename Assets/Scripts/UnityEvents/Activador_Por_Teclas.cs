using UnityEngine;
using System.Collections;

public class Activador_Por_Teclas : MonoBehaviour
{
    [SerializeField] float transitionTime = 2f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject Canvas;
    [SerializeField] bool animationComplete = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(Transition());
        }
    }
    IEnumerator Transition()
    {
        if (animationComplete)
        {
            animator.SetBool("BoolInput", true);

            // Esperar la duración
            yield return new WaitForSeconds(transitionTime);

            // Desactivar animación
            animator.SetBool("BoolInput", false);
            Canvas.SetActive(false);
            animationComplete = false;
        }
    }

}
