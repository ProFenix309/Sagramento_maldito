using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 2f;

    private void Awake()
    {
        transitionAnimator = GetComponent<Animator>();
        

    }
    private void Start()
    {
        StartCoroutine(StartTransition());
    }
    public IEnumerator StartTransition()
    {

        // Activar animación
        transitionAnimator.SetBool("End", true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación
        transitionAnimator.SetBool("End", false);

        // Cargar la escena
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void LoadScene(int sceneName)
    {
        
        StartCoroutine(Transition(sceneName));

    }

    public IEnumerator Transition(int sceneNumber)
    {
        // Activar animación
        transitionAnimator.SetBool("Start", true);

        // Esperar la duración
        yield return new WaitForSeconds(transitionTime);

        // Desactivar animación
        transitionAnimator.SetBool("Start", false);
        
        // Cargar la escena
        SceneManager.LoadScene(sceneNumber);

       
    }
}

